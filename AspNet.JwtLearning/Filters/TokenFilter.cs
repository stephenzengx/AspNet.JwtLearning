using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility;
using AspNet.JwtLearning.Utility.BaseHelper;
using AspNet.JwtLearning.Utility.TokenHandle;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspNet.JwtLearning.Filters
{
    public class TokenFilter : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            List<string> allowOrigins = ConfigConst.AllowOrigins.Split(',').ToList();
            IEnumerable<string> headerOrigin = new List<string>();
            string origin = string.Empty;
            if (request.Headers.TryGetValues("Origin", out headerOrigin))
                origin = headerOrigin.FirstOrDefault();
           
            //预请求放行
            if (request.Method == HttpMethod.Options)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Headers.Add("Access-Control-Allow-Origin", origin);
                response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PATCH,DELETE,PUT,OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "*");
                response.Headers.Add("Access-Control-Allow-Credentials", "true");

                return response;
            }

            if (allowOrigins.Contains(origin))
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", origin);
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "*");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET,POST,PATCH,DELETE,PUT,OPTIONS");
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Credentials", "true");

            //登录注册-请求不用验证token 
            //request.RequestUri.AbsolutePath -> api/userinfo
            //request.RequestUri.AbsoluteUri ->  http://localhost:59655/api/userinfo
            if (request.RequestUri.AbsolutePath.Contains("login") || request.RequestUri.AbsolutePath.Contains("register"))
                return await base.SendAsync(request, cancellationToken);

            IEnumerable<string> authHeads = null;
            if (!request.Headers.TryGetValues(ConfigConst.AuthHeaderName, out authHeads))
                return ResponseFormat.GetResponse( 
                    ResultFactory.GetErrorResponse("token expire",-2,HttpStatusCode.Unauthorized)); 

            //开始验证token逻辑 
            string token = authHeads.FirstOrDefault();
            JwtContainerModel jwtContainerModel = JWTService.ValidateToken(token);
            if (jwtContainerModel == null)
                return ResponseFormat.GetResponse(
                    //401认证未通过 403forbidden 未授权  
                    ResultFactory.GetErrorResponse("token expire", -2, HttpStatusCode.Unauthorized));  
                    

            request.Properties.Add("userinfo", jwtContainerModel);

            return await base.SendAsync(request,cancellationToken); ;
        }
    }
}