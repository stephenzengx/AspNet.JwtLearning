using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility.Redis;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace AspNet.JwtLearning.BLL
{
    public class RedisBLL
    {
        #region 变量
        protected static string tenants = "tenants";
        protected static string roleInfos = "roleInfos";
        protected static string userRoles = "userRoles";
        protected static string menus = "menus";
        protected static string roleMenus = "roleMenus";
        protected static string buttons = "buttons";//menu_button
        protected static string roleButtons = "roleButtons";
        protected static string apiInfos = "apiInfos";
        protected static string roleApiInfos = "roleApiInfos";
        protected static TimeSpan cacheExpireMinute = TimeSpan.FromMinutes(300);
        protected static TimeSpan cacheExpireHour = TimeSpan.FromHours(5);
        protected static RedisHelper redisHelper = RedisHelper.GetInstance();
        #endregion

        /// <summary>
        /// 初始化全局 redis 缓存
        /// </summary>
        public async static void InitGlobalInfoCache()
        {            
            //初始化缓存         
            using (AuthDbContext authDbContext = new AuthDbContext())
            {          
                if (!redisHelper.KeyExists(tenants))
                {
                    var list = await authDbContext.tb_tenant_infos.Where(m=>m.isEnable).ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(tenants, list, cacheExpireHour);
                }
                if (!redisHelper.KeyExists(roleInfos))
                {
                    var list = await authDbContext.tb_roleInfos.Where(m => m.isEnable).ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleInfos, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(userRoles))
                {
                    var list = await authDbContext.tb_user_roles.ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(userRoles, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(menus))
                {
                    var list = await authDbContext.tb_system_menus.Where(m => m.isEnable).ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(menus, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleMenus))
                {
                    var list = await authDbContext.tb_role_accessMenus.ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleMenus, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(buttons))
                {
                    var list = await authDbContext.tb_menu_buttons.Where(m => m.isEnable).ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(buttons, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleButtons))
                {
                    var list = await authDbContext.tb_role_accessBtns.ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleButtons, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(apiInfos))
                {
                    var list = await authDbContext.tb_system_apis.Where(m => m.isEnable).ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(apiInfos, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleApiInfos))
                {
                    var list = await authDbContext.tb_role_accessApis.ToListAsync();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleApiInfos, list, cacheExpireHour);
                }
            }
        }

        /// <summary>
        /// 系统租户列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_tenant_info>> GetSysTenants()
        {
            if (redisHelper.KeyExists(tenants))
                return redisHelper.StringGet<List<tb_tenant_info>>(tenants);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await authDbContext.tb_tenant_infos.Where(m => m.isEnable).ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(tenants, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 系统菜单列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_system_menu>> GetSysMenus()
        {
            if (redisHelper.KeyExists(menus))
                return redisHelper.StringGet<List<tb_system_menu>>(menus);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await authDbContext.tb_system_menus.Where(m=>m.isEnable).ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(menus, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 系统角色列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_roleInfo>> GetRoleInfos()
        {
            if (redisHelper.KeyExists(roleInfos))
                return redisHelper.StringGet<List<tb_roleInfo>>(roleInfos);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await authDbContext.tb_roleInfos.Where(m => m.isEnable && m.tenantId != 0).ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(roleInfos, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 系统api列表
        /// </summary>
        /// <returns></returns>

        public async static Task<List<tb_system_api>> GetSysApiInfos()
        {
            if (redisHelper.KeyExists(apiInfos))
                return redisHelper.StringGet<List<tb_system_api>>(apiInfos);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await  authDbContext.tb_system_apis.Where(m => m.isEnable).ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(apiInfos, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 系统菜单按钮列表
        /// </summary>
        /// <returns></returns>

        public async static Task<List<tb_menu_button>> GetSysMenuButtons()
        {
            if (redisHelper.KeyExists(buttons))
                return redisHelper.StringGet<List<tb_menu_button>>(buttons);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await authDbContext.tb_menu_buttons.Where(m => m.isEnable).ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(buttons, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 角色菜单权限列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_role_accessMenu>> GetRoleMenus()
        {
            if (redisHelper.KeyExists(roleMenus))
                return redisHelper.StringGet<List<tb_role_accessMenu>>(roleMenus);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await  authDbContext.tb_role_accessMenus.ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(roleMenus, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 用户角色信息列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_user_role>> GetUserRoles()
        {
            if (redisHelper.KeyExists(userRoles))
                return redisHelper.StringGet<List<tb_user_role>>(userRoles);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await  authDbContext.tb_user_roles.ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(userRoles, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 角色api权限列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_role_accessApi>> GetRoleApiInfos()
        {
            if (redisHelper.KeyExists(roleApiInfos))
                return redisHelper.StringGet<List<tb_role_accessApi>>(roleApiInfos);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await  authDbContext.tb_role_accessApis.ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(roleApiInfos, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 角色按钮权限列表
        /// </summary>
        /// <returns></returns>
        public async static Task<List<tb_role_accessBtn>> GetRoleMenuButtons()
        {
            if (redisHelper.KeyExists(roleButtons))
                return redisHelper.StringGet<List<tb_role_accessBtn>>(roleButtons);

            using (AuthDbContext authDbContext = new AuthDbContext())
            {
                var list = await authDbContext.tb_role_accessBtns.ToListAsync();
                if (list.Count > 0)
                    redisHelper.StringSet(roleButtons, list, cacheExpireHour);

                return list;
            }
        }

        /// <summary>
        /// 获取用户roleId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async static Task<int> GetRoleId(int userId)
        {
            var UserRoles = await GetUserRoles();
            var role = UserRoles.FirstOrDefault(m => m.userId == userId);
            if (role == null)
                throw new Exception("userId:"+userId+" need bind role");

            return  role.roleId;
        }

        /// <summary>
        /// 保存角色菜单权限列表 （缓存）
        /// </summary>
        /// <param name="list"></param>
        public static void SetCacheRoleMenus(List<tb_role_accessMenu> list)
        {
            if (list == null)
                list = new List<tb_role_accessMenu>();

            redisHelper.StringSet(roleMenus, list, cacheExpireHour);
        }

        /// <summary>
        /// 保存角色授权Api
        /// </summary>
        /// <param name="list"></param>
        public static void SetCacheRoleApis(List<tb_role_accessApi> list)
        {
            if (list == null)
                list = new List<tb_role_accessApi>();

            redisHelper.StringSet(roleApiInfos, list, cacheExpireHour);
        }

        /// <summary>
        /// 保存角色授权按钮
        /// </summary>
        /// <param name="list"></param>
        public static void SetCacheRoleBtns(List<tb_role_accessBtn> list)
        {
            if (list == null)
                list = new List<tb_role_accessBtn>();

            redisHelper.StringSet(roleMenus, list, cacheExpireHour);
        }
    }
}
