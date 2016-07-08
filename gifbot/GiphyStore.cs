using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using gifbot.Models;
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

		public async Task<Gif> GetGifAsync(string subject = null)
		{
			var url = _configuration.GiphyUrlWithApiToken;

			if (!string.IsNullOrWhiteSpace(_configuration.Rating))
				url += $"&rating={_configuration.Rating}";

			if (!string.IsNullOrWhiteSpace(subject))
				url += "&tag=" + HttpUtility.UrlEncode(subject);
			
			var httpResponse = await _httpClient
				.GetAsync(url)
				.ConfigureAwait(false);

			httpResponse.EnsureSuccessStatusCode();

			var body = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
			return JsonConvert.DeserializeObject<Gif>(body);
		}
	}
}