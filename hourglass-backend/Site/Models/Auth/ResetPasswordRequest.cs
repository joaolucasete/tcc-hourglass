using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api.Auth;

public class ResetPasswordRequest {
	[Required(ErrorMessage = "Password is required")]
	[MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*]).{8,}$",
	   ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one digit and one non-alphanumeric character. The password must be at least 8 characters long.")]
	[DataType(DataType.Password)]
	public string NewPassword { get; set; } = string.Empty;

	[Compare("NewPassword")]
	public string ConfirmNewPassword { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;
	public string Token { get; set; } = string.Empty;
}
