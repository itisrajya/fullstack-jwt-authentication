using Microsoft.EntityFrameworkCore;
using server.authentication.data.DatabaseConnection;
using server.authentication.data.Entities;
using server.authentication.data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDataContext _context;

        public UserRepository(UserDataContext context)
        {
            _context = context; 
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}