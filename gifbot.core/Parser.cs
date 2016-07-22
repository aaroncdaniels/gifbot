using System;
using System.Collections.Generic;
using System.Linq;

namespace gifbot.core
{
    public class Parser : IParser
    {
	    private const StringComparison Oic = StringComparison.OrdinalIgnoreCase;

	    private const int DefaultLimit = 1;
	    private const int MaxLimit = 10;

	    private const string DefaultRequiredTerm = "[blank]";

		private const Size DefaultSize = Size.FixedHeight;
	    
		public Input ParseInput(string input)
	    {
			if (string.IsNullOrWhiteSpace(input))
				return new Input(Function.Random, Enumerable.Empty<string>(), DefaultLimit, DefaultSize);

			var function = Function.Translate;
			var limit = DefaultLimit;
			var size = DefaultSize;

		    var args = input.Trim().Split(' ');
		    var terms = new List<string>();
			var ignoreOperatorFound = false;
			var limitFound = false;
			var sizeFound = false;
			
		    for (var i = 0; i < args.Length; i++)
		    {
			    var arg = args[i];

			    if (string.IsNullOrWhiteSpace(arg))
				    continue;

			    if (i == 0 && Enum.TryParse(arg, true, out function))
				    continue;

				if (ignoreOperatorFound)
			    {
				    terms.Add(arg);
				    continue;
			    }

			    if ("--".Equals(arg, Oic) && i != args.Length - 1)
			    {
				    ignoreOperatorFound = true;
				    continue;
			    }

			    if (!limitFound && (function == Function.Search || function == Function.Trending))
			    {
				    if (int.TryParse(arg, out limit))
				    {
					    limitFound = true;
					    continue;
				    }
			    }

				if (!sizeFound && "-h".Equals(arg, Oic))
				{
					size = Size.FixedHeight;
					sizeFound = true;
					continue;
				}

				if (!sizeFound && "-w".Equals(arg, Oic))
				{
					size = Size.FixedWidth;
					sizeFound = true;
					continue;
				}

				if (!sizeFound && "-o".Equals(arg, Oic))
				{
					size = Size.Original;
					sizeFound = true;
					continue;
				}

				terms.Add(arg);
			}

			// :crystal_ball: translate and search require a phrase...
			if (terms.Count == 0 && (function == Function.Translate || function == Function.Search))
				terms.Add(DefaultRequiredTerm);

			if (limit < 1)
				limit = 1;

			if (limit > MaxLimit)
				limit = MaxLimit;
			
			return new Input(function, terms, limit, size);
	    }
    }
}