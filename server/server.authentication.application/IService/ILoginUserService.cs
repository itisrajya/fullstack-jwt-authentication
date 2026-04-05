using server.authentication.data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.application.IService
{
	public interface ILoginUserService
	{
		Task<User> Login(string email, string password);
	}
}
