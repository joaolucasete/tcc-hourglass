using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hourglass.Api.Review {
	public class ReviewSummary {
		public Guid UserId { get; set; }
		public Guid ConsumedServiceId { get; set; }
		public string Comment { get; set; }
		public int Rating { get; set; }
	}
}
