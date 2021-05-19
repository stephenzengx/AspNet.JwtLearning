using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static AspNet.JwtLearning.Utility.BaseHelper.ResultFactory;

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

        public static HttpResponseMessage GetResponse(ResultClass ret)
        {
            return new HttpResponseMessage
            {
                Content = new StringContent(
                JsonConvert.SerializeObject(new ResultClass
                {
                    State = 0,
                    Message = ret.Message,
                    Record = ret.Record
                }, serialSetting),

                System.Text.Encoding.GetEncoding("UTF-8"), "application/json"),
                StatusCode = ret.HttpCode
                
            };
        }

    }
}