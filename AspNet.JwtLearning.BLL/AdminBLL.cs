using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Models.Tree;
using AspNet.JwtLearning.Utility.BaseHelper;
using System.Collections.Generic;
using System.Linq;

namespace AspNet.JwtLearning.BLL
{
    public class AdminBLL
    {
        public AuthDbContext authDbContext;

        /// <summary>
        /// 构造方法 注入 DbContext
        /// </summary>
        /// <param name="authDbContext"></param>
        public AdminBLL(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        /// <summary>
        /// 获取所有菜单节点列表
        /// </summary>
        /// <returns></returns>
        public List<Node> GetAdminAllMenuNode()
        {
            return authDbContext.Database
                .SqlQuery<Node>("select menuId as NodeId, parentId as ParentId,menuName as NodeName, sort from tb_system_menu where isEnable=1")
                .ToList();
        }

        public void GetAdminRoleInfoList()
        {
            List<tb_system_roleInfo> roleInfos = authDbContext.tb_system_roleInfos.OrderBy(m => m.tenantId).ThenBy(m => m.roleId).ToList();
        }
    }
}
