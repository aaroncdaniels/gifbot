using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using gifbot.core;
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
	    private readonly IAuditor _auditor;

	    public GifBotController(
			IConfiguration configuration, 
			IGifProcess gifProcess, 
			ITfsProcess tfsProcess, 
			IAuditor auditor)
	    {
		    _configuration = configuration;
		    _gifProcess = gifProcess;
		    _tfsProcess = tfsProcess;
		    _auditor = auditor;
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

		    var queryEntry = new Entry(roomId, messageId);

			var input = roomMessage.Substring(_configuration.BotName.Length + 1);

		    try
		    {
			    var outputs = (await _gifProcess
					.ProcessAsync(input)
					.ConfigureAwait(false)).ToList();

			    if (outputs.Any())
				    _auditor.RecordActivity(queryEntry);
			    else
				    await _tfsProcess
						.DeleteMessageFromChatroomAsync(queryEntry)
						.ConfigureAwait(false);

				foreach (var output in outputs)
			    {
					var entry = await _tfsProcess
						.WriteToChatroomAsync(roomId, output)
						.ConfigureAwait(false);

					_auditor.RecordActivity(entry);
				}
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
			var entry = await _tfsProcess
				.WriteToChatroomAsync(
					roomId,
					$"{_configuration.ErrorMessage} - Exception message is [{ex.Message}].")
				.ConfigureAwait(false);

			_auditor.RecordActivity(entry);
		}
	}
}