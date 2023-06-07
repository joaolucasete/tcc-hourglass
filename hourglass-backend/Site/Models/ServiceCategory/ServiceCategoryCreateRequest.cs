using System.ComponentModel.DataAnnotations;

namespace Hourglass.Site.Models.ServiceCategory {
	public class ServiceCategoryCreateRequest {
		[Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }
	}
}
