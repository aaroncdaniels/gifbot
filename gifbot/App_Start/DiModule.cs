using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using gifbot.core;
using gifbot.core.gifs;
using gifbot.core.Tfs;

namespace gifbot
{
	public class DiModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder
				.RegisterType<AppSettingsConfiguration>()
				.As<IConfiguration>()
				.SingleInstance();

			builder
				.RegisterType<GiphyStore>()
				.As<IGifStore>()
				.InstancePerRequest();

			builder
				.RegisterType<Parser>()
				.As<IParser>()
				.InstancePerRequest();

			builder
				.RegisterType<GifProcess>()
				.As<IGifProcess>()
				.InstancePerRequest();

			builder
				.RegisterType<TermFormatter>()
				.As<ITermFormatter>()
				.InstancePerRequest();

			builder
				.RegisterType<TfsProcess>()
				.As<ITfsProcess>()
				.SingleInstance();

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