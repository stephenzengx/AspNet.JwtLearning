﻿using System.Net.Http;
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
        public MenuBLL menuBLL;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="menuBLL"></param>
        public MenuController(MenuBLL menuBLL)
        {
            this.menuBLL = menuBLL;
        }

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
            var menuTrue= menuBLL.GetSystemMenuTree();

            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(menuTrue));
        }

        /// <summary>
        /// (后台) 角色菜单 menuId集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public HttpResponseMessage AdminRoleMenuIdList(int roleId)
        {
            var list = menuBLL.GetAdminRoleMenuIdList(roleId);

            var result = ResultHelper.GetOkResponse(list);

            return ResponseFormat.GetResponse(result);
        }

        /// <summary>
        /// 获得用户(菜单)菜单 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage UserMenuTree(int userId)
        {
            var menuTrue = menuBLL.GetUserMenuTree(userId);

            return ResponseFormat.GetResponse(ResultHelper.GetOkResponse(menuTrue));
        }

        public class AdminRoleInfoClass
        {
            public string roleId { get; set; }
            public string roleNanme { get; set; }
            public string tenantId { get; set; }
            public string tenantName { get; set; }
        }

        /// <summary>
        /// 某个租户或所有角色列表信息
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage RoleInfoList(int tenantId)
        {
            return new HttpResponseMessage();
        }

    }
}