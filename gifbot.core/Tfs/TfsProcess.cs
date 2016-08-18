using System;
using System.Net;
using System.Threading.Tasks;
using gifbot.core.gifs;
using Microsoft.TeamFoundation.Chat.WebApi;
using Microsoft.VisualStudio.Services.Common;

namespace gifbot.core.Tfs
{
	public class TfsProcess : ITfsProcess
	{
		private readonly IConfiguration _configuration;
		private readonly ChatHttpClient _client;

		public TfsProcess(IConfiguration configuration)
		{
			_configuration = configuration;
			var tfsUri = new Uri(_configuration.TfsUri);
			_client = new ChatHttpClient(tfsUri,
				 new VssCredentials());
		}

		public async Task WriteToChatroom(int roomid, string message)
		{
			var reply = await _client.SendMessageToRoomAsync(new MessageData { Content = $"{message}{_configuration.BotDescription}" }, roomid);
			await DeleteMessageFromChatroom(roomid, reply.Id);
		}

		public async Task DeleteMessageFromChatroom(int roomId, int messageId)
		{
			await _client.DeleteMessageAsync(roomId, messageId);
		}
	}
}