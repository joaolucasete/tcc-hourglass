namespace Hourglass.Site.Models.User {
	public class UserUpdateRequest {
		public string Name { get; set; }
		public string Address { get; set; }
		public Guid? PictureUploadId { get; set; }
	}
}
