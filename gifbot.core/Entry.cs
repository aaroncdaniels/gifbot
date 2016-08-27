namespace gifbot.core
{
	public struct Entry
	{
		public Entry(int roomId, int messageId)
		{
			RoomId = roomId;
			MessageId = messageId;
		}

		public int RoomId { get; }

		public int MessageId { get; }
	}
}