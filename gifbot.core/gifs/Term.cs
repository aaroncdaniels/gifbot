namespace gifbot.core.gifs
{
	public class Term
	{
		public Term(string urlTerm, string readableTerm)
		{
			UrlTerm = urlTerm;
			ReadableTerm = readableTerm;
		}

		public string UrlTerm { get; }
		public string ReadableTerm { get; }
	}
}