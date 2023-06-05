using Microsoft.AspNetCore.Mvc;

namespace Hourglass.Site.Controllers;

[Route("api/v1/home")]
[ApiController]
public class HomeController : ControllerBase {
	public HomeController() {
	}

	[HttpGet]
	public string Hello() {
		return $"Hello World";
	}
}
