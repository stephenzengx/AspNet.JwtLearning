using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility.BaseHelper;
using AspNet.JwtLearning.Utility.Common;
using AspNet.JwtLearning.Utility.TokenHandle;

namespace AspNet.JwtLearning.Filters
{
    /// <summary>
    /// token 拦截器
    /// </summary>
    public class TokenFilter : DelegatingHandler
    {
        protected List<string> ignoreTokenUrlKey = new List<string> {"System", "Admin" };

        /// <summary>
        /// 重写方法, 拦截请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var absPath = request.RequestUri.AbsolutePath;
            if (absPath.Contains("swagger"))//swagger 放行
                return await base.SendAsync(request, cancellationToken);

            IEnumerable<string> headerOriginList;
            request.Headers.TryGetValues("Origin", out headerOriginList);
            //预请求放行
            if (request.Method == HttpMethod.Options)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", headerOriginList.FirstOrDefault());
                response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PATCH,DELETE,PUT,OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "*");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");

                return response;
            }

            List<string> allowOrigins = ConfigConst.AllowOrigins.Split(',').ToList();
            if (headerOriginList!=null && allowOrigins.Contains(headerOriginList.FirstOrDefault()))
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", headerOriginList.FirstOrDefault());
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "*");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,PATCH,DELETE,PUT,OPTIONS");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            //(system标头,业务系统)登录,注册,Admin(后台管理系统)- 请求不用验证token 
            //request.RequestUri.AbsolutePath -> api/userinfo
            //request.RequestUri.AbsoluteUri ->  http://localhost:59655/api/userinfo
            foreach (var key in ignoreTokenUrlKey)
            {
                if (absPath.Contains(key))
                    return await base.SendAsync(request, cancellationToken);
            }

            IEnumerable<string> authHeads = null;
            if (!request.Headers.TryGetValues(ConfigConst.AuthHeaderName, out authHeads))
                return ResponseFormat.GetResponse( 
                    ResultHelper.GetErrorResponse("token expire",-2,HttpStatusCode.Unauthorized)); 

            //开始验证token逻辑 
            string token = authHeads.FirstOrDefault();
            try
            {
                JwtContainerModel jwtContainerModel = JWTService.ValidateToken(token);
                if (jwtContainerModel == null)
                    return ResponseFormat.GetResponse(
                        //401认证未通过 403forbidden 未授权  
                        ResultHelper.GetErrorResponse("token expire", -2, HttpStatusCode.Unauthorized));

                request.Properties.Add("userinfo", jwtContainerModel);
            }
            catch (System.Exception e)
            {
                    return ResponseFormat.GetResponse(
                        //401认证未通过 403forbidden 未授权  
                        ResultHelper.GetErrorResponse("token expire", -2, HttpStatusCode.Unauthorized));
            }                   

            return await base.SendAsync(request,cancellationToken); ;
        }
    }
}