namespace gifbot.Models.Giphy
{
	public class Images
	{
		public Image fixed_height { get; set; }
		public Image fixed_height_still { get; set; }
		public Image fixed_height_downsampled { get; set; }
		public Image fixed_width { get; set; }
		public Image fixed_width_still { get; set; }
		public Image fixed_width_downsampled { get; set; }
		public Image fixed_height_small { get; set; }
		public Image fixed_height_small_still { get; set; }
		public Image fixed_width_small { get; set; }
		public Image fixed_width_small_still { get; set; }
		public Image downsized { get; set; }
		public Image downsized_still { get; set; }
		public Image downsized_large { get; set; }
		public Image original { get; set; }
		public Image original_still { get; set; }
	}
}