using System.Web.Http;
using AspNet.JwtLearning.Filters;

namespace AspNet.JwtLearning.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // 自定义消息处理器   
            config.MessageHandlers.Add(new TokenFilter());

            // Web API 属性路由
            config.MapHttpAttributeRoutes();

            //action拦截器
            config.Filters.Add(new ActionFilter());

            //全局异常处理
            config.Filters.Add(new GlobalExceptionFilter());

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
