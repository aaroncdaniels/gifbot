using System.Collections.Generic;

namespace gifbot.core.gifs
{
	public class Input
	{
		public Input(Function function, IEnumerable<string> terms, int limit, Size size)
		{
			Function = function;
			Terms = terms;
			Limit = limit;
			Size = size;
		}

		public Function Function { get; }

		public IEnumerable<string> Terms { get; }

		public int Limit { get; }

		public Size Size { get; }
	}
}