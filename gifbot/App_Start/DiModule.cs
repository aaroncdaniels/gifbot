using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using gifbot.Core;

namespace gifbot
{
	public class DiModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder
				.RegisterType<AppSettingsConfiguration>()
				.As<IConfiguration>()
				.InstancePerRequest();

			builder
				.RegisterType<GiphyStore>()
				.As<IGifStore>()
				.InstancePerRequest();

			builder
				.Register(context =>
				{
					var httpClient = new HttpClient();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					return httpClient;
				})
				.SingleInstance();

			base.Load(builder);
		}
	}
}