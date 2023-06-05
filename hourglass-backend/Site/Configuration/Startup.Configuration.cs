using Hourglass.Site.Entities;
using Microsoft.AspNetCore.Identity;
using VerifyEmailForgotPasswordTutorial.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigurationServiceCollectionExtensions {
	private const string General = "General";
	private const string JwtSettings = "JwtSettings";
	private const string Email = "Email";


	public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration configuration) {
		services.Configure<AppGeneralConfig>(configuration.GetSection(General));
		services.Configure<JwtSettings>(configuration.GetSection(JwtSettings));
		services.Configure<EmailConfig>(configuration.GetSection(Email));

		services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(10));

		return services;
	}
}
