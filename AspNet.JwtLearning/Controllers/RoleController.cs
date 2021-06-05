using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Utility.BaseHelper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : ApiController
    {
        private readonly RoleBLL roleBLL; //依赖注入属性 私有只读

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="roleBLL"></param>
        public RoleController(RoleBLL roleBLL)
        {
            this.roleBLL = roleBLL;
        }

        /// <summary>
        /// 获取(租户)角色列表)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> AdminRoleList(int tenantId=0)
        {
            var list = await roleBLL.GetAdminRoleInfoList(tenantId);
           
            return ResponseFormat.GetResponse(ResponseHelper.GetOkResponse(list));
        }

        /// <summary>
        /// 角色授权菜单 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResponseMessage> AdminAuthMenuByIds([FromBody] AdminAuthClass model)
        {
            var ret = await roleBLL.AdminAuthMenuByIds(model);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 角色授权api 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> AdminAuthApis([FromBody] AdminAuthClass model)
        {
            var ret = await roleBLL.AdminAuthApiByIds(model);

            return ResponseFormat.GetResponse(ret);
        }

        /// <summary>
        /// 角色授权按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> AdminAuthBtns([FromBody] AdminAuthClass model)
        {
            var ret = await roleBLL.AdminAuthBtnByIds(model);

            return ResponseFormat.GetResponse(ret);
        }
    }
}
