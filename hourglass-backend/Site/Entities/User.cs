using Hourglass.Api;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Site.Entities;

public class User : IdentityUser<Guid> {

	[Required, Cpf]
	public string Cpf { get; set; }

	public string Name { get; set; }

	public string Phone { get; set; }
	public string Street { get; set; }
	public string Number { get; set; }
	public string Complement { get; set; }
	public string Neighborhood { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public string Country { get; set; }
	public string PostalCode { get; set; }

	public List<Service> Services { get; set; }

	public List<Review> Reviews { get; set; }

	public List<ConsumedService> ConsumedServices { get; set; }

	#region CreatedAt
	[NotMapped]
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

	public DateTime CreatedAtDate {
		get => CreatedAt.UtcDateTime;
		set => CreatedAt = new DateTimeOffset(value, TimeSpan.Zero);
	}
	#endregion

	#region UpdatedAt
	[NotMapped]
	public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
	public DateTime UpdatedAtDate {
		get => UpdatedAt.UtcDateTime;
		set => UpdatedAt = new DateTimeOffset(value, TimeSpan.Zero);
	}
	#endregion
}
