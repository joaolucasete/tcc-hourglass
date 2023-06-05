using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hourglass.Site.Entities;


public enum ConsumedServiceStatus {
	Pending = 1,
	Confirmed,
	Canceled,
	Completed
}

public class ConsumedService {
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Id { get; set; }

	public Guid UserId { get; set; }
	public User User { get; set; }

	public Guid ServiceId { get; set; }
	public Service Service { get; set; }

	public DateTime Date { get; set; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public string Observations { get; set; }
	public ConsumedServiceStatus Status { get; set; }

	public Guid ReviewId { get; set; }
	public Review Review { get; set; }
}
