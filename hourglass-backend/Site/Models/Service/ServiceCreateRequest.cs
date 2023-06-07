using System.ComponentModel.DataAnnotations;

namespace Hourglass.Site.Models.Service {
	public class ServiceCreateRequest {
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Description is required")]
		public string Description { get; set; }

		[Required(ErrorMessage = "User is required")]
		public Guid UserId { get; set; }

		public Guid? ServiceCategoryId { get; set; }

		[Required(ErrorMessage = "Price is required")]
		public float Price { get; set; }

		[Required(ErrorMessage = "Contact link is required")]
		public string ContactLink { get; set; }
	}
}
