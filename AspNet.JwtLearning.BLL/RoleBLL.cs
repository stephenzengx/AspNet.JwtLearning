using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility.BaseHelper;
using AspNet.JwtLearning.Utility.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.JwtLearning.BLL
{
    public class RoleBLL
    {
        private readonly AuthDbContext authDbContext;

        public RoleBLL(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        /// <summary>
        /// 获取(租户)角色列表)
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        public async Task<List<AdminRoleInfoClass>> GetAdminRoleInfoList(int tenantId)
        {
            var ret = new List<AdminRoleInfoClass>();
            
            var roles = (await RedisBLL.GetRoleInfos()).Where(m=>m.tenantId!=0).ToList();

            var tenants = await RedisBLL.GetSysTenants();
            if (Utils.IsNullOrEmptyList(roles))
                return ret;
            if (tenantId > 0)
                roles = roles.Where(m => m.tenantId == tenantId).ToList();

            foreach (var role in roles)
            {
                ret.Add(new AdminRoleInfoClass
                {
                    roleId = role.roleId,
                    roleName = role.roleName,
                    tenantId = tenants.FirstOrDefault(m => m.tenantId == role.tenantId)?.tenantId,
                    tenantName = tenants.FirstOrDefault(m => m.tenantId == role.tenantId)?.tenantName
                }); 
            }

            return ret;
        }

        /// <summary>
        /// 角色授权菜单权限
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AdminAuthMenuByIds(AdminAuthClass model)
        {
            if (model == null || Utils.IsNullOrEmptyList(model.selectIds))
                throw new ArgumentException("AdminAuthMenuClass data error!");

            var delList = await authDbContext.tb_role_accessMenus.Where(m => m.roleId == model.roleId).ToListAsync();
            if (delList.Count>0 && (await authDbContext.Database.ExecuteSqlCommandAsync($"delete from tb_role_accessMenu where roleId = {model.roleId}"))<=0)
                throw new Exception("database exec error");

            var sysMenuNodes = await MenuBLL.GetSystemMenuNodes();
            if (Utils.IsNullOrEmptyList(sysMenuNodes))
                return new ResponseResult();

            var rootMenuIds = new HashSet<int>();
            var insertRoleMenus = new List<tb_role_accessMenu>();

            foreach (var menuId in model.selectIds)
            {
                var curNode = sysMenuNodes.FirstOrDefault(m => m.NodeId == menuId);
                if (curNode == null)
                    continue;

                var node = Utils.GetRootNode(curNode, sysMenuNodes);
                if (node != null)
                    rootMenuIds.Add(node.NodeId);

                insertRoleMenus.Add(new tb_role_accessMenu { 
                    roleId = model.roleId,
                    menuId = menuId,
                    isSelect = true,
                    addTime = DateTime.Now
                });
            }

            if (rootMenuIds.Count < 0)
                throw new ArgumentException("args empty");

            foreach (var menuId in rootMenuIds)
            {
                insertRoleMenus.Add(new tb_role_accessMenu
                {
                    roleId = model.roleId,
                    menuId = menuId,
                    isSelect = false,
                    addTime = DateTime.Now
                });
            }

            authDbContext.tb_role_accessMenus.AddRange(insertRoleMenus);
            if (await authDbContext.SaveChangesAsync() <= 0)
                return ResponseHelper.GetOkResponse(null,"授权失败，重试", -1);

            RedisBLL.SetCacheRoleMenus(await authDbContext.tb_role_accessMenus.ToListAsync());
            return ResponseHelper.GetOkResponse(null, "授权成功", 0);
        }

        /// <summary>
        /// 角色授权api
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AdminAuthApiByIds(AdminAuthClass model)
        {
            if (model == null || Utils.IsNullOrEmptyList(model.selectIds))
                throw new ArgumentException("AdminAuthMenuClass data error!");

            var delList = await authDbContext.tb_role_accessMenus.Where(m => m.roleId == model.roleId).ToListAsync();
            if (delList.Count > 0 && (await authDbContext.Database.ExecuteSqlCommandAsync($"delete from tb_role_accessApi where roleId = {model.roleId}")) <= 0)
                throw new Exception("database exec error");

            List<tb_role_accessApi> insertRoleApis = new List<tb_role_accessApi>();
            foreach (var apiId in model.selectIds)
            {
                insertRoleApis.Add(new tb_role_accessApi { 
                    apiId = apiId,
                    roleId = model.roleId
                });
            }

            authDbContext.tb_role_accessApis.AddRange(insertRoleApis);
            if ( await authDbContext.SaveChangesAsync() <= 0)
                return ResponseHelper.GetOkResponse(null, "授权失败，重试", -1);

            RedisBLL.SetCacheRoleApis(await authDbContext.tb_role_accessApis.ToListAsync());
            return ResponseHelper.GetOkResponse(null, "授权成功", 0);
            
        }

        /// <summary>
        /// 角色授权按钮
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> AdminAuthBtnByIds(AdminAuthClass model)
        {
            if (model == null || Utils.IsNullOrEmptyList(model.selectIds))
                throw new ArgumentException("AdminAuthMenuClass data error!");

            var delList = await authDbContext.tb_role_accessMenus.Where(m => m.roleId == model.roleId).ToListAsync();
            if (delList.Count > 0 && (await authDbContext.Database.ExecuteSqlCommandAsync($"delete from tb_role_accessBtn where roleId = {model.roleId}")) <= 0)
                throw new Exception("database exec error");


            List<tb_role_accessBtn> insertRoleBtns = new List<tb_role_accessBtn>();
            foreach (var btnId in model.selectIds)
            {
                insertRoleBtns.Add(new tb_role_accessBtn
                {
                    btnId = btnId,
                    roleId = model.roleId,
                    menuId = model.menuId
                });
            }

            authDbContext.tb_role_accessBtns.AddRange(insertRoleBtns);
            if (await authDbContext.SaveChangesAsync() <= 0)
                return ResponseHelper.GetOkResponse(null, "授权失败，重试", -1);

            RedisBLL.SetCacheRoleBtns(await authDbContext.tb_role_accessBtns.ToListAsync());
            return ResponseHelper.GetOkResponse(null, "授权成功", 0);           
        }
    }
}
