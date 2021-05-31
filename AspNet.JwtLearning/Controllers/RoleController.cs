using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Utility.BaseHelper;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : ApiController
    {
        public RoleBLL roleBLL;

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
        public HttpResponseMessage AdminRoleList(int tenantId)
        {
            var list = roleBLL.GetAdminRoleInfoList(tenantId);
           
            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(list));
        }

        /// <summary>
        /// 角色绑定菜单 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HttpResponseMessage AdminAuthMenus([FromBody] AdminAuthMenuClass model)
        {
            var list = string.Empty;

            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(list));
        }

        /// <summary>
        /// 角色绑定api to do
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public HttpResponseMessage AdminAuthApis(int roleId)
        {
            var list = string.Empty;

            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(list));
        }

        /// <summary>
        /// 角色绑定按钮
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public HttpResponseMessage AdminAuthBtns(int roleId)
        {
            var list = string.Empty;

            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(list));
        }
    }
}
