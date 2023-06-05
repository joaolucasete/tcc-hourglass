namespace Hourglass.Site.Configuration;

public class EmailConfig {
	public bool Enabled { get; set; } = true;

	public string ConnectionString { get; set; } = string.Empty;

	public string SenderAddress { get; set; } = string.Empty;

	public string SenderName { get; set; } = string.Empty;
}
