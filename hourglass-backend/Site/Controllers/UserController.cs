using Azure.Core;
using Hourglass.Api.User;
using Hourglass.Site.Classes;
using Hourglass.Site.Entities;
using Hourglass.Site.Models.User;
using Hourglass.Site.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hourglass.Site.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase {
	private readonly UserManager<User> userManager;
	private readonly ModelConverter mc;
	private readonly AppDbContext appDbContext;

	public UserController(
		UserManager<User> userManager,
		ModelConverter mc,
		AppDbContext appDbContext
	) {
		this.userManager = userManager;
		this.mc = mc;
		this.appDbContext = appDbContext;
	}

	[Authorize]
	[HttpGet("me")]
	public async Task<UserModel> GetMyInfos() {
		var userName = User.GetName();
		var user = await userManager.Users.Include(u => u.Services).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync(u => u.UserName == userName);

		return mc.ToModel(user);
	}

	[Authorize]
	[HttpPut("me")]
	public async Task<IActionResult> UpdateMyInfos(UserUpdateRequest request) {
		var userName = User.GetName();
		var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
		if (user == null) {
			return Unauthorized();
		}

		if (!string.IsNullOrEmpty(request.Name)) {
			user.Name = request.Name;
		}
		if (!string.IsNullOrEmpty(request.Address)) {
			user.Street = request.Address;
		}
		if (request.PictureUploadId != null) {
			user.PictureUploadId = request.PictureUploadId;
		}

		user.UpdatedAt = DateTimeOffset.UtcNow;

		await appDbContext.SaveChangesAsync();
		return NoContent();
	}

	//[Authorize]
	//[DisableFormValueModelBinding]
	//[HttpPost("me/picture")]
	//public async Task<IActionResult> UploadProfilePicture(IFormFile fileTest) {
	//	var a = fileTest;
	//	var userName = User.GetName();
	//	var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
	//	if (user == null) {
	//		return Unauthorized();
	//	}
	//	var picture = Request.Form.Files.FirstOrDefault();

	//	if (picture?.Length > 0) {
	//		using var memoryStream = new MemoryStream();
	//		await picture.CopyToAsync(memoryStream);
	//		if (memoryStream.Length < 2_097_152) {
	//			user.PictureSource = memoryStream.ToArray();
	//		} else {
	//			return BadRequest("Picture file is too large");
	//		}
	//	}

	//	await appDbContext.SaveChangesAsync();

	//	return Ok();
	//}

	//[Authorize]
	//[HttpGet("me/picture")]
	//public async Task<IActionResult> GetMyProfilePicture() {
	//	var userName = User.GetName();
	//	var user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == userName);
	//	if (user == null) {
	//		return Unauthorized();
	//	}
	//	if (user.PictureSource == null) {
	//		return null;
	//	}

	//	return File(user.PictureSource, "image/png");
	//}
}
