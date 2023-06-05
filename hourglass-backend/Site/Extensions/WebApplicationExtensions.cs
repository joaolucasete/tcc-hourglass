using Bogus;
using Hourglass.Site.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Hourglass.Site.Extensions;

public static class WebApplicationExtensions
{

    public static async Task<WebApplication> InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.EnsureCreatedAsync();
        await context.Database.MigrateAsync();

        if (context.Roles.IsNullOrEmpty())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var roles = new List<string> { "Admin", "User", "Worker" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new Role(role));
            }
        }

        if (context.Users.IsNullOrEmpty())
        {

            var faker = new Faker("pt_BR");
            var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

            var adminUser = new User
            {
                Name = "User admin",
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Cpf = "22255735229",
                City = faker.Address.City(),
                Country = faker.Address.Country(),
                Number = faker.Address.BuildingNumber(),
                PostalCode = faker.Address.ZipCode(),
                State = faker.Address.State(),
                Street = faker.Address.StreetName(),
                Phone = faker.Phone.PhoneNumber(),
                EmailConfirmed = true,
            };

            await userManager.CreateAsync(adminUser, "Cachorr_0@");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            var dummyServiceCategory = new ServiceCategory
            {
                Id = Guid.NewGuid(),
                Name = "Programação",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            context.ServiceCategories.Add(dummyServiceCategory);

            var dummyService = new Service
            {
                Id = Guid.NewGuid(),
                Name = "Programador FullStack",
                Description = "Faço qualquer site da sua escolha",
                Price = float.Parse(faker.Commerce.Price()),
                User = adminUser,
                ServiceCategory = dummyServiceCategory
            };

            context.Services.Add(dummyService);
        }

        await context.SaveChangesAsync();
        return app;
    }
}
