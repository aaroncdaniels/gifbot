using System.Configuration;

namespace gifbot
{
	public class AppSettingsConfiguration : IConfiguration
	{
		public string BotName => ConfigurationManager.AppSettings["botName"];
		public string BotDescription => ConfigurationManager.AppSettings["botDescription"];
		public string ErrorMessage => ConfigurationManager.AppSettings["errorMessage"];
		public string TfsUri => ConfigurationManager.AppSettings["tfsUri"];
		public string GiphyUrlWithApiToken => ConfigurationManager.AppSettings["giphyUrlWithApiToken"];
		public string Rating => ConfigurationManager.AppSettings["rating"];
		public string HelpMessage => ConfigurationManager.AppSettings["helpMessage"];
	}
}