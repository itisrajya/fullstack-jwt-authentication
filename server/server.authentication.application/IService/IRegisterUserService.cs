using server.authentication.data.Entities;

namespace server.authentication.application.IService
{
    public interface IRegisterUserService
    {
        Task<User> RegisterUser(string username, string email, string password);
    }
}
