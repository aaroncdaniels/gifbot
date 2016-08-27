using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gifbot.core.gifs
{
	public class GifProcess : IGifProcess
	{
		private readonly IParser _parser;
		private readonly ITermFormatter _termFormatter;
		private readonly IGifStore _gifStore;
		private readonly IConfiguration _configuration;
		private readonly IAuditor _auditor;

		public GifProcess(
			IParser parser, 
			ITermFormatter termFormatter, 
			IGifStore gifStore, 
			IConfiguration configuration, 
			IAuditor auditor)
		{
			_parser = parser;
			_termFormatter = termFormatter;
			_gifStore = gifStore;
			_configuration = configuration;
			_auditor = auditor;
		}

		public async Task<IEnumerable<string>> ProcessAsync(string input)
		{
			var parsedInput = _parser.ParseInput(input);

			if (parsedInput.Function == Function.Help)
				return new List<string> {_configuration.HelpMessage};

			if (parsedInput.Function == Function.Delete)
			{
				await _auditor.StrikeFromTheRecordAsync(parsedInput.Limit).ConfigureAwait(false);
				return Enumerable.Empty<string>();
			}

			var term = _termFormatter.Format(parsedInput.Terms);

			var gifs = await RetrieveGifsAsync(parsedInput, term).ConfigureAwait(false);

			if (gifs.Count == 0)
				return new[] {$"{parsedInput.Function} [{term.ReadableTerm}] returned 0 results."};

			var messages = new List<string>();

			for (var i = 0; i < gifs.Count; i++)
			{
				var gif = gifs[i];

				var gifUrl = GifUrlBySize(parsedInput, gif);

				var phrase = parsedInput.Function != Function.Trending && !string.IsNullOrWhiteSpace(term.ReadableTerm)
					? $" [{term.ReadableTerm}]"
					: string.Empty;

				var countSuffix = $" {i + 1} of {gifs.Count}";

				messages.Add($"{gifUrl} id:[{ gif.Id}] {parsedInput.Function}{phrase}{countSuffix}");
			}

			return messages;
		}

		private static async Task<IEnumerable<Gif>> InvokeStoreSpecifiedTimes(Func<string, Task<Gif>> storeFunc, string urlTerm, int times)
		{
			var gifs = new List<Gif>();

			for (var i = 0; i < times; i++)
			{
				var result = await storeFunc(urlTerm);
				if (result == null)
					break;

				gifs.Add(result);
			}

			return gifs;
		}

		private static string GifUrlBySize(Input parsedInput, Gif gif)
		{
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

			return gifUrl;
		}

		private async Task<List<Gif>> RetrieveGifsAsync(Input input, Term term)
		{
			var gifs = new List<Gif>();

			switch (input.Function)
			{
				case Function.Translate:
					gifs.AddRange(await TranslateGifsAsync(term.UrlTerm, input.Limit));
					break;
				case Function.Random:
					gifs.AddRange(await RandomGifsAsync(term.UrlTerm, input.Limit));
					break;
				case Function.Search:
					gifs.AddRange(await _gifStore.SearchGifsAsync(term.UrlTerm, input.Limit));
					break;
				case Function.Trending:
					gifs.AddRange(await _gifStore.TrendingGifsAsync(input.Limit));
					break;
			}

			return gifs;
		}

		private Task<IEnumerable<Gif>> TranslateGifsAsync(string urlTerm, int times)
		{
			return InvokeStoreSpecifiedTimes(_gifStore.TranslateGifAsync, urlTerm, times);
		}

		private Task<IEnumerable<Gif>> RandomGifsAsync(string urlTerm, int times)
		{
			return InvokeStoreSpecifiedTimes(_gifStore.RandomGifAsync, urlTerm, times);
		}
	}
}