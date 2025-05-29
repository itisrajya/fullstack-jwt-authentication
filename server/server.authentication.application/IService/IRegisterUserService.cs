using server.authentication.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.IService
{
    public interface IRegisterUserService
    {
        Task<User> RegisterUser(string username, string email, string password);
    }
}
