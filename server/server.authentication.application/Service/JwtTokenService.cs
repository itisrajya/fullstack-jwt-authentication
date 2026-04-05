using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using server.authentication.application.IService;
using server.authentication.data.Entities;

namespace server.authentication.application.Service
{
	public class JwtTokenService : IJwtTokenService
	{
		private readonly string _key;
		private readonly string _issuer;
		private readonly int _expirationMinutes;

		public JwtTokenService(IConfiguration configuration)
		{
			_key = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
			_issuer = configuration["Jwt:Issuer"] ?? string.Empty;
			if (!int.TryParse(configuration["Jwt:ExpirationInMinutes"], out _expirationMinutes))
				_expirationMinutes = 60;
		}

		public string GenerateToken(User user)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Email, user.Email)
			};

			// Audience set to the username so the token is specific to the user
			var token = new JwtSecurityToken(
				issuer: string.IsNullOrWhiteSpace(_issuer) ? null : _issuer,
				audience: user.Username,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}