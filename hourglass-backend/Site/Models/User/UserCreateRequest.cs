using System.ComponentModel.DataAnnotations;

namespace Hourglass.Api.User;

public class UserRegisterRequest {
	[Required]
	[EmailAddress]
	public string Email { get; set; }

	public string Name { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	[Required]
	[Cpf]
	public string Cpf { get; set; }
}
