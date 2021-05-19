using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility.BaseHelper;
using System;
using System.Web.Http.Filters;

namespace AspNet.JwtLearning.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 重写异常拦截方法 只能拦截到 进入到action之内的方法 比如消息处理器 MessageHandlers
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception e = actionExecutedContext.Exception;

            actionExecutedContext.Response = ResponseFormat.GetResponse(
                ResultFactory.GetErrorResponse(e.Message +(e.InnerException!=null ? ","+e.InnerException.Message:""),
                -1,
                System.Net.HttpStatusCode.InternalServerError)
            );
        }
    }
}