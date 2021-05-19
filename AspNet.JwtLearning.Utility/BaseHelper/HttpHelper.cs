using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AspNet.JwtLearning.Utility
{
    public class HttpHelper
    {
        /// <summary>
        /// 基础调用方法 Get 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                //获取内容
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// post方法， ContentType: x-www-form-urlencoded 请求头这里要修改 to do
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string Post(string url, string postData)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.Timeout = 15000;

            byte[] data = Encoding.UTF8.GetBytes(postData);//把字符串转换为字节
            //req.ContentLength = data.Length; //请求长度 无需手动指定长度

            using (Stream reqStream = req.GetRequestStream()) //获取
            {
                reqStream.Write(data, 0, data.Length);//向当前流中写入字节               
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse(); //响应结果
            if (resp == null)
                return result;

            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }

            return result;
        }
    }
}