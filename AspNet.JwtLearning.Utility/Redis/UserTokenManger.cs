using AspNet.JwtLearning.Utility.Common;
using AspNet.JwtLearning.Utility.TokenHandle;
using System;

namespace AspNet.JwtLearning.Utility.Redis
{
    public class UserTokenManger
    {
        public const string SufixTokenKey = "_tk";
        public const string SufixRefreshTokenKey = "_rtk";

        //public static readonly TimeSpan TsToken = TimeSpan.FromMinutes(1);
        //public static readonly TimeSpan TsRefreshToken = TimeSpan.FromMinutes(120);

        public static RedisHelper _helper = null;

        static UserTokenManger()
        {
            //new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            _helper = new RedisHelper(0, ConfigConst.RedisAddrFortest);//根据测试地址-正式地址 选择数据库Number
        }

        public static void SetUserToken_v1(string userid, string token)
        {
            _helper.StringSet(userid + SufixTokenKey, token, TimeSpan.FromMinutes(JWTService._tokenExpireMinToken));
        }

        public static string GetUserToken(string userid)
        {
            return  _helper.StringGet(userid+ SufixTokenKey);
        }


        public static string GetRefreshToken(string userid)
        {
            return _helper.StringGet(userid + SufixRefreshTokenKey);
        }
    }
}