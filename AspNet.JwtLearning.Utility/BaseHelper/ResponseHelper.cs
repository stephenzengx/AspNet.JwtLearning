using System.Net;

namespace AspNet.JwtLearning.Utility.BaseHelper
{
    public class ResponseResult
    {
        public HttpStatusCode HttpCode { get; set; } //http状态码
        public int State { get; set; } //业务码
        public string Message { get; set; }
        public object Record { get; set; }
    }

    public static class ResponseHelper
    {
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

        public static ResponseResult OkResponse(object obj = null,string message = "操作成功")
        {
            return new ResponseResult
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = message,
                Record = obj
            };
        }
        public static ResponseResult SuccessAddResponse(string id)
        {
            return new ResponseResult
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = SUCCESS_ADD,
                Record = id
            };
        }
        public static ResponseResult SuccessUpdateResponse()
        {
            return new ResponseResult
            {
                HttpCode = HttpStatusCode.OK,
                State = 0,
                Message = SUCCESS_UPDATE
            };
        }
        public static ResponseResult SuccessDeleteResponse()
        {
            return OkResponse(SUCCESS_DELETE);
        }

        public static ResponseResult ErrorParamResponse(string apppend = "")
        {
            return GetErrorResponse(ERROR_PARAM + apppend);
        }

        public static ResponseResult ErrorDataNotFoundResponse()
        {
            return GetErrorResponse(ERROR_DATANOTFOUND);
        }

        public static ResponseResult GetErrorResponse(string errMessage = "系统异常", int state = -1, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ResponseResult
            {
                HttpCode = statusCode,
                State = -1,
                Message = errMessage
            };
        }
    }
}
