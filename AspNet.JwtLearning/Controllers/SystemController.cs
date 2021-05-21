using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility.BaseHelper;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 系统操作控制器
    /// </summary>
    public class SystemController : ApiController
    {
        //public UserBLL userBLL;

        //public SystemController(UserBLL userBLL)
        //{
        //    this.userBLL = userBLL;
        //}

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Login([FromBody]LoginModel user)
        {
            //var ret = userBLL.Login(user);
            var ret = new ResponseResult();

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody]tb_tenant_user user)
        {
            //var ret = userBLL.Register(user);
            var ret = new ResponseResult();

            return ResponseFormat.GetResponse(ret);
        }
    }
}
