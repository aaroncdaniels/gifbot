using System.Collections.Generic;

namespace gifbot.core
{
	public interface ITermFormatter
	{
		Term Format(IEnumerable<string> terms);
	}
}