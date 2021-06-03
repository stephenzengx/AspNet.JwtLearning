using System;
using System.Collections.Generic;
using System.Configuration;

namespace AspNet.JwtLearning.Utility.Common
{
    public class ConfigConst
    {
        public static string DocExpansionValue
        {
            get
            {
                var _DocExpansion = ConfigurationManager.AppSettings["DocExpansion"];

                return _DocExpansion == null ? "1" : _DocExpansion;
            }
        }

        public static bool IsStage
        {
            get
            {
                var modeSetting = ConfigurationManager.AppSettings["Mode"];
                return modeSetting != null && modeSetting.ToString().Equals("Stage", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public static bool IsProduct
        {
            get
            {
                var modeSetting = ConfigurationManager.AppSettings["Mode"];
                return modeSetting != null && modeSetting.ToString().Equals("Product", StringComparison.CurrentCultureIgnoreCase);
            }
        }

        public static string AllowOrigins
        {
            get
            {
                var allowOrigins = ConfigurationManager.AppSettings["AllowOrigins"];
                return allowOrigins != null ? allowOrigins : string.Empty;
            }
        }

        public static string AuthHeaderName
        {
            get
            {
                var allowOrigins = ConfigurationManager.AppSettings["AuthHeaderName"];
                return allowOrigins != null ? allowOrigins : string.Empty;
            }
        }

        public static string RedisAddrFortest
        {
            get
            {                
                return ConfigurationManager.ConnectionStrings["redisfortest"].ConnectionString;
            }
        }

        /// <summary>
        /// 无需验证token (后台管理系统)
        /// </summary>
        public static List<string> ignoreTokenCheckUrlKey = new List<string> { "Admin" };

        /// <summary>
        /// 无需验证api权限 (登录,注册,用户菜单树,页面按钮权限)
        /// </summary>
        public static List<string> ignoreApiRightCheckUrlKey = new List<string> { "Admin","System" };
    }
}