using AspNet.JwtLearning.App_Start;
using AspNet.JwtLearning.BLL;
using System.Web.Http;

namespace AspNet.JwtLearning
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //log4net configuretion
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));

            RedisBLL.InitGlobalInfoCache(); //init cache

            AutofacConfig.ConfigureContainer();//init ioc container

            GlobalConfiguration.Configure(WebApiConfig.Register);//init HttpConfiguration

        }
    }
}
