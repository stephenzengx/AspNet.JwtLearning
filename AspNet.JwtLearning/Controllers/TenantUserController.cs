using System.Net.Http;
using System.Web.Http;

using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility.BaseHelper;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 测试api控制器
    /// </summary>
    public class TenantUserController : ApiController
    {
        public UserBLL userBLL;

        public TenantUserController(UserBLL userBLL)
        {
            this.userBLL = userBLL;
        }

        [HttpGet]
        public HttpResponseMessage ListByPage(int pageIndex, int pageSize)
        {
            int totalCount;
            var list = userBLL.GetListByPage(pageIndex, pageSize,m=>m.userId>0 ,m=>m.userId,out totalCount);

            var ret = ResponseHelper.GetListPageResponse(list,totalCount);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 获取租户用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public string Get(int userId)
        {
            return "value get";
        }

        /// <summary>
        /// 新增租户用户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        public string Post([FromBody] string value)
        {
            return "value post";
        }

        /// <summary>
        /// 修改租户用户信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut]
        public string Put([FromBody] string value)
        {
            return "value put";
        }

        /// <summary>
        /// 删除租户用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public string Delete(int id)
        {
            return "value delete";
        }
    }
}
