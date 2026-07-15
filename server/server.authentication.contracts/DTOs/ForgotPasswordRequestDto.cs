using System.ComponentModel.DataAnnotations;

namespace server.authentication.contracts.DTOs
{
    public class ForgotPasswordRequestDto
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}
