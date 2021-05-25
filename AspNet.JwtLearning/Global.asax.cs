using AspNet.JwtLearning.App_Start;
using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Utility.Redis;
using System.Web.Http;

namespace AspNet.JwtLearning
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected static string keyTenants = "tenants";
        protected static string roleInfos = "roleInfos";
        protected static string userRoles = "userRoles";

        protected static string menus = "menus";
        protected static string roleMenus = "roleMenus";

        protected static string buttons = "buttons";//menu_button
        protected static string roleButtons = "roleButtons";

        protected static string apiInfos = "apiInfos";
        protected static string roleApiInfos = "roleApiInfos";

        protected void Application_Start()
        {
            Init_GlobalInfoCache();//初始化缓存数据

            AutofacConfig.ConfigureContainer();

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public static void Init_GlobalInfoCache()
        {
            //加载数据数据进入redis to do...
            using (AuthDbContext db = new AuthDbContext())
            {
                //RedisHelper.
            }
        }
    }
}
