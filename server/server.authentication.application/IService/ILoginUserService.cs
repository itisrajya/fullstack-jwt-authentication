namespace server.authentication.application.IService;
public interface ILoginUserService
{
	Task<User> Login(string email, string password);
}
