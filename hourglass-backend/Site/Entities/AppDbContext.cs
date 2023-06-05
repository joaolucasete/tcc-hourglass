using Bogus;
using Bogus.Extensions.Brazil;
using Hourglass.Api;
using Hourglass.Site.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hourglass.Site.Entities;

public class AppDbContext : IdentityDbContext<User, Role, Guid> {
	private readonly IConfiguration configuration;

	public AppDbContext(
		DbContextOptions options,
		IConfiguration configuration
	) : base(options) {
		this.configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
		optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		#region Custom indexes
		modelBuilder
			.Entity<ConsumedService>()
			.HasIndex(s => new { s.StartTime, s.EndTime });
		#endregion

		#region RelationShips

		modelBuilder.Entity<Review>()
			.HasOne(r => r.ConsumedService) // Specify the navigation property
			.WithOne(c => c.Review) // Specify the inverse navigation property
			.HasForeignKey<Review>(r => r.ConsumedServiceId); // Specify the foreign key property

		modelBuilder.Entity<ConsumedService>()
			.HasKey(c => c.Id); // Specify the primary key property

		// user has a list of services
		modelBuilder.Entity<User>()
			.HasMany(u => u.Services)
			.WithOne(s => s.User)
			.HasForeignKey(s => s.UserId);

		// user has a list of consumed services
		modelBuilder.Entity<User>()
			.HasMany(u => u.ConsumedServices)
			.WithOne(s => s.User)
			.HasForeignKey(s => s.UserId);

		// user has a list of reviews
		modelBuilder.Entity<User>()
			.HasMany(u => u.Reviews)
			.WithOne(r => r.User)
			.HasForeignKey(r => r.UserId);

		// service has a list of consumed services
		modelBuilder.Entity<Service>()
			.HasMany(s => s.ConsumedServices)
			.WithOne(s => s.Service)
			.HasForeignKey(s => s.ServiceId);

		#endregion
	}

	// Include the DbSet's below
	public DbSet<Review> Reviews { get; set; }
	public DbSet<Service> Services { get; set; }
	public DbSet<ConsumedService> ConsumedServices { get; set; }
	public DbSet<ServiceCategory> ServiceCategories { get; set; }
}
