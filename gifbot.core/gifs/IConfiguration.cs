﻿namespace gifbot.core.gifs
{
	public interface IConfiguration
	{
		string BotName { get; }
		string BotDescription { get; }
		string HelpMessage { get; }
		string ErrorMessage { get; }
		string TfsUri { get; }
		string GiphyUrl { get; }
		string GiphyApiKey { get; }
		string GiphyRating { get; }
		string GiphySearchRoute { get; }
		string GiphyRandomRoute { get; }
		string GiphyTranslateRoute { get; }
		string GiphyTrendingRoute { get; }
		int QueueSize { get; }
	}
}