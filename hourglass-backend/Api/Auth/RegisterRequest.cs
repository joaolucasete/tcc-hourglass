using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api.Auth {
	public class RegistrationRequest {
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; } = string.Empty;

		[EmailAddress, Required(ErrorMessage = "Email is required")]
		public string Email { get; set; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		[MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$",
			ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit and one non-alphanumeric character. The password must be at least 8 characters long.")]
		[DataType(DataType.Password)]
		public string Password { get; set; } = string.Empty;

		[Compare("Password", ErrorMessage = "Confirmation password does not match")]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; } = string.Empty;

		[Required(ErrorMessage = "Cpf is required")]
		[Cpf]
		public string Cpf { get; set; } = string.Empty;
	}
}
