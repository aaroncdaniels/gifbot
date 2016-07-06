namespace gifbot.Models
{
	public class RootObject
	{
		public string subscriptionId { get; set; }
		public int notificationId { get; set; }
		public string id { get; set; }
		public string eventType { get; set; }
		public string publisherId { get; set; }
		public Resource resource { get; set; }
		public string createdDate { get; set; }
	}

}