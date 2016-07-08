using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using gifbot.Models;
using Microsoft.TeamFoundation.Chat.WebApi;
using Microsoft.VisualStudio.Services.Common;

namespace gifbot.Controllers
{
    public class GifBotController : ApiController
    {
	    private readonly IConfiguration _configuration;
	    private readonly IGifStore _gifStore;

	    private readonly string[] _supportedCommands = {"search", "translate", "random", "trending"};

	    public GifBotController(IConfiguration configuration, IGifStore gifStore)
	    {
		    _configuration = configuration;
		    _gifStore = gifStore;
	    }

		//Assuming app is named "gifbot"
	    [HttpPost, Route("retrieve")]
	    public async Task<HttpResponseMessage> Post(RootObject rootObject)
		{
			var args = rootObject?.resource?.content?.Split(' ');
			
			if (args == null || !args[0].Equals(_configuration.BotName, StringComparison.OrdinalIgnoreCase) || rootObject.resource == null)
			{
				//They're just talking about you gifbot, or something's wrong...just ignore them.
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

		    var roomId = rootObject.resource.postedRoomId;
		    
		    if (args.Length < 2 || !_supportedCommands.Any(sc => sc.Equals(args[1], StringComparison.OrdinalIgnoreCase)))
		    {
				await WriteToChatroom(roomId,
					$"{_configuration.BotDescription} - Powered By Giphy. \r\n{_configuration.HelpMessage}");
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

		    var chatMessage = args[1];

			string subject = null;
		    string subjectMessage = null;
		    if (!string.IsNullOrWhiteSpace(chatMessage))
		    {
			    subject = chatMessage;
			    subjectMessage = $" tagged [{subject}]";
		    }

		    try
		    {
				var gif = await _gifStore.GetGifAsync(subject);

				await WriteToChatroom(roomId,
					$"{gif.data?.image_original_url} - Powered By Giphy. - Retrieved by {_configuration.BotDescription} - Random gif{subjectMessage}");
			}
		    catch (Exception ex)
		    {
			    await WriteToChatroom(
					roomId, 
					_configuration.ErrorMessage + $"Exception message is [{ex.Message}].");
		    }
		   
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		private async Task WriteToChatroom(int roomid, string message)
		{
			var tfsUri = new Uri(_configuration.TfsUri);
			var client = new ChatHttpClient(tfsUri,
				 new VssCredentials());
			await client.SendMessageToRoomAsync(new MessageData {Content = message}, roomid);
		}

		[HttpGet, Route("ping")]
		public Task<string> Get()
		{
			return Task.FromResult(_configuration.BotName + " is alive!");
		}
	}
}