using AspNet.JwtLearning.Utility.BaseHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.JwtLearning.Helpers
{
    public class ResponseFormat
    {
        public static JsonSerializerSettings serialSetting;

        //静态代码块
        static ResponseFormat()
        {
            serialSetting = GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings;
            serialSetting.DateTimeZoneHandling = DateTimeZoneHandling.Local;
            serialSetting.ContractResolver = new CamelCasePropertyNamesContractResolver();//驼峰式
            serialSetting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 返回403 response实例
        /// </summary>
        /// <returns></returns>
        public static HttpResponseMessage GetForbiddenResponseInstance()
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(("未授权"),
                System.Text.Encoding.GetEncoding("UTF-8"),"application/json"),
                StatusCode = HttpStatusCode.Forbidden //403
            };
        }

        /// <summary>
        /// 返回404 response实例
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetNotFoundResponseInstance(string text)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(("404 NoFonud:" +text),
                System.Text.Encoding.GetEncoding("UTF-8"), "application/json"),
                StatusCode = HttpStatusCode.NotFound //403
            };
        }

        public static HttpResponseMessage GetResponse(ResponseResult ret)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                JsonConvert.SerializeObject(new ResponseClass
                {
                    Status = ret.ResponseClass.Status,
                    Message = ret.ResponseClass.Message,
                    Record = ret.ResponseClass.Record,
                    TotalCount = ret.ResponseClass.TotalCount
                }, serialSetting),

                System.Text.Encoding.GetEncoding("UTF-8"), "application/json"),
                StatusCode = ret.HttpCode
                
            };
        }
    }
}