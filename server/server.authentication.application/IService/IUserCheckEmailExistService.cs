using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.IService
{
    public interface IUserCheckEmailExistService
    {
        Task<bool> IsEmailTaken(string email);
    }
}
