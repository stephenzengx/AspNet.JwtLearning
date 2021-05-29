using System.Net.Http;
using System.Web.Http;

using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility;
using AspNet.JwtLearning.Utility.BaseHelper;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 测试api控制器
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// userBLL
        /// </summary>
        public UserBLL userBLL;

        /// <summary>
        /// 构造方法 ioc容器注入
        /// </summary>
        /// <param name="userBLL"></param>
        public UserController(UserBLL userBLL)
        {
            this.userBLL = userBLL;
        }

        /// <summary>
        /// 获取分页列表信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage ListByPage(int pageIndex, int pageSize)
        {
            int totalCount;
            var list = userBLL.GetListByPage(pageIndex, pageSize,m=>m.userId>0 ,m=>m.userId,out totalCount);

            var ret = ResultHelper.GetListPageResponse(list,totalCount);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 获取租户用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetOne(int userId)
        {
            var ret = userBLL.FirstOrDefault(m => m.userId == userId);
            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(ret));
        }

        /// <summary>
        /// 新增租户用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody] tb_user user)
        {
            user.isEnable = true;
            user.passWord = RSAHelper.Encrypt(user.passWord);

            bool ret = userBLL.Add(user);
            var respRet = (ret ? ResultHelper.SuccessAddResponse(user.userId.ToString()) : ResultHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }

        /// <summary>
        /// 修改租户用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public HttpResponseMessage Put([FromBody] tb_user user)
        {
            bool ret = userBLL.Update(user);
            var respRet = (ret ? ResultHelper.SuccessUpdateResponse() : ResultHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }

        /// <summary>
        /// 删除租户用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        public HttpResponseMessage Delete(int userId)
        {
            bool ret = userBLL.Delete(userId);
            var respRet = (ret ? ResultHelper.SuccessDeleteResponse() : ResultHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }
    }
}
