using System.Collections.Generic;
using Newtonsoft.Json;

namespace gifbot.core.Giphy
{
	public class RandomResult
	{
		[JsonConverter(typeof(SingleValueArrayConverter<RandomGif>))]
		public IList<RandomGif> data { get; set; }
		public Meta meta { get; set; }
	}
}