using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hourglass.Api.Review {
	public class ReviewRequest {
		[Required]
		public Guid UserId { get; set; }

		[Required]
		public Guid ConsumedServiceId { get; set; }

		[Required]
		public string Comment { get; set; }

		[Range(1, 5)]
		[Required]
		public int Rating { get; set; }
	}
}
