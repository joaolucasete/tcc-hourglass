using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Site.Entities;

public class Service {
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Id { get; set; }

	public string Name { get; set; }
	public string Description { get; set; }
	public string ContactLink { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid ServiceCategoryId { get; set; }
	public ServiceCategory ServiceCategory { get; set; }

	[Required]
	public float Price { get; set; }

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
