using System.Collections.Generic;

namespace gifbot.core.gifs
{
	public interface ITermFormatter
	{
		Term Format(IEnumerable<string> terms);
	}
}