using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility.BaseHelper;
using AspNet.JwtLearning.Utility.TokenHandle;
using Newtonsoft.Json;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 系统操作控制器
    /// </summary>
    public class SystemController : ApiController
    {
        private readonly UserBLL userBLL;

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
        public async Task<HttpResponseMessage> Login([FromBody] LoginModel user)
        {
            var ret = await userBLL.Login(user);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> Register([FromBody] tb_user user)
        {
            var ret = await userBLL.Register(user);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 用户菜单树 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> UserMenuTree()
        {

            var jsonModel = Request.Properties["userinfo"].ToString();
            var model = JsonConvert.DeserializeObject<JwtContainerModel>(jsonModel);

            var menuTrue = await MenuBLL.GetUserMenuTree(model.UserId);

            return ResponseFormat.GetResponse(ResponseHelper.GetOkResponse(menuTrue));
        }

        /// <summary>
        /// 用户菜单(页面)按钮权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> UserMenuBtnRight(int menuId)
        {
            var jsonModel = Request.Properties["userinfo"].ToString();
            var model = JsonConvert.DeserializeObject<JwtContainerModel>(jsonModel);
            
            var ret = await userBLL.GetMenuBtnRight(model.UserId, menuId);        

            return ResponseFormat.GetResponse(ret);
        }
    }
}
