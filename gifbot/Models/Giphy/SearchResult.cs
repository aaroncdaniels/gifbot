using System.Collections.Generic;

namespace gifbot.Models.Giphy
{
	public class SearchResult
	{
		public List<Data> data { get; set; }
		public Meta meta { get; set; }
		public Pagination pagination { get; set; }
	}
}