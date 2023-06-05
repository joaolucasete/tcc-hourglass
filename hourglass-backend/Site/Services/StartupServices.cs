using Hourglass.Site.JwtFeatures;
using Hourglass.Site.Repositories;
using VerifyEmailForgotPasswordTutorial.Services;

namespace Hourglass.Site.Services;

public static class StartupServices {
	public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration) {
		services.AddScoped<ServiceService>();
		services.AddScoped<UserRepository>();
		services.AddScoped<ModelConverter>();
		services.AddScoped<JwtHandler>();
		services.AddScoped<IMailSender, MailSender>();

		services.AddSingleton<ResourceLoader>();

		// Configure JWT authentication
		services.ConfigureJwtAuthentication(configuration);

		services.AddAuthorization();

		return services;
	}
}
