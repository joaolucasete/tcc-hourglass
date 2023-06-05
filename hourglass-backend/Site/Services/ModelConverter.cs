using Hourglass.Api.Review;
using Hourglass.Api.Service;
using Hourglass.Api.User;
using Hourglass.Site.Entities;

namespace Hourglass.Site.Services {
	public class ModelConverter {

		public ModelConverter() {

		}

		public ReviewModel ToModel(Review review)
			=> new() {
				Comment = review.Comment,
				ConsumedServiceId = review.ConsumedServiceId,
				Rating = review.Rating,
				UserId = review.UserId,
			};

		public ReviewSummary ToSummary(Review review)
			=> new() {
				UserId = review.UserId,
				ConsumedServiceId = review.ConsumedServiceId,
				Rating = review.Rating,
				Comment = review.Comment,
			};

		public ServiceSummary ToSummary(Service service)
			=> new() {
				Id = service.Id,
				CreatedAt = service.CreatedAt,
				Description = service.Description,
				Name = service.Name,
				Price = service.Price,
				UpdatedAt = service.UpdatedAt,
				ServiceCategory = service.ServiceCategory.Name,
				UserName = service.User.Name,
			};

		public UserModel ToModel(User user)
			=> new() {
				Id = user.Id,
				Name = user.Name,
				State = user.State,
				Country = user.Country,
				City = user.City,
				Email = user.Email,
				Complement = user.Complement,
				Neighborhood = user.Neighborhood,
				Number = user.Number,
				Phone = user.Phone,
				PostalCode = user.PostalCode,
				Street = user.Street,
				Services = user.Services?.Select(ToSummary).ToList(),
			};
	}
}
