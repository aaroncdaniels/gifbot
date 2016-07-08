using System.Collections.Generic;
using System.Threading.Tasks;

namespace gifbot
{
	public interface IGifStore
	{
		Task<IEnumerable<string>> SearchGifsAsync(string query, int limit = 1);

		Task<string> RandomGifAsync(string tag = null);

		Task<string> TranslateGifAsync(string phrase);
	}
}