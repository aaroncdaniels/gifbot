using System.Collections.Generic;
using System.Threading.Tasks;
using gifbot.Core;

namespace gifbot.core
{
	public class Process : IProcess
	{
		private readonly IParser _parser;
		private readonly ITermFormatter _termFormatter;
		private readonly IGifStore _gifStore;

		public Process(IParser parser, ITermFormatter termFormatter, IGifStore gifStore)
		{
			_parser = parser;
			_termFormatter = termFormatter;
			_gifStore = gifStore;
		}

		public async Task<IEnumerable<string>> ProcessAsync(string input)
		{
			var parsedInput = _parser.ParseInput(input);
			var term = _termFormatter.Format(parsedInput.Terms);

			var gifs = new List<Gif>();

			switch (parsedInput.Function)
			{
				case Function.Translate:
					var translateResult = await _gifStore.TranslateGifAsync(term.UrlTerm);
					if (translateResult != null)
						gifs.Add(translateResult);
					break;
				case Function.Random:
					var randomResult = await _gifStore.RandomGifAsync(term.UrlTerm);
					if (randomResult != null)
						gifs.Add(randomResult);
					break;
				case Function.Search:
					gifs.AddRange(await _gifStore.SearchGifsAsync(term.UrlTerm, parsedInput.Limit));
					break;
				case Function.Trending:
					gifs.AddRange(await _gifStore.TrendingGifsAsync(parsedInput.Limit));
					break;
			}

			if (gifs.Count == 0)
				return new[] {$"{parsedInput.Function} [{term.ReadableTerm}] returned 0 results."};

			var messages = new List<string>();

			for (var i = 0; i < gifs.Count; i++)
			{
				var gif = gifs[i];

				string gifUrl;

				switch (parsedInput.Size)
				{
					case Size.FixedHeight:
						gifUrl = gif.FixedHeightUrl;
						break;
					case Size.FixedWidth:
						gifUrl = gif.FixedWidthUrl;
						break;
					default:
						gifUrl = gif.OriginalUrl;
						break;
				}

				var phrase = (parsedInput.Function != Function.Trending && !string.IsNullOrWhiteSpace(term.ReadableTerm))
					? $" [{term.ReadableTerm}]"
					: string.Empty;

				var countSuffix = (parsedInput.Function == Function.Search || parsedInput.Function == Function.Trending)
					? $" {i + 1} of {gifs.Count}"
					: string.Empty;

				messages.Add($"{gifUrl} id:[{ gif.Id}] {parsedInput.Function}{phrase}{countSuffix}");
			}

			return messages;
		}
	}
}