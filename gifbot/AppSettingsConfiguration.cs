using System.Configuration;

namespace gifbot
{
	public class AppSettingsConfiguration : IConfiguration
	{
		public string BotName => ConfigurationManager.AppSettings["botName"];
		public string ErrorMessage => ConfigurationManager.AppSettings["errorMessage"];
		public string TfsUri => ConfigurationManager.AppSettings["tfsUri"];
		public string GiphyUrlWithApiToken => ConfigurationManager.AppSettings["giphyUrlWithApiToken"];
	}
}