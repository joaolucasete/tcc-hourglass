using System.Reflection;

namespace Hourglass.Site.Services {
	public class ResourceLoader {
		private readonly IWebHostEnvironment env;
		private readonly ILogger<ResourceLoader> logger;

		public ResourceLoader(
			IWebHostEnvironment env,
			ILogger<ResourceLoader> logger
		) {
			this.env = env;
			this.logger = logger;
		}

		public string GetEmailTemplate(string fileName) {
#if DEBUG
			var path = Path.Combine(env.ContentRootPath, "Resources", "EmailTemplates", fileName);
			Console.WriteLine(path);
			try {

				if (File.Exists(path)) {
					using var stream = File.OpenRead(path);
					using var reader = new StreamReader(stream);
				} else {
					logger.LogWarning($"Template {fileName} not found in filesystem");
				}
			} catch (Exception ex) {
				logger.LogWarning(ex, $"Error trying to fetch EmailTemplate {fileName} from fylesystem");
			}
#else
			var assembly = Assembly.GetEntryAssembly();
			var resourceFullName = $"{assembly.GetName().Name}.Resources.EmailTemplates.{fileName}";
			var stream = assembly.GetManifestResourceStream(resourceFullName);
			if (stream == null) {
				logger.LogWarning($"Could not load embedded file \"{fileName}\" (resourceFullName: {resourceFullName}, assembly: {assembly.FullName})");
				return null;
			}
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
#endif
			return null;
		}
	}
}
