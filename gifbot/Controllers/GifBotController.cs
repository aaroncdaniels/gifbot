using System;
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

	    public GifBotController(IConfiguration configuration, IGifStore gifStore)
	    {
		    _configuration = configuration;
		    _gifStore = gifStore;
	    }

	    [HttpPost, Route("gifbot")]
	    public async Task<HttpResponseMessage> Post(RootObject r)
		{
			var message = r.resource.content;
			if (!message.StartsWith(_configuration.BotName))
			{
				//Not us...just return.
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

		    var chatMessage = message.Substring(_configuration.BotName.Length).Trim();

			string subject = null;
		    string subjectMessage = null;
		    if (!string.IsNullOrWhiteSpace(chatMessage))
		    {
			    subject = chatMessage;
			    subjectMessage = $" tagged [{subject}]";
		    }

		    var gif = await _gifStore.GetGifAsync(subject);

			await WriteToChatroom(r.resource.postedRoomId,
				$"{gif.data?.image_original_url} - Powered By Giphy. - Retrieved by {_configuration.BotDescription} - Random gif{subjectMessage}");

			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		private async Task WriteToChatroom(int roomid, string message)
		{
			var tfsUri = new Uri(_configuration.TfsUri);
			var client = new ChatHttpClient(tfsUri,
				 new VssCredentials());
			await client.SendMessageToRoomAsync(new MessageData {Content = message}, roomid);
		}

		[HttpGet, Route("gifbot")]
		public Task<string> Get()
		{
			return Task.FromResult("Hello World!");
		}
	}
}