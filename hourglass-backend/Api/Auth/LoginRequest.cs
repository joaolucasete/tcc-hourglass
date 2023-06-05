using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api.Auth {
	public class LoginRequest {

		[EmailAddress, Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
