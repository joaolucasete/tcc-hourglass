using Azure;
using Azure.Communication.Email;
using Hourglass.Site.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace Hourglass.Site.Services {
	public enum EmailTypes {
		Verify,
		ForgotPassword
	}

	public interface IMailSender {
		void SendAccountVerifyEmail(string to, string confirmationLink);
		void SendForgotPasswordEmail(string to, string passwordResetLink);
	}

	public class MailSender : IMailSender {
		private readonly IOptions<EmailConfig> emailConfig;
		private readonly ResourceLoader resourceLoader;
		private readonly ILogger<MailSender> logger;
		private readonly Lazy<EmailClient> emailClient;
		private readonly ConcurrentDictionary<EmailTypes, string> templates = new();


		public MailSender(
			IOptions<EmailConfig> emailConfig,
			ResourceLoader resourceLoader,
			ILogger<MailSender> logger
		) {
			this.emailConfig = emailConfig;
			this.resourceLoader = resourceLoader;
			this.logger = logger;
			emailClient = new Lazy<EmailClient>(() => new EmailClient(emailConfig.Value.ConnectionString));
		}

		public void SendAccountVerifyEmail(string to, string confirmationLink)
			=> sendEmail(to, "Verificação de email", EmailTypes.Verify, new Dictionary<string, string> {
				["verificationLink"] = confirmationLink,
			});


		public void SendForgotPasswordEmail(string to, string passwordResetLink)
			=> sendEmail(to, "Recuperação de senha", EmailTypes.ForgotPassword, new Dictionary<string, string> {
				["verificationLink"] = passwordResetLink,
			});

		private void sendEmail(string to, string subject, EmailTypes type, Dictionary<string, string> values) {

			var emailContent = new EmailContent(subject) { Html = FormatTemplate(type, values) };
			var emailRecipients = new EmailRecipients(new List<EmailAddress> { new EmailAddress(to) });
			var emailMessage = new EmailMessage(emailConfig.Value.SenderAddress, emailRecipients, emailContent);

			try {
				logger.LogInformation("Sending email...");
				var emailSendOperation = emailClient.Value.Send(WaitUntil.Completed, emailMessage);

				logger.LogInformation($"Email Sent. Status = {emailSendOperation.Value.Status}");
				logger.LogInformation($"Email operation id = {emailSendOperation.Id}");
			} catch (RequestFailedException ex) {
				logger.LogWarning($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
			}
		}

		public string FormatTemplate(EmailTypes mailType, Dictionary<string, string> values) {
			var template = templates.GetOrAdd(mailType, loadTemplate);

			if (template == null) {
				return string.Empty;
			}

			var body = new StringBuilder(template);

			foreach (Match match in Regex.Matches(template, @"\{\{(.+?)\}\}")) {
				var argName = match.Groups[1].Value.Trim();
				if (!values.TryGetValue(argName, out var argValue)) {
					argValue = argName;
				}
				body.Replace(match.Value, argValue);
			}

			return body.ToString();
		}

		private string loadTemplate(EmailTypes mailType) => mailType switch {
			EmailTypes.Verify => resourceLoader.GetEmailTemplate("VerifyEmail.html"),
			EmailTypes.ForgotPassword => resourceLoader.GetEmailTemplate("VerifyEmail.html"),
			_ => throw new NotImplementedException()
		};

	}
}
