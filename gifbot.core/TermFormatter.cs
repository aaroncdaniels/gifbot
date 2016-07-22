using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gifbot.core
{
	public class TermFormatter : ITermFormatter
	{
		public Term Format(IEnumerable<string> terms)
		{
			var splitTerms = new List<string>();

			foreach (var term in terms)
				splitTerms.AddRange(term.Replace("%20", "+").Split('+'));

			var encodedTerms = splitTerms.Select(HttpUtility.UrlEncode);

			return new Term(string.Join("+", encodedTerms), string.Join(" ", splitTerms));
		}
	}
}