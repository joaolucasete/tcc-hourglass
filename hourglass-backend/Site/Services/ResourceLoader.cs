namespace VerifyEmailForgotPasswordTutorial.Services
{
    public class ResourceLoader
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<ResourceLoader> logger;

        public ResourceLoader(
            IWebHostEnvironment env,
            ILogger<ResourceLoader> logger
        )
        {
            this.env = env;
            this.logger = logger;
        }

        public string GetEmailTemplate(string fileName)
        {
            var path = Path.Combine(env.ContentRootPath, "Resources", "EmailTemplates", fileName);
            try
            {

                if (File.Exists(path))
                {
                    using var stream = File.OpenRead(path);
                    using var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
                else
                {
                    logger.LogWarning($"Template {fileName} not found in filesystem");
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, $"Error trying to fetch EmailTemplate {fileName} from fylesystem");
            }

            return null;
        }
    }
}
