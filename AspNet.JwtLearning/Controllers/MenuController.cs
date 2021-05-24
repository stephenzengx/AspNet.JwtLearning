using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models.Tree;
using AspNet.JwtLearning.Utility.BaseHelper;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace AspNet.JwtLearning.Controllers
{
    /// <summary>
    /// 菜单控制器
    /// </summary>
    public class MenuController : ApiController
    {
        public AuthDbContext authDbContext;

        /// <summary>
        /// 构造方法 注入 DbContext
        /// </summary>
        /// <param name="authDbContext"></param>
        public MenuController(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        /// <summary>
        /// 获得系统菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage SystemMenu()
        {
            List<TreeNode> treeNodeList = new List<TreeNode>();
            List<Node> allNode  = authDbContext.Database
                .SqlQuery<Node>("select menuId as NodeId, parentId as ParentId,menuName as Label, sort from tb_system_menu where isEnable=1")
                .ToList();

            //遍历所有的根节点
            var rootList = allNode.Where(s => s.ParentId == 0).OrderBy(m=>m.sort).ToList();
            foreach (var ent in rootList)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.NodeId = ent.NodeId;
                treeNode.Label = ent.Label;
                treeNode.ParentId = ent.ParentId;
                treeNode.Children = Utility.Common.Utils.GetChildrenTree(ent.NodeId, allNode);
                treeNodeList.Add(treeNode);
            }

            return ResponseFormat.GetResponse(ResponseHelper.GetOkResponse(treeNodeList));
        }

        /// <summary>
        /// 获得角色菜单 (跟RoleId关联)
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage RoleMenu(int userId)
        {
            //通过 userId 在redis里面找到roleId
            return new HttpResponseMessage();
        }
    }
}