using System.Collections.Generic;
using System.Threading.Tasks;

namespace gifbot.Core
{
	public interface IGifStore
	{
		Task<IEnumerable<Gif>> SearchGifsAsync(string query, int limit = 1);

		Task<Gif> RandomGifAsync(string tag = null);

		Task<Gif> TranslateGifAsync(string phrase);

		Task<IEnumerable<Gif>> TrendingGifsAsync(int limit = 1);
	}
}