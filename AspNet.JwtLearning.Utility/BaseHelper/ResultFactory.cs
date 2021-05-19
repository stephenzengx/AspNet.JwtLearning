using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http;
using System.Web;
//using System.Web.Http;

namespace AspNet.JwtLearning.Utility.BaseHelper
{
    public class ResultFactory
    {
        public class ResultClass
        {
            public HttpStatusCode HttpCode { get; set; } //http状态码
            public int State { get; set; } //业务码
            public string Message { get; set; }
            public object Record { get; set; }
        }

        #region tips
        public static readonly string SUCCESS_ADD = "新增成功";
        public static readonly string SUCCESS_UPDATE = "更新成功";
        public static readonly string SUCCESS_DELETE = "删除成功";
        public static readonly string SUCCESS_BIND = "绑定成功";
        public static readonly string SUCCESS_UNBIND = "解绑成功";
        public static readonly string ERROR_PARAM = "参数错误,请检查";
        public static readonly string ERROR_DATANOTFOUND = "数据不存在,刷新页面重试";
        public static readonly string ERROR_DUPLICATED_BIND = "重复绑定,请检查";
        public static readonly string EXCEPTION_REQUEST = "请求异常,请检查";
        #endregion

        public static ResultClass OkResponse(object obj, string message = "操作成功")
        {
            return new ResultClass
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = message,
                Record = obj
            };
        }
        public static ResultClass SuccessAddResponse(string id)
        {
            return new ResultClass
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = SUCCESS_ADD,
                Record = id
            };
        }
        public static ResultClass SuccessUpdateResponse()
        {
            return new ResultClass
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = SUCCESS_UPDATE
            };
        }
        public static ResultClass SuccessDeleteResponse()
        {
            return OkResponse(SUCCESS_DELETE);
        }

        public static ResultClass ErrorParamResponse(string apppend = "")
        {
            return GetErrorResponse(ERROR_PARAM + apppend);
        }

        public static ResultClass ErrorDataNotFoundResponse()
        {
            return GetErrorResponse(ERROR_DATANOTFOUND);
        }

        public static ResultClass GetErrorResponse(string errMessage = "系统异常", int state = -1, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ResultClass
            {
                HttpCode = statusCode,
                State = -1,
                Message = errMessage
            };
        }
    }
}
