using System;
using System.Configuration;

namespace AspNet.JwtLearning.Utility
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
    }
}