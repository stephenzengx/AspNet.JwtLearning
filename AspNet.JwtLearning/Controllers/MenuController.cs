using System.Net.Http;
using System.Web.Http;

using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Utility.BaseHelper;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : ApiController
    {
        /// <summary>
        /// （后台）系统菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AdminSystemMenuTree()
        {
            //?? to do 
            //在控制器层 为什么加了 using AspNet.JwtLearning.Utility.Common 引用，
            //没法直接还得这样 Utility.Common.Utils 调用Utils

            //而 AutofacConfig, UserBLL里面 缺可以直接使用LogHelper, Utils
            var menuTrue= MenuBLL.GetSystemMenuTree();

            return ResponseFormat.GetResponse(ResponseHelper.GetOkResponse(menuTrue));
        }

        /// <summary>
        /// (后台) 角色菜单 menuId集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AdminRoleAccessMenuIds(int roleId)
        {
            var list = MenuBLL.GetRoleAccessMenuIds(roleId);

            var result = ResponseHelper.GetOkResponse(list);

            return ResponseFormat.GetResponse(result);
        }
    }
}