namespace gifbot.Models
{
	public class Gif
	{
		public GifData data { get; set; }
		public Meta meta { get; set; }
	}

	public class GifData
	{
		public string type { get; set; }
		public string id { get; set; }
		public string url { get; set; }
		public string image_original_url { get; set; }
		public string image_url { get; set; }
		public string image_mp4_url { get; set; }
		public string img_frames { get; set; }
		public string img_width { get; set; }
		public string img_height { get; set; }
		public string fixed_height_downsampled_url { get; set; }
		public string fixed_height_downsampled_width { get; set; }
		public string fixed_height_downsampled_height { get; set; }
		public string fixed_width_downsampled_url { get; set; }
		public string fixed_width_downsampled_width { get; set; }
		public string fixed_width_downsampled_height { get; set; }
		public string fixed_height_small_url { get; set; }
		public string fixed_height_small_still_url { get; set; }
		public string fixed_height_small_width { get; set; }
		public string fixed_height_small_height { get; set; }
		public string fixed_width_small_url { get; set; }
		public string fixed_width_small_still_url { get; set; }
		public string fixed_width_small_width { get; set; }
		public string fixed_width_small_height { get; set; }
		public string username { get; set; }
		public string caption { get; set; }
	}

	public class Meta
	{
		public short status { get; set; }
		public string msg { get; set; }
	}
}