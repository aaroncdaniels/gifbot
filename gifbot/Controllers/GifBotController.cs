using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using gifbot.core.gifs;
using gifbot.core.Tfs;
using gifbot.Models;

namespace gifbot.Controllers
{
    public class GifBotController : ApiController
    {
	    private readonly IConfiguration _configuration;
	    private readonly IGifProcess _gifProcess;
	    private readonly ITfsProcess _tfsProcess;

	    public GifBotController(
			IConfiguration configuration, 
			IGifProcess gifProcess, 
			ITfsProcess tfsProcess)
	    {
		    _configuration = configuration;
		    _gifProcess = gifProcess;
		    _tfsProcess = tfsProcess;
	    }

		//Assuming app is named "gifbot"
	    [HttpPost, Route("retrieve")]
	    public async Task<HttpResponseMessage> Post(RootObject rootObject)
		{
			var roomMessage = rootObject?.resource?.content;
			
			if (roomMessage == null || !roomMessage.StartsWith(_configuration.BotName, StringComparison.OrdinalIgnoreCase) || rootObject.resource == null)
			{
				//They're just talking about you gifbot, or something's wrong...just ignore them.
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

		    var roomId = rootObject.resource.postedRoomId;
		    var messageId = rootObject.resource.id;

		    var input = roomMessage.Substring(_configuration.BotName.Length + 1);

		    try
		    {
			    var outputs = await _gifProcess.ProcessAsync(input);

				foreach (var output in outputs)
					await _tfsProcess.WriteToChatroom(roomId, output);

			    await _tfsProcess.DeleteMessageFromChatroom(roomId, messageId);
		    }
		    catch (Exception ex)
		    {
			    await WriteExceptionMessageToChatRoom(roomId, ex);
		    }
			
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		[HttpGet, Route("ping")]
		public Task<string> Get()
		{
			return Task.FromResult($"{_configuration.BotDescription}is alive!");
		}

		private async Task WriteExceptionMessageToChatRoom(int roomId, Exception ex)
		{
			await _tfsProcess.WriteToChatroom(
				roomId,
				$"{_configuration.ErrorMessage} - Exception message is [{ex.Message}].");
		}
	}
}