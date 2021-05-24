using System.Net;

namespace AspNet.JwtLearning.Utility.BaseHelper
{
    public class ResponseResult
    {
        public ResponseClass ResponseClass { get; set; }
        public HttpStatusCode HttpCode { get; set; } //http状态码
    }

    public class ResponseClass
    {    
        public int Status { get; set; } //业务码
        public string Message { get; set; }
        public object Record { get; set; }
        public int TotalCount { get; set; }
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

        public static ResponseResult GetOkResponse(object obj = null,string message = "操作成功")
        {
            return new ResponseResult
            {
                ResponseClass = new ResponseClass
                {
                    Status = 0,
                    Message = message,
                    Record = obj
                },
                HttpCode = HttpStatusCode.OK
            };
        }

        public static ResponseResult GetListPageResponse(object obj,int totalCount, string message = "查询成功")
        {
            return new ResponseResult
            {
                ResponseClass = new ResponseClass
                {
                    Status = 0,
                    Message = message,
                    Record = obj,
                    TotalCount = totalCount
                },
                HttpCode = HttpStatusCode.OK
            };
        }

        public static ResponseResult SuccessAddResponse(string id)
        {
            return new ResponseResult
            {
                ResponseClass = new ResponseClass
                {
                    Status = 0,
                    Message = SUCCESS_ADD,
                    Record = id
                },
                HttpCode = HttpStatusCode.OK
            };
        }
        public static ResponseResult SuccessUpdateResponse()
        {
            return new ResponseResult
            {
                ResponseClass = new ResponseClass
                {
                    Status = 0,
                    Message = SUCCESS_UPDATE
                },
                HttpCode = HttpStatusCode.OK
            };
        }
        public static ResponseResult SuccessDeleteResponse()
        {
            return GetOkResponse(null, SUCCESS_DELETE);
        }

        public static ResponseResult ErrorParamResponse(string apppend = "")
        {
            return GetErrorResponse(ERROR_PARAM + apppend);
        }

        public static ResponseResult ErrorDataNotFoundResponse()
        {
            return GetErrorResponse(ERROR_DATANOTFOUND);
        }

        public static ResponseResult GetErrorResponse(string errMessage = "系统异常", int status = -1, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            return new ResponseResult
            {
                ResponseClass = new ResponseClass
                {
                    Status = status,
                    Message = errMessage
                },
                HttpCode = statusCode
            };
        }
    }
}
