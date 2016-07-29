using System.Collections.Generic;
using System.Threading.Tasks;

namespace gifbot.core.gifs
{
	public interface IGifProcess
	{
		Task<IEnumerable<string>> ProcessAsync(string input);
	}
}