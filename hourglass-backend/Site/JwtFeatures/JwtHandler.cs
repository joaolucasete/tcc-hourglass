using Hourglass.Site.Configuration;
using Hourglass.Site.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hourglass.Site.JwtFeatures {
	public class JwtHandler {
		private readonly UserManager<User> userManager;
		private readonly IOptions<JwtSettings> jwtSettings;

		public JwtHandler(
			UserManager<User> userManager,
			IOptions<JwtSettings> jwtSettings
		) {
			this.userManager = userManager;
			this.jwtSettings = jwtSettings;
		}

		public SigningCredentials GetSigningCredentials() {
			var key = Encoding.UTF8.GetBytes(jwtSettings.Value.Secret);
			var secret = new SymmetricSecurityKey(key);

			return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
		}

		public async Task<List<Claim>> GetClaims(User user) {
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Email)
			};

			var roles = await userManager.GetRolesAsync(user);
			foreach (var role in roles) {
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			return claims;
		}

		public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
			var tokenOptions = new JwtSecurityToken(
				issuer: jwtSettings.Value.Issuer,
				audience: jwtSettings.Value.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.Value.TokenExpirationMinutes)),
				signingCredentials: signingCredentials);

			return tokenOptions;
		}
	}
}
