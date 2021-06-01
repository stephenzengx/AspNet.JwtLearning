using System;
using System.Collections.Generic;
using System.Linq;

using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Models.Tree;
using AspNet.JwtLearning.Utility.Common;

namespace AspNet.JwtLearning.BLL
{
    public class MenuBLL
    {
        /// <summary>
        /// 获取系统菜单节点列表
        /// </summary>
        /// <returns></returns>
        public static List<Node> GetSystemMenuNodes()
        {
            var menuList = RedisBLL.GetSystemMenus();
            var nodeList = new List<Node>();
            foreach (var item in menuList)
            {
                nodeList.Add(new Node { 
                    NodeId = item.menuId,
                    ParentId = item.parentId,
                    NodeName = item.menuName,
                    Sort = item.sort
                });
            }
            return nodeList;
        }

        /// <summary>
        /// 获取系统菜单树
        /// </summary>
        /// <returns></returns>
        public static List<TreeNode> GetSystemMenuTree()
        {
            List<Node> allNode = GetSystemMenuNodes();

            var treeNodeList = Utils.BuildTree(allNode);

            return treeNodeList;
        }

        /// <summary>
        /// 获取用户(角色)菜单节点列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<Node> GetUserAccessMenuNodes(int userId)
        {
            var UserRoles = RedisBLL.GetUserRoles();

            var role = UserRoles.FirstOrDefault(m => m.userId == userId);
            if (role == null)
                throw new ArgumentException("userId not found");

            List<tb_role_accessMenu> roleMenuList = RedisBLL.GetRoleMenus().Where(m => m.roleId == role.roleId).ToList();
            if (Utils.IsNullOrEmptyList(roleMenuList))
                return new List<Node>();

            var menuIdList = roleMenuList.Select(m => m.menuId).ToList();
            List<tb_system_menu> menusList = RedisBLL.GetSystemMenus().Where(m => menuIdList.Contains(m.menuId)).ToList();

            List<Node> nodeList = new List<Node>();
            foreach (var item in menusList)
            {
                nodeList.Add(new Node
                {
                    NodeId = item.menuId,
                    ParentId = item.parentId,
                    NodeName = item.menuName,
                    Sort = item.sort
                });
            }
            return nodeList;
        }

        /// <summary>
        /// 获取用户菜单树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<TreeNode> GetUserMenuTree(int userId)
        {
            var nodeList = GetUserAccessMenuNodes(userId);
            if (Utils.IsNullOrEmptyList(nodeList))
                return new List<TreeNode>();

            return Utils.BuildTree(nodeList);
        }

        /// <summary>
        /// 获取角色菜单 menuId集合
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static List<int> GetRoleAccessMenuIds(int roleId)
        {
            if (roleId <= 0)
                throw new ArgumentException("roleId should be larger than zero");

            var list = RedisBLL.GetRoleMenus()
                            .Where(m => m.roleId == roleId && m.isSelect)
                            .Select(m => m.menuId).ToList();

            return list;
        }

        
    }
}
