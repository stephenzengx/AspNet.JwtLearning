using AspNet.JwtLearning.Utility.BaseHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        public static HttpResponseMessage GetResponse(ResponseResult ret)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                JsonConvert.SerializeObject(new ResponseClass
                {
                    Status = 0,
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