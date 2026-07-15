using System.ComponentModel.DataAnnotations;

namespace server.authentication.contracts.DTOs
{
    public class ResetPasswordRequestDto
    {
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Token field is required.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "The NewPassword field is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The ConfirmPassword field is required.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
