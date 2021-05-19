﻿using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace AspNet.JwtLearning.Filters
{
    /// <inheritdoc />
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            string actionName = context.ActionDescriptor.ActionName;
            string controllName = context.ActionDescriptor.ControllerDescriptor.ControllerName;

            return;
            base.OnActionExecuting(context);//to do...     
        }

        /// <summary>
        /// Action过滤器
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            //这里放 docker redis吧 
            //菜单权限，基础角色表，项目角色表(增加冗余 角色名称字段)
            //

            return;
            //var token = context.Response.Headers.GetValues("Authentication");//获取token

            //拦截请求 判断token
            //context.Response = new HttpResponseMessage {
            //    Content = new StringContent(
            //            ("未授权"),
            //            System.Text.Encoding.GetEncoding("UTF-8"),
            //            "application/json"),
            //    StatusCode = HttpStatusCode.Unauthorized
            //};

            //context.Response.Headers.Add("Content-Type", "application/json;charset=utf-8");
            //context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");

            //在传统的MVC中  获得控制器名和方法名
            //string controllerName = (string)filterContext.RouteData.Values["controller"];
            //string actionName = (string)filterContext.RouteData.Values["action"];
            //string ClientIp = GetClientIp();

            //webapi获得控制器名和方法名     
            //通过拿到controller和actionname可以进行精确到按钮级别的权限控制 关联用户角色或者具体的用户ID (后台弄一个配置表)      
            // 可将配置一次性放入iis缓存 或者 redis 更新配置更新数据库 以及(更新对应的缓存)
            //string controllerName  = context.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            //string actionName = context.ActionContext.ActionDescriptor.ActionName;
            //AbsolutePath: api/userinfo,  AbsoluteUri:http://localhost:59655/api/userinfo
            //string portName = context.Request.RequestUri.AbsolutePath;         

            //base.OnActionExecuted(context); // to do...
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