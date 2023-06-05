using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hourglass.Api.Review {
	public class ReviewModel {
		public Guid UserId { get; set; }
		//public User User { get; set; }
		public Guid ConsumedServiceId { get; set; }
		//public ConsumedService ConsumedService { get; set; }
		public string Comment { get; set; }
		public int Rating { get; set; }
	}
}
