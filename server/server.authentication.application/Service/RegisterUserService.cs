using server.authentication.application.IService;
using server.authentication.data.DatabaseConnection;
using server.authentication.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.Service
{
    public class RegisterUserService : IRegisterUserService
    {
        private readonly UserDataContext _context;
        private readonly IValidEmailService _validEmailService;
        private readonly IUserCheckEmailExistService _userCheckEmailExistService;
        public RegisterUserService(UserDataContext context, IValidEmailService validEmailService, IUserCheckEmailExistService userCheckEmailExistService)
        {
            _context = context;
            _validEmailService = validEmailService;
            _userCheckEmailExistService = userCheckEmailExistService;
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => "!@#$%^&*()".Contains(ch));
        }

        public async Task<User> RegisterUser(string username, string email, string password)
        {
            if (!_validEmailService.IsValidEmailFormat(email) || !await _validEmailService.HasValidMxRecords(email))
            {
                throw new ArgumentException("Invalid email address.");
            }

            if (await _userCheckEmailExistService.IsEmailTaken(email))
            {
                throw new ArgumentException("User with this email already exists.");
            }

            if (!IsPasswordValid(password))
            {
                throw new ArgumentException("Password must be at least 8 characters long, contain an uppercase letter, lowercase letter, number, and special character.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                Email = email,
                Password = passwordHash,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

    }
}
