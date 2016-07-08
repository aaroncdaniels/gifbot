namespace gifbot
{
	public interface IConfiguration
	{
		string BotName { get; }
		string BotDescription { get; }
		string ErrorMessage { get; }
		string TfsUri { get; }
		string GiphyUrlWithApiToken { get; }
		string Rating { get; }
	}
}