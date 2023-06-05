using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Site.Entities;

public class Review {
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Id { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid ConsumedServiceId { get; set; }
	public ConsumedService ConsumedService { get; set; }

	public string Comment { get; set; } = string.Empty;

	[Range(1, 5)]
	[Required]
	public int Rating { get; set; }
}
