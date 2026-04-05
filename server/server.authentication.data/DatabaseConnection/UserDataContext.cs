using Microsoft.EntityFrameworkCore;
using server.authentication.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.data.DatabaseConnection
{
    public class UserDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDataContext(DbContextOptions<UserDataContext> options) : base(options) 
        {

        }
    }
}