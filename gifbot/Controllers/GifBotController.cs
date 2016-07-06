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
			var bits = message.Split(' ');
			if (bits.Length != 1 || bits[0] != _configuration.BotName)
			{
				//Not us...just return.
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

		    string subject = null;
			
			if (bits.Length >= 2)
				subject = bits[1];

		    var gif = await _gifStore.GetGifAsync(subject);

			await WriteToChatroom(r.resource.postedRoomId,
				$"{_configuration.BotName} retrieved {gif.data?.image_original_url}. Powered By Giphy.");

			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		private async Task WriteToChatroom(int roomid, string message)
		{
			var tfsUri = new Uri(_configuration.TfsUri);
			var client = new ChatHttpClient(tfsUri,
				 new VssCredentials()); //use app pool credentials
			await client.SendMessageToRoomAsync(new MessageData {Content = message}, roomid);
		}

		[HttpGet, Route("gifbot")]
		public Task<string> Get()
		{
			return Task.FromResult("Hello World!");
		}
	}
}