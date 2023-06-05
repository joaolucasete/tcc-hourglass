using Hourglass.Site.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hourglass.Site.Classes {
	public class Dtos {
		public class ReviewDto {
			public Guid UserId { get; set; }
			public Guid ConsumedServiceId { get; set; }
			public string Comment { get; set; } = string.Empty;

			[Range(1, 5)]
			[Required]
			public int Rating { get; set; }
		}

		public class ServiceDto {
			public Guid Id { get; set; }
			public string Name { get; set; }
			public string Description { get; set; }

			public Guid ServiceCategoryId { get; set; }

			[JsonIgnore]
			public DateTimeOffset CreatedAt { get; set; }

			[JsonIgnore]
			public DateTimeOffset UpdatedAt { get; set; }

		}

		public class ServiceCategoryDto {
			public Guid Id { get; set; }

			public string Name { get; set; }

			[JsonIgnore]
			public DateTimeOffset CreatedAt { get; set; }

			[JsonIgnore]
			public DateTimeOffset UpdatedAt { get; set; }
		}

		public class UserServiceDto {
			public Guid UserId { get; set; }

			public Guid ServiceId { get; set; }

			public float? Price { get; set; }
		}

		public class ConsumedServiceDto {
			public Guid UserId { get; set; }
			public Guid ServiceId { get; set; }
			public DateTime Date { get; set; }
			public TimeSpan StartTime { get; set; }
			public TimeSpan EndTime { get; set; }
			public string Observations { get; set; }

			public ConsumedServiceStatus Status { get; set; }
		}
	}
}
