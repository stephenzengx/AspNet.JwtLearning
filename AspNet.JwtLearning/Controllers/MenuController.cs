using AspNet.JwtLearning.BLL;
using AspNet.JwtLearning.Helpers;
using AspNet.JwtLearning.Models.AdminEntity;
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
        public AdminBLL adminBLL;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="addminBLL"></param>
        public MenuController(AdminBLL addminBLL)
        {
            this.adminBLL = addminBLL;
        }

        public class AdminRoleInfoClass
        {
            public string roleId { get; set; }
            public string roleNanme { get; set; }
            public string tenantId { get; set; }
            public string tenantName { get; set; }
        }

        public HttpResponseMessage AdminRoleInfoList()
        {

            return new HttpResponseMessage();
        }

        /// <summary>
        /// 获得系统菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage AdminMenu()
        {
            List<TreeNode> treeNodeList = new List<TreeNode>();
            List<Node> allNode = adminBLL.GetAdminAllMenuNode();

            //遍历所有的根节点
            var rootList = allNode.Where(s => s.ParentId == 0).OrderBy(m=>m.Sort).ToList();
            foreach (var ent in rootList)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.NodeId = ent.NodeId;   
                treeNode.NodeName = ent.NodeName;
                //treeNode.ParentId = ent.ParentId;
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