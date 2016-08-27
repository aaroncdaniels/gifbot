using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using gifbot.core;
using gifbot.core.gifs;
using gifbot.core.Tfs;
using Microsoft.TeamFoundation.Chat.WebApi;
using Microsoft.VisualStudio.Services.Common;

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
				.InstancePerLifetimeScope();

			builder
				.RegisterType<Parser>()
				.As<IParser>()
				.InstancePerLifetimeScope();

			builder
				.RegisterType<GifProcess>()
				.As<IGifProcess>()
				.InstancePerLifetimeScope();

			builder
				.RegisterType<TermFormatter>()
				.As<ITermFormatter>()
				.InstancePerLifetimeScope();

			builder
				.RegisterType<TfsProcess>()
				.As<ITfsProcess>()
				.InstancePerLifetimeScope();

			builder
				.Register(context =>
				{
					var configuration = context.Resolve<IConfiguration>();
					var tfsUri = new Uri(configuration.TfsUri);
					return new ChatHttpClient(tfsUri,
						 new VssCredentials());
				})
				.SingleInstance();

			builder
				.Register(context =>
				{
					var httpClient = new HttpClient();
					httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					return httpClient;
				})
				.SingleInstance();

			builder
				.RegisterType<Auditor>()
				.As<IAuditor>()
				.SingleInstance();

			base.Load(builder);
		}
	}
}