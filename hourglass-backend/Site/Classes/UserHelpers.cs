using System.Security.Claims;

namespace Hourglass.Site.Classes;

public static class UserHelpers {
	public static string GetName(this ClaimsPrincipal principal) {
		var userName = principal.FindFirst(c => c.Type == ClaimTypes.Name) ?? principal.FindFirst(c => c.Type == "sub");
		if (userName != null && !string.IsNullOrEmpty(userName.Value)) {
			return userName.Value;
		}

		return null;
	}
}
