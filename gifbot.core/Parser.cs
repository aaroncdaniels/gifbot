using System;

namespace gifbot.core
{
    public class Parser : IParser
    {
	    public Input ParseInput(string input)
	    {
			if (string.IsNullOrWhiteSpace(input))
				return new Input();

		    throw new NotImplementedException();
	    }
    }
}