using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using gifbot.Models.Giphy;
using Newtonsoft.Json;

namespace gifbot
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

		public async Task<IEnumerable<string>> SearchGifsAsync(string query, int limit = 1)
		{
			var url = BuildBaseUrl(_configuration.GiphySearchRoute);
			url += $"&q={query}&limit={limit}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<SearchResult>(body);

			return result.data.Select(d => d.images).Select(i => i.original.url);
		}

		public async Task<string> RandomGifAsync(string tag = null)
		{
			var url = BuildBaseUrl(_configuration.GiphyRandomRoute);

			if (!string.IsNullOrWhiteSpace(tag))
				url += $"&tag={tag}";
			
			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var randomResult = JsonConvert.DeserializeObject<RandomResult>(body);

			if (randomResult?.data != null && randomResult.data.Count > 0)
				return randomResult.data[0]?.image_original_url;

			return null;
		}

		public async Task<string> TranslateGifAsync(string phrase)
		{
			var url = BuildBaseUrl(_configuration.GiphyTranslateRoute);

			if (!string.IsNullOrWhiteSpace(phrase))
				url += $"&s={phrase}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var randomResult = JsonConvert.DeserializeObject<TranslateResult>(body);

			if (randomResult?.data != null && randomResult.data.Count > 0)
				return randomResult.data?[0]?.images?.original?.url;

			return null;
		}

		public async Task<IEnumerable<string>> TrendingGifsAsync(int limit = 1)
		{
			var url = BuildBaseUrl(_configuration.GiphyTrendingRoute);
			url += $"&limit={limit}";

			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			var result = JsonConvert.DeserializeObject<TrendingResult>(body);

			return result.data.Select(d => d.images).Select(i => i.original.url);
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