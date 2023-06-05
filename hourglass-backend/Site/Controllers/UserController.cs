using Hourglass.Api.User;
using Hourglass.Site.Classes;
using Hourglass.Site.Entities;
using Hourglass.Site.Repositories;
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

	public UserController(
		UserManager<User> userManager,
		ModelConverter mc
	) {
		this.userManager = userManager;
		this.mc = mc;
	}

	[Authorize]
	[HttpGet("me")]
	public async Task<UserModel> GetMyInfos() {
		var userName = User.GetName();
		var user = await userManager.Users.Include(u => u.Services).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync(u => u.UserName == userName);

		return mc.ToModel(user);
	}
}
