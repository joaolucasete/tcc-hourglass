
using Hourglass.Site.Entities;
using Microsoft.EntityFrameworkCore;

namespace Hourglass.Site.Repositories;

public class UserRepository {
	private readonly AppDbContext context;
	public UserRepository(AppDbContext context) {
		this.context = context;
	}

	public async Task<User> GetUserByIdAsync(Guid id) {
		return await context.Users.Include(u => u.Services).ThenInclude(s => s.ServiceCategory).FirstOrDefaultAsync(u => u.Id == id);
	}

	public async Task<User> GetUserByEmailAsync(string email) {
		return await context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));
	}
}
