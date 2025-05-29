using Microsoft.EntityFrameworkCore;
using server.authentication.application.IService;
using server.authentication.data.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.Service
{
    public class UserCheckEmailExistService : IUserCheckEmailExistService
    {
        private readonly UserDataContext _context;
        public UserCheckEmailExistService(UserDataContext context) 
        {
            _context = context;
        }
        public async Task<bool> IsEmailTaken(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
