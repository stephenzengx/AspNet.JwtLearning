using System.Net.Http;
using System.Web.Http;

using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 系统操作控制器
    /// </summary>
    public class SystemController : ApiController
    {
        public UserBLL userBLL;

        public SystemController(UserBLL userBLL)
        {
            this.userBLL = userBLL;
        }
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Login([FromBody] LoginModel user)
        {
            var ret = userBLL.Login(user);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Register([FromBody] tb_tenant_user user)
        {
            var ret = userBLL.Register(user);

            return ResponseFormat.GetResponse(ret);
        }
    }
}
