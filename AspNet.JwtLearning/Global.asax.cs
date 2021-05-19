using AspNet.JwtLearning.App_Start;
using System.Web.Http;

namespace AspNet.JwtLearning
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.ConfigureContainer();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
