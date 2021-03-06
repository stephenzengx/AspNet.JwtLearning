using System.Net.Http;
using System.Threading.Tasks;
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
        private readonly UserBLL userBLL;

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
        public async Task<HttpResponseMessage> ListByPage(int pageIndex, int pageSize)
        {
            var retTuple = await userBLL.GetListByPage(pageIndex, pageSize,m=>m.userId>0 ,m=>m.userId);

            var response = ResponseHelper.GetListPageResponse(retTuple.Item2, retTuple.Item1);

            return ResponseFormat.GetResponse(response);
        }

        /// <summary>
        /// 获取租户用户详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetOne(int userId)
        {
            var ret = await userBLL.FirstOrDefaultAsync(m => m.userId == userId);
            return ResponseFormat.GetResponse(ResponseHelper.GetOkResponse(ret));
        }

        /// <summary>
        /// 新增租户用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] tb_user user)
        {
            user.isEnable = true;
            user.passWord = RSAHelper.Encrypt(user.passWord);

            bool ret = await userBLL.AddAsync(user);
            var respRet = (ret ? ResponseHelper.SuccessAddResponse(user.userId.ToString()) : ResponseHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }

        /// <summary>
        /// 修改租户用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<HttpResponseMessage> Put([FromBody] tb_user user)
        {
            bool ret = await userBLL.UpdateAsync(user);
            var respRet = (ret ? ResponseHelper.SuccessUpdateResponse() : ResponseHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }

        /// <summary>
        /// 删除租户用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<HttpResponseMessage> AdminDelete(int userId)
        {
            bool ret = await userBLL.DeleteAsync(userId);
            var respRet = (ret ? ResponseHelper.SuccessDeleteResponse() : ResponseHelper.GetErrorResponse());

            return ResponseFormat.GetResponse(respRet);
        }
    }
}
