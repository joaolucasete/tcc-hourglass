using Hourglass.Api.Review;
using Hourglass.Api.Service;
using Hourglass.Api.User;
using Hourglass.Site.Configuration;
using Hourglass.Site.Entities;
using Microsoft.Extensions.Options;

namespace Hourglass.Site.Services {
	public class ModelConverter {
		private readonly IOptions<AppGeneralConfig> generalConfig;

		public ModelConverter(
			IOptions<AppGeneralConfig> generalConfig
		) {
			this.generalConfig = generalConfig;
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
				UserPictureId = service.User.PictureUploadId,
				ContactLink = service.ContactLink,
			};

		public UserModel ToModel(User user)
			=> new() {
				Id = user.Id,
				Name = user.Name,
				Cpf = user.Cpf,
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
				PictureUploadId = user.PictureUploadId,
				Services = user.Services?.Select(ToSummary).ToList(),
			};
	}
}
