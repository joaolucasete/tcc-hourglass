using AutoMapper;
using Hangfire;
using Hourglass.Api.Auth;
using Hourglass.Site.Entities;
using Hourglass.Site.JwtFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using VerifyEmailForgotPasswordTutorial.Services;

namespace Hourglass.Site.Controllers {
	[Route("api/v1/auth")]
	[ApiController]
	public class AuthController : ControllerBase {
		private readonly UserManager<User> userManager;
		private readonly RoleManager<Role> roleManager;
		private readonly IOptions<JwtSettings> jwtSettings;
		private readonly IMailSender mailSender;
		private readonly IMapper mapper;
		private readonly JwtHandler jwtHandler;

		public AuthController(
			UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IOptions<JwtSettings> jwtSettings,
			IMailSender mailSender,
			IMapper mapper,
			JwtHandler jwtHandler
		) {
			this.userManager = userManager;
			this.roleManager = roleManager;
			this.jwtSettings = jwtSettings;
			this.mailSender = mailSender;
			this.mapper = mapper;
			this.jwtHandler = jwtHandler;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginRequest request) {

			// checking the user
			var user = await userManager.FindByEmailAsync(request.Email);

			if (user == null || !await userManager.CheckPasswordAsync(user, request.Password)) {
				return Unauthorized(new LoginResponse { ErrorMessage = "Invalid Authentication" });
			}

			if (!await userManager.IsEmailConfirmedAsync(user)) {
				return Unauthorized(new LoginResponse { ErrorMessage = "Email not confirmed" });
			}

			var signingCredentials = jwtHandler.GetSigningCredentials();
			var claims = await jwtHandler.GetClaims(user);
			var tokenOptions = jwtHandler.GenerateTokenOptions(signingCredentials, claims);
			var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

			return Ok(new LoginResponse { IsAuthSuccessful = true, Token = token });
		}

		[HttpPost("registration")]
		public async Task<IActionResult> Registration(RegistrationRequest request) {
			var role = "User";

			// check user exist
			var userExist = await userManager.FindByEmailAsync(request.Email);
			if (userExist != null) {
				return StatusCode(StatusCodes.Status403Forbidden, "User already exists!");
			}
			var roleExist = await roleManager.FindByNameAsync(role);
			if (roleExist == null) {
				return StatusCode(StatusCodes.Status500InternalServerError, $"{role} role does not exist");
			}

			var user = mapper.Map<User>(request);
			var result = await userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded) {
				var errors = result.Errors.Select(e => e.Description);
				return BadRequest(new RegisterResponse { Errors = errors });
			}

			// Assign a role.
			await userManager.AddToRoleAsync(user, role);

			// Add token to verify email
			var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = Url.Action(nameof(EmailConfirmation), "Auth", new { token, email = user.Email }, Request.Scheme);

			BackgroundJob.Enqueue(() => mailSender.SendAccountVerifyEmail(user.Email, confirmationLink));

			return StatusCode(StatusCodes.Status201Created, $"User created & email sent to {user.Email} successfully");
		}

		[HttpGet("resend-email-confirmation")]
		public async Task<IActionResult> ResendEmailConfirmation(string email) {
			var user = await userManager.FindByEmailAsync(email);
			if (user == null) {
				return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
			}

			var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = Url.Action(nameof(EmailConfirmation), "Auth", new { token, email = user.Email }, Request.Scheme);

			BackgroundJob.Enqueue(() => mailSender.SendAccountVerifyEmail(user.Email, confirmationLink));

			return Ok("Email sent successfully");
		}

		[HttpGet("email-confirmation")]
		public async Task<IActionResult> EmailConfirmation(string token, string email) {
			var user = await userManager.FindByEmailAsync(email);
			if (user == null) {
				return StatusCode(StatusCodes.Status500InternalServerError, "User does not exist");
			}

			var result = await userManager.ConfirmEmailAsync(user, token);
			if (!result.Succeeded) {
				return Ok(result.ToString());
			}

			return StatusCode(StatusCodes.Status200OK, "Email verified successfully");
		}

		[HttpPost("forgot-password")]
		[AllowAnonymous]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request) {
			var user = await userManager.FindByEmailAsync(request.Email);
			if (user == null) {
				return BadRequest("Invalid Request");
			}

			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			var param = new Dictionary<string, string>
			{
				{"token", token },
				{"email", request.Email }
			};

			var forgotPasswordLink = QueryHelpers.AddQueryString(request.ClientURI, param);

			BackgroundJob.Enqueue(() => mailSender.SendForgotPasswordEmail(user.Email, forgotPasswordLink));

			return Ok($"Password changed request is sent on Email {user.Email}. Please open your email & click the link!");
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword(ResetPasswordRequest request) {

			var user = await userManager.FindByEmailAsync(request.Email);
			if (user == null) {
				return BadRequest("Invalid Request");
			}

			var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
			if (!result.Succeeded) {
				var errors = result.Errors.Select(e => e.Description);
				return BadRequest(new { Errors = errors });
			}

			return Ok($"Password has been changed");
		}
	}
}
