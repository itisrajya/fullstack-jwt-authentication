using Microsoft.AspNetCore.Mvc;
using server.authentication.application.IService;
using server.authentication.contracts.DTOs;

namespace server.authentication.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IPasswordResetService _passwordResetService;

        public PasswordController(IPasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        // TODO: Add rate limiting middleware / attribute on this endpoint to mitigate abuse (e.g. AspNetCoreRateLimit).
        [HttpPost("forgot")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid data provided.", Errors = ModelState });

            // Always return the same generic response, regardless of whether the email exists,
            // to avoid leaking account existence.
            await _passwordResetService.RequestPasswordReset(request.Email);

            return Ok(new { Message = "If an account with that email exists, a password reset link has been sent." });
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Invalid data provided.", Errors = ModelState });

            try
            {
                await _passwordResetService.ResetPassword(request.Email, request.Token, request.NewPassword);
                return Ok(new { Message = "Your password has been reset successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
