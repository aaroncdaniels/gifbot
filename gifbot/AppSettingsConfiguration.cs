using System.Configuration;

namespace gifbot
{
	public class AppSettingsConfiguration : IConfiguration
	{
		public string BotName => ConfigurationManager.AppSettings["botName"];
		public string BotDescription => ConfigurationManager.AppSettings["botDescription"];
		public string HelpMessage => ConfigurationManager.AppSettings["helpMessage"];
		public string ErrorMessage => ConfigurationManager.AppSettings["errorMessage"];
		public string TfsUri => ConfigurationManager.AppSettings["tfsUri"];
		public string GiphyUrl => ConfigurationManager.AppSettings["giphyUrl"];
		public string GiphyApiKey => ConfigurationManager.AppSettings["giphyApiKey"];
		public string GiphyRating => ConfigurationManager.AppSettings["giphyRating"];
		public string GiphySearchRoute => ConfigurationManager.AppSettings["giphySearchRoute"];
		public string GiphyRandomRoute => ConfigurationManager.AppSettings["giphyRandomRoute"];
		public string GiphyTranslateRoute => ConfigurationManager.AppSettings["giphyTranslateRoute"];
		public string GiphyTrendingRoute => ConfigurationManager.AppSettings["giphyTrendingRoute"];
	}
}