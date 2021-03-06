﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace gifbot.core.Giphy
{
	public class TranslateResult
	{
		[JsonConverter(typeof(SingleValueArrayConverter<Data>))]
		public List<Data> data { get; set; }
		public Meta meta { get; set; }
	}
}