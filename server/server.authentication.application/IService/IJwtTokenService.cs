

namespace server.authentication.application.IService;
public interface IJwtTokenService
{
	string GenerateToken(User user);
}
