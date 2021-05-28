using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Utility.Redis;
using System;
using System.Linq;

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
        #endregion
        public void Init_GlobalInfoCache()
        {                  
            //初始化缓存         
            using (AuthDbContext db = new AuthDbContext())
            {
                RedisHelper redisHelper = new RedisHelper();
                if (!redisHelper.KeyExists(tenants))
                {
                    var list = db.tb_tenant_infos.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(tenants, list, cacheExpireHour);
                }
                if (!redisHelper.KeyExists(roleInfos))
                {
                    var list = db.tb_system_roleInfos.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleInfos, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(userRoles))
                {
                    var list = db.tb_user_roles.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(userRoles, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(menus))
                {
                    var list = db.tb_system_menus.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(menus, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleMenus))
                {
                    var list = db.tb_role_accessMenus.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleMenus, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(buttons))
                {
                    var list = db.tb_menu_buttons.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(buttons, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleButtons))
                {
                    var list = db.tb_role_accessMenus.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleButtons, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(apiInfos))
                {
                    var list = db.tb_system_apis.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(apiInfos, list, cacheExpireHour);
                }

                if (!redisHelper.KeyExists(roleApiInfos))
                {
                    var list = db.tb_role_accessApis.ToList();
                    if (list.Count > 0)
                        redisHelper.StringSet(roleApiInfos, list, cacheExpireHour);
                }
            }
        }
    }
}
