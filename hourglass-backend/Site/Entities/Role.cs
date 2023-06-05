using Microsoft.AspNetCore.Identity;

namespace Hourglass.Site.Entities;

public class Role : IdentityRole<Guid> {
	public Role() : base() { }

	public Role(string roleName) : base(roleName) {
	}
}
