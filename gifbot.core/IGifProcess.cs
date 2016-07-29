using System.Collections.Generic;
using System.Threading.Tasks;

namespace gifbot.core
{
	public interface IGifProcess
	{
		Task<IEnumerable<string>> ProcessAsync(string input);
	}
}