using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility.Common;
using AspNet.JwtLearning.Utility.Log;
using AspNet.JwtLearning.Utility.TokenHandle;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace AspNet.JwtLearning.Filters
{
    /// <inheritdoc />
    public class ActionFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Action过滤器
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            //webapi 获得 controller和actionname  
            string controllerName = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string actionName = context.ActionContext.ActionDescriptor.ActionName;
            //在传统的MVC中  获得控制器名和方法名
            //string controllerName = (string)filterContext.RouteData.Values["controller"];
            //string actionName = (string)filterContext.RouteData.Values["action"];

            foreach (var key in ConfigConst.ignoreApiRightCheckUrlKey)
            {
                if (controllerName.Contains(key) || actionName.Contains(key))
                {
                    base.OnActionExecuted(context);
                    return;
                }
            }

            var jsonModel = context.Request.Properties["userinfo"].ToString();
            var model = JsonConvert.DeserializeObject<JwtContainerModel>(jsonModel);
            if (model == null || model.UserId <= 0)
                throw new ArgumentException("userinfo format error!");

            var sysApis = RedisBLL.GetSysApiInfos();
            var selSysApi = sysApis.FirstOrDefault(m => m.controllerName == controllerName && m.actionName == actionName);
            if (selSysApi == null)
            {                
                context.Response = ResponseFormat.GetNotFoundResponseInstance("api isn't configured system apis ");
                base.OnActionExecuted(context);
                return;
            }

            var roleId = RedisBLL.GetUserRoles().FirstOrDefault(m => m.userId == model.UserId)?.roleId;
            if (roleId <= 0)
                throw new ArgumentException("userId not bind role");

            if (null == RedisBLL.GetRoleApiInfos().FirstOrDefault(m => m.roleId == roleId && m.apiId == selSysApi.apiId))
                context.Response = ResponseFormat.GetForbiddenResponseInstance();  
            
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <returns></returns>
        public string GetClientIp()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result))
            {
                return "0.0.0.0";
            }
            return result;
        }
    }
}