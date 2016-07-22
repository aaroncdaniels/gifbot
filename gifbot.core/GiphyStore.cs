using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using gifbot.core;
using gifbot.core.Giphy;
using Newtonsoft.Json;

namespace gifbot.Core
{
	public class GiphyStore : IGifStore
	{
		private readonly HttpClient _httpClient;
		private readonly IConfiguration _configuration;

		public GiphyStore(HttpClient httpClient, IConfiguration configuration)
		{
			_httpClient = httpClient;
			_configuration = configuration;
		}

		public async Task<Gif> TranslateGifAsync(string phrase)
		{
			var url = BuildBaseUrl(_configuration.GiphyTranslateRoute);

			if (!string.IsNullOrWhiteSpace(phrase))
				url += $"&s={phrase}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<TranslateResult>(body);

			if (result?.data != null && result.data.Count > 0)
				return new Gif
				{
					Id = result.data[0].id,
					OriginalUrl = result.data[0].images?.original?.url,
					FixedHeightUrl = result.data[0].images?.fixed_height_small?.url,
					FixedWidthUrl = result.data[0].images?.fixed_width_small?.url
				};

			return null;
		}

		public async Task<Gif> RandomGifAsync(string tag = null)
		{
			var url = BuildBaseUrl(_configuration.GiphyRandomRoute);

			if (!string.IsNullOrWhiteSpace(tag))
				url += $"&tag={tag}";
			
			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<RandomResult>(body);

			if (result?.data != null && result.data.Count > 0)
				return new Gif
				{
					Id = result.data[0].id,
					OriginalUrl = result.data[0].image_original_url,
					FixedHeightUrl = result.data[0].fixed_height_small_url,
					FixedWidthUrl = result.data[0].fixed_width_small_url
				};

			return null;
		}

		public async Task<IEnumerable<Gif>> SearchGifsAsync(string query, int limit = 1)
		{
			var url = BuildBaseUrl(_configuration.GiphySearchRoute);
			url += $"&q={query}&limit={limit}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<SearchResult>(body);

			if (result?.data == null)
				return Enumerable.Empty<Gif>();

			return result.data.Select(d => new Gif
			{
				Id = d.id,
				OriginalUrl = d.images?.original?.url,
				FixedHeightUrl = d.images?.fixed_height?.url,
				FixedWidthUrl = d.images?.fixed_width?.url
			});
		}

		public async Task<IEnumerable<Gif>> TrendingGifsAsync(int limit = 1)
		{
			var url = BuildBaseUrl(_configuration.GiphyTrendingRoute);
			url += $"&limit={limit}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<TrendingResult>(body);

			if (result?.data == null)
				return Enumerable.Empty<Gif>();

			return result.data.Select(d => new Gif
			{
				Id = d.id,
				OriginalUrl = d.images?.original?.url,
				FixedHeightUrl = d.images?.fixed_height?.url,
				FixedWidthUrl = d.images?.fixed_width?.url
			});
		}

		private string BuildBaseUrl(string route)
		{
			var url = $"{_configuration.GiphyUrl}{route}{_configuration.GiphyApiKey}";

			if (!string.IsNullOrWhiteSpace(_configuration.GiphyRating))
				url += $"&rating={_configuration.GiphyRating}";

			return url;
		}
	}
}