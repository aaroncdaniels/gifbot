using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace gifbot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

			var builder = new ContainerBuilder();
	        builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

			var config = GlobalConfiguration.Configuration;
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
	        builder.RegisterModule<DiModule>();

			var container = builder.Build();
			config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
		}
    }
}
