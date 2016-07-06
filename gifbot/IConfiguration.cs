using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gifbot
{
	public interface IConfiguration
	{
		string BotName { get; }
		string ErrorMessage { get; }
		string TfsUri { get; }
		string GiphyUrlWithApiToken { get; }
	}
}
