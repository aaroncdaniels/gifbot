using System.Threading.Tasks;
using gifbot.Models;

namespace gifbot
{
	public interface IGifStore
	{
		Task<Gif> GetGifAsync(string subject = null);
	}
}