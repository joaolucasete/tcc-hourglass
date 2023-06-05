using Hourglass.Site.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Microsoft.Extensions.DependencyInjection;

public static class JwtBearerExtensions {
	public static IServiceCollection ConfigureJwtAuthentication(this IServiceCollection services, IConfiguration configuration) {
		// Get JWT configuration from appsettings.json
		var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

		// Configure JWT authentication
		services.AddAuthentication(options => {
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(o => {
			o.SaveToken = true;
			o.RequireHttpsMetadata = false;
			o.TokenValidationParameters = new TokenValidationParameters {
				ValidIssuer = jwtSettings.Issuer,
				ValidAudience = jwtSettings.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				RequireExpirationTime = true,
			};
			o.Events = new JwtBearerEvents() {
				OnChallenge = context => {
					context.HandleResponse();
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					context.Response.ContentType = "application/json";

					// Ensure we always have an error and error description.
					if (string.IsNullOrEmpty(context.Error))
						context.Error = "invalid_token";
					if (string.IsNullOrEmpty(context.ErrorDescription))
						context.ErrorDescription = "This request requires a valid JWT access token to be provided";

					// Add some extra context for expired tokens.
					if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException)) {
						var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
						context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
						context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
					}

					return context.Response.WriteAsync(JsonSerializer.Serialize(new {
						error = context.Error,
						error_description = context.ErrorDescription
					}));
				}
			};
		});

		return services;
	}
}
