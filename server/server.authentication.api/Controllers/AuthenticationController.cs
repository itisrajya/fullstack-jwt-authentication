using Microsoft.AspNetCore.Mvc;
using server.authentication.application.IService;
using server.authentication.application.Service;
using server.authentication.contracts.DTOs;

namespace server.authentication.api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IRegisterUserService _registerUserService;
		private readonly ILoginUserService _loginService;
		private readonly IJwtTokenService _jwtTokenService;
		public AuthenticationController(IRegisterUserService registerUserService, ILoginUserService loginService, IJwtTokenService jwtTokenService)
		{
			_registerUserService = registerUserService;
			_loginService = loginService;
			_jwtTokenService = jwtTokenService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
		{
			if (!ModelState.IsValid)
				return BadRequest(new { Message = "Invalid data provided.", Errors = ModelState });

			try
			{
				var registrationRequest = await _registerUserService.RegisterUser(request.Username, request.Email, request.Password);

				return Ok(new { Message = "Registration was sucessfull" });
			}
			catch (ArgumentException ex)
			{
				return BadRequest(new { Message = ex.Message });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "An error occurred during registration.", Details = ex.Message });
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginUserDto request)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				var user = await _loginService.Login(request.Email, request.Password);
				var token = _jwtTokenService.GenerateToken(user);
				return Ok(token);
			}
			catch (ArgumentException ex)
			{
				return Unauthorized(new { Message = ex.Message });
			}
		}
	}
}
