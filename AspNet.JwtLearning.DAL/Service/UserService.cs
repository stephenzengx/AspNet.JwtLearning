using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using System.Net.Http;

namespace AspNet.JwtLearning.Service
{
    public class UserService
    {
        //这里需要变动一下 to do...
        public static HttpResponseMessage UserRegister(tb_tenant_user user)
        {
            return new HttpResponseMessage();

            //if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.passWord))
            //{
            //    return ApiHelper.GetErrorResponse("请输入用户名和密码");
            //}

            //var realUserName = RSAHelper_v2.Decrypt(user.userName);            
            //int ret = 0;
            //using (AuthDbContext db = new AuthDbContext())
            //{
            //    if (db.tb_tenant_users.Any(m => m.userName == realUserName))
            //    {
            //        return ApiHelper.GetErrorResponse("账户已存在");
            //    }
            //    user.userName = realUserName;
            //    user.addTime = DateTime.Now;

            //    db.tb_tenant_users.Add(user);
            //    ret = db.SaveChanges();
            //}

            //if (ret <= 0)
            //    return ApiHelper.GetErrorResponse("系统异常,请稍后重试");

            //return ApiHelper.GetMsgResponse("注册成功");
        }
        
        //to do

        public static HttpResponseMessage UserLogin(LoginModel user)
        {
            return new HttpResponseMessage();
            //var checkUserName = string.Empty;
            //var checkPassWord = string.Empty;
            //var rightPassWord = string.Empty;
            //if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            //{
            //    return ApiHelper.GetErrorResponse("用户名或密码错误");
            //}

            //try
            //{
            //    checkUserName = RSAHelper_v2.Decrypt(user.UserName);
            //    checkPassWord = RSAHelper_v2.Decrypt(user.Password);
            //}
            //catch (Exception e)
            //{
            //    LogHelper.WriteLog("登录数据异常,"+e.Message);
            //    throw e;
            //}          

            ////1非对称解密前端. 2-加密密码保存 3下次请求api资源，前端直接通过token 跟redis里面token对比
            //tb_tenant_user findUser = null;
            //using (DbAuthTest db = new DbAuthTest())
            //{
            //    findUser = db.tb_tenant_users.FirstOrDefault(m => m.userName == checkUserName);
            //}
            //if (findUser == null)
            //    return ApiHelper.GetErrorResponse("用户名或者密码错误");

            //rightPassWord = RSAHelper_v2.Decrypt(findUser.passWord);
            //if (!rightPassWord.Equals(checkPassWord))
            //    return ApiHelper.GetErrorResponse("用户名或者密码错误");

            //return ApiHelper.GetJsonResponse(LoginAction(findUser));
        }

        //to do
        private static string LoginAction(tb_tenant_user findUser)
        {
            return string.Empty;
            
            //var model = new JwtContainerModel
            //{
            //    UserId = findUser.userId,
            //    UserName = findUser.userName,
            //    Guid = Guid.NewGuid().ToString().Substring(0, 16)//取前16位
            //};
            //var token = JWTService.GenerateToken(model);
            
            // 暂时注释代码 to do
            //var refToken = JWTService.GenerateToken(model, JWTService._expireMinRefreshToken);
            //UserTokenManger.SetUserToken_v1(findUser.UserId+"_"+model.Guid, token);
            //UserTokenManger.SetRefreshToken(findUser.userid + "");

            //return token;
        }
    }
}