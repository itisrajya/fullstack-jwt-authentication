namespace server.authentication.application.Service;
public class LoginUserService : ILoginUserService
{
	private readonly UserDataContext _context;

	public LoginUserService(UserDataContext context)
	{
		_context = context;
	}

	public async Task<User> Login(string email, string password)
	{
		var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

		if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
			throw new ArgumentException("Invalid email or password.");
		return user;
	}
}
