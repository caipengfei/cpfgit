using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Core;
using Autofac.Integration.WebApi;
using cpf.core;
using Senparc.Weixin.MP.TenPayLib;
using Senparc.Weixin.MP.TenPayLibV3;
namespace api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {

            var builder = RegisterService();
            var resolver = new AutofacWebApiDependencyResolver(builder.Build());
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            GlobalConfiguration.Configure(WebApiConfig.Register);

            GlobalConfiguration.Configuration.MessageHandlers.Add(new CorsHandler());
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
 

            System.IO.FileInfo fileinfo = new System.IO.FileInfo(Server.MapPath("~/log4net.config"));

            log4net.Config.XmlConfigurator.Configure(fileinfo);

            log.Info("api资源服务器启动......");
        }


        private ContainerBuilder RegisterService()
        {
            //
            var builder = new ContainerBuilder();

            var baseType = typeof(IDependency);
            var assemblys = BuildManager.GetReferencedAssemblies().Cast<Assembly>().ToList();//AppDomain.CurrentDomain.GetAssemblies().ToList();
            var AllServices = assemblys
                .SelectMany(s => s.GetTypes())
                .Where(p => baseType.IsAssignableFrom(p) && p != baseType);

            builder.RegisterApiControllers(assemblys.ToArray());

            builder.RegisterAssemblyTypes(assemblys.ToArray())
                   .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                   .AsImplementedInterfaces().InstancePerApiRequest();
            //.AsImplementedInterfaces().InstancePerLifetimeScope();
            return builder;
        }
    }
}
