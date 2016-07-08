using System.Collections.Generic;

namespace gifbot.Models.Giphy
{
	public class SearchResult
	{
		public List<SearchData> data { get; set; }
		public Meta meta { get; set; }
		public Pagination pagination { get; set; }
	}

	public class SearchData
	{
		public string type { get; set; }
		public string id { get; set; }
		public string slug { get; set; }
		public string url { get; set; }
		public string bitly_gif_url { get; set; }
		public string bitly_url { get; set; }
		public string embed_url { get; set; }
		public string username { get; set; }
		public string source { get; set; }
		public string rating { get; set; }
		public string caption { get; set; }
		public string content_url { get; set; }
		public string source_tld { get; set; }
		public string source_post_url { get; set; }
		public string import_datetime { get; set; }
		public string trending_datetime { get; set; }
		public SearchImages images { get; set; }
	}

	public class SearchImage
	{
		public string url { get; set; }
		public string width { get; set; }
		public string height { get; set; }
		public string size { get; set; }
		public string frames { get; set; }
		public string mp4 { get; set; }
		public string mp4_size { get; set; }
		public string webp { get; set; }
		public string webp_size { get; set; }
	}

	public class SearchImages
	{
		public SearchImage fixed_height { get; set; }
		public SearchImage fixed_height_still { get; set; }
		public SearchImage fixed_height_downsampled { get; set; }
		public SearchImage fixed_width { get; set; }
		public SearchImage fixed_width_still { get; set; }
		public SearchImage fixed_width_downsampled { get; set; }
		public SearchImage fixed_height_small { get; set; }
		public SearchImage fixed_height_small_still { get; set; }
		public SearchImage fixed_width_small { get; set; }
		public SearchImage fixed_width_small_still { get; set; }
		public SearchImage downsized { get; set; }
		public SearchImage downsized_still { get; set; }
		public SearchImage downsized_large { get; set; }
		public SearchImage original { get; set; }
		public SearchImage original_still { get; set; }
	}
}