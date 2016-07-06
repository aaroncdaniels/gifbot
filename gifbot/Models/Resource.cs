namespace gifbot.Models
{
	public class Resource
	{
		public int id { get; set; }
		public string content { get; set; }
		public string messageType { get; set; }
		public string postedTime { get; set; }
		public int postedRoomId { get; set; }
		public PostedBy postedBy { get; set; }

	}
}