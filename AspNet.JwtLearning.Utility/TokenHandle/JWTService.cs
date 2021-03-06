using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AspNet.JwtLearning.Utility.TokenHandle
{
    public static class CustomClaimTypes
    {
        public const string Userid = "userid";
        public const string TimeStamp = "guid";
    }

    public class JwtContainerModel
    {
        //后续可以通过usreid,username 以及rolename进行授权 (放一些前端需要的常用信息)
        public int UserId { get; set; }
        public string TimeStamp { get; set; }
    }

    /// <summary>
    /// JWTService
    /// </summary>
    public class JWTService 
    {
        private static string _secretKey { get; set; } = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
        private static string _securityAlgorithm { get; set; } = SecurityAlgorithms.HmacSha256;//HS256 对称加密算法 

        /// <summary>
        /// token过期时间
        /// </summary>
        public static double _tokenExpireMinToken = 60 * 24 * 2;//两天

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GenerateToken(JwtContainerModel model)
        {
            byte[] key = Convert.FromBase64String(_secretKey);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new List<Claim> {
                      new Claim(CustomClaimTypes.Userid, model.UserId.ToString()),
                      new Claim(CustomClaimTypes.TimeStamp, model.TimeStamp),
                }),
                //秘钥和加密方式
                SigningCredentials = new SigningCredentials(securityKey, _securityAlgorithm),
                Expires = DateTime.Now.AddMinutes(_tokenExpireMinToken)//过期时间
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }

        /// <summary>
        /// 验证token方法
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JwtContainerModel ValidateToken(string token)
        {
            return  TransferToModel(token);
        }

        /// <summary>
        /// 前端 token验证
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static JwtContainerModel TransferToModel(string token)
        {
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;

            ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
            if (identity == null)
                return null;

            return new JwtContainerModel
            {
                UserId = Convert.ToInt32(identity.FindFirst(CustomClaimTypes.Userid)?.Value),
                TimeStamp = identity.FindFirst(CustomClaimTypes.TimeStamp).Value
            };
        }

        private static ClaimsPrincipal GetPrincipal(string token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            if (jwtToken == null)
                return null;

            byte[] key = Convert.FromBase64String(_secretKey);
            TokenValidationParameters parameters = new TokenValidationParameters()
            {
                RequireExpirationTime = false,                     
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
            SecurityToken securityToken;                

            return tokenHandler.ValidateToken(token, parameters, out securityToken);
        }

    }
}