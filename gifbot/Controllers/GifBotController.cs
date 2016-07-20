using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using gifbot.Core;
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
			    await WriteHelpMessageToChatRoom(roomId);
			    return new HttpResponseMessage(HttpStatusCode.OK);
		    }

		    var command = args[1];

		    switch (command.ToLower())
		    {
				case "search":
				    await SearchGifs(args, roomId);
				    break;
				case "random":
				    await RandomGif(args, roomId);
				    break;
				case "translate":
					await TranslateGif(args, roomId);
					break;
				case "trending":
					await TrendingGifs(args, roomId);
					break;
				default:
				    await WriteToChatroom(roomId, "{command} not yet implemented.");
				    break;
		    }
		   
			return new HttpResponseMessage(HttpStatusCode.OK);
		}

	    private async Task SearchGifs(IReadOnlyList<string> args, int roomId)
	    {
			if (args.Count < 3 | args.Count > 4)
			{
				await WriteHelpMessageToChatRoom(roomId, "Invalid number of arguments for the search function.");
				return;
			}

		    var query = args[2];
		    var limit = GetLimit(args, 3);

		    try
			{
				var gifs = (await _gifStore.SearchGifsAsync(query, limit)).ToList();
				var count = gifs.Count;

				if (count < 1)
				{
					await WriteToChatroom(roomId, $"Searching [{query}] returned 0 results.");
					return;
				}

				for (var i = 0; i < count; i++)
					await WriteToChatroom(roomId, $"{gifs[i].FixedHeightUrl} id:[{gifs[i].Id}] - searching {FormatTerm(query)} result {i + 1} of {count}");
			}	
			catch (Exception ex)
			{
				await WriteExceptionMessageToChatRoom(roomId, ex);
			}
	    }

	    private async Task RandomGif(IReadOnlyList<string> args, int roomId)
		{
			if (args.Count > 3)
			{
				await WriteHelpMessageToChatRoom(roomId, "Invalid number of arguments for the random function.");
				return;
			}

			string tag = null;
			string tagLine;

		    if (args.Count == 3)
		    {
			    tag = args[2];
			    tagLine = $" - random gif tagged [{FormatTerm(tag)}]";
		    }
		    else
			    tagLine = "- random gif";

			try
			{
				var gif = await _gifStore.RandomGifAsync(tag);

				if (gif == null)
				{
					await WriteToChatroom(roomId, $"Random tagged [{tag}] returned 0 results.");
					return;
				}

				await WriteToChatroom(roomId, $"{gif.FixedHeightUrl} id:[{gif.Id}]{tagLine}");
			}
			catch (Exception ex)
			{
				await WriteExceptionMessageToChatRoom(roomId, ex);
			}
		}

	    private async Task TranslateGif(IReadOnlyList<string> args, int roomId)
		{
			if (args.Count != 3)
			{
				await WriteHelpMessageToChatRoom(roomId, "Invalid number of arguments for the translate function.");
				return;
			}

			var phrase = args[2];
			var phraseLine = $"translates the phrase [{FormatTerm(phrase)}].";

			try
			{
				var gif = await _gifStore.TranslateGifAsync(phrase);

				if (gif == null)
				{
					await WriteToChatroom(roomId, $"Translate [{phrase}] returned 0 results.");
					return;
				}

				await WriteToChatroom(roomId, $"{gif.FixedHeightUrl} id:[{gif.Id}] - {phraseLine}");
			}
			catch (Exception ex)
			{
				await WriteExceptionMessageToChatRoom(roomId, ex);
			}
		}

		private async Task TrendingGifs(IReadOnlyList<string> args, int roomId)
		{
			if (args.Count > 3)
			{
				await WriteHelpMessageToChatRoom(roomId, "Invalid number of arguments for the trending function.");
				return;
			}

			var limit = GetLimit(args, 2);

			try
			{
				var gifs = (await _gifStore.TrendingGifsAsync(limit)).ToList();
				var count = gifs.Count;

				if (count < 1)
				{
					await WriteToChatroom(roomId, "Trending returned 0 results.");
					return;
				}

				for (var i = 0; i < count; i++)
					await WriteToChatroom(roomId, $"{gifs[i].FixedHeightUrl} id:[{gifs[i].Id}] - trending result {i + 1} of {count}");
			}
			catch (Exception ex)
			{
				await WriteExceptionMessageToChatRoom(roomId, ex);
			}
		}

		[HttpGet, Route("ping")]
		public Task<string> Get()
		{
			return Task.FromResult($"{_configuration.BotDescription}is alive!");
		}

		private async Task WriteToChatroom(int roomid, string message)
		{
			var tfsUri = new Uri(_configuration.TfsUri);
			var client = new ChatHttpClient(tfsUri,
				 new VssCredentials());
			await client.SendMessageToRoomAsync(new MessageData {Content = $"{message}{_configuration.BotDescription}" }, roomid);
		}

		private async Task WriteHelpMessageToChatRoom(int roomId, string extraHelp = null)
		{
			await WriteToChatroom(roomId,
				$"{extraHelp} - {_configuration.HelpMessage}");
		}

		private async Task WriteExceptionMessageToChatRoom(int roomId, Exception ex)
		{
			await WriteToChatroom(
				roomId,
				$"{_configuration.ErrorMessage} - Exception message is [{ex.Message}].");
		}

		private static string FormatTerm(string term)
		{
			return term?.Replace('+', ' ').Replace("%20", " ");
		}

		private static int GetLimit(IReadOnlyList<string> args, int index)
		{
			int limit;
			if (args.Count != index + 1 || !int.TryParse(args[index], out limit))
				return 1;

			if (limit < 1)
				limit = 1;

			if (limit > 10)
				limit = 10;

			return limit;
		}
	}
}