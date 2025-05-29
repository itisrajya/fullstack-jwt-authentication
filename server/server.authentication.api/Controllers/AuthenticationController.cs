using Microsoft.AspNetCore.Mvc;
using server.authentication.application.IService;
using server.authentication.contracts.DTOs;

namespace server.authentication.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IRegisterUserService _registerUserService;
        public AuthenticationController(IRegisterUserService registerUserService)
        {
            _registerUserService = registerUserService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid data provided.", Errors = ModelState });
            }

            try
            {
                var registrationRequest = await _registerUserService.RegisterUser(request.Username, request.Email, request.Password);

                return Ok(new
                {
                    Message = "Registration was sucessfull",
                    RegistrationRequest = registrationRequest
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred during registration.",
                    Details = ex.Message
                });
            }
        }
    }
}
