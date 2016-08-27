using System.Threading.Tasks;
using gifbot.core.gifs;
using Microsoft.TeamFoundation.Chat.WebApi;

namespace gifbot.core.Tfs
{
	public class TfsProcess : ITfsProcess
	{
		private readonly ChatHttpClient _client;
		private readonly IConfiguration _configuration;

		public TfsProcess(
			ChatHttpClient client,
			IConfiguration configuration)
		{
			_configuration = configuration;
			_client = client;
		}

		public async Task<Entry> WriteToChatroomAsync(int roomid, string message)
		{
			var reply = await _client
				.SendMessageToRoomAsync(new MessageData {Content = $"{message}{_configuration.BotDescription}"}, roomid)
				.ConfigureAwait(false);

			return new Entry(roomid, reply.Id);
		}

		public Task DeleteMessageFromChatroomAsync(Entry entry)
		{
			return _client.DeleteMessageAsync(entry.RoomId, entry.MessageId);
		}
	}
}