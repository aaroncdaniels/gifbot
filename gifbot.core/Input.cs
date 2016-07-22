namespace gifbot.core
{
	public class Input
	{
		public Input(Function function = Function.Random, string phrase = "", int limit = 1, Size size = Size.FixedHeight)
		{
			Function = function;
			Size = size;
			Limit = limit;
			Phrase = phrase;
		}

		public Function Function { get; }

		public string Phrase { get; }

		public int Limit { get; }

		public Size Size { get; }
	}
}