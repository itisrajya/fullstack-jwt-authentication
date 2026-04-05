using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server.authentication.contracts.DTOs
{
	public class LoginUserDto
	{
		[Required(ErrorMessage = "The Email field is required.")]
		[EmailAddress(ErrorMessage = "Invalid email format.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "The Password field is required.")]
		public string Password { get; set; }
	}
}
