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
			return NotFound();
		}

		if (!string.IsNullOrEmpty(request.Name)) {
			user.Name = request.Name;
		}
		if (!string.IsNullOrEmpty(request.Address)) {
			user.Street = request.Address;
		}
		user.UpdatedAt = DateTimeOffset.UtcNow;

		await appDbContext.SaveChangesAsync();
		return NoContent();
	}
}
