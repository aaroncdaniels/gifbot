using System.Collections.Generic;
using Newtonsoft.Json;

namespace gifbot.Models.Giphy
{
	public class TrendingResult
	{
		[JsonConverter(typeof(SingleValueArrayConverter<Data>))]
		public List<Data> data { get; set; }
		public Meta meta { get; set; }
		public Pagination pagination { get; set; }
	}
}