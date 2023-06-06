using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api.Auth;
public class ForgotPasswordRequest {
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	[Required]
	public string ClientURI { get; set; }
}
