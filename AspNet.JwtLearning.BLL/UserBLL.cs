using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using AspNet.JwtLearning.DAL;
using AspNet.JwtLearning.Models;
using AspNet.JwtLearning.Models.AdminEntity;
using AspNet.JwtLearning.Utility;
using AspNet.JwtLearning.Utility.BaseHelper;
using AspNet.JwtLearning.Utility.Common;
using AspNet.JwtLearning.Utility.TokenHandle;

namespace AspNet.JwtLearning.BLL
{
    public class UserBLL
    {
        public IDAL<tb_tenant_user> userDAL;

        public UserBLL(IDAL<tb_tenant_user> userDAL)
        {
            this.userDAL = userDAL;
        }

        #region 基础方法
        public bool Add(tb_tenant_user entity)
        {
            return userDAL.Add(entity);
        }

        public bool Update(tb_tenant_user entity)
        {
            return userDAL.Update(entity);
        }

        public bool Delete(int id)
        {
            return userDAL.Delete(id);
        }

        public tb_tenant_user FirstOrDefault(Expression<Func<tb_tenant_user, bool>> whereExpression)
        {
            return userDAL.FirstOrDefault(whereExpression);
        }

        public List<tb_tenant_user> GetList(Expression<Func<tb_tenant_user, bool>> whereExpression)
        {
            return userDAL.GetList(whereExpression);
        }

        public List<tb_tenant_user> GetListByPage(int pageIndex, int pageSize, Expression<Func<tb_tenant_user, bool>> whereExpression, Expression<Func<tb_tenant_user, bool>> orderExpression)
        {
            return userDAL.GetListByPage(pageIndex, pageSize, whereExpression, orderExpression);
        }
        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseResult Login(LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
            {
                return ResponseHelper.GetErrorResponse("用户名或密码错误");
            }

            var checkUserName = RSAHelper.Decrypt(model.UserName);
            var checkPassWord = RSAHelper.Decrypt(model.Password);

            //1非对称解密前端. 2-加密密码保存 3下次请求api资源，前端直接通过token 跟redis里面token对比
            tb_tenant_user findUser = userDAL.FirstOrDefault(m => m.userName == checkUserName);

            if (findUser == null)
                return ResponseHelper.GetErrorResponse("用户名或者密码错误");

            var rightPassWord = RSAHelper.Decrypt(findUser.passWord);
            if (!rightPassWord.Equals(checkPassWord))
                return ResponseHelper.GetErrorResponse("用户名或者密码错误");

            JwtContainerModel jwtModel = new JwtContainerModel
            {
                UserId = findUser.userId,
                TimeStamp = Utils.GetTimeStamp() 
            };

            return ResponseHelper.OkResponse(JWTService.GenerateToken(jwtModel));
        }

        /// <summary>
        /// 用户注册(管理员手动创建用户)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResponseResult Register(tb_tenant_user user)
        {
            if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.passWord))
            {
                return ResponseHelper.GetErrorResponse("请输入用户名和密码");
            }

            var realUserName = RSAHelper.Decrypt(user.userName);
            if (userDAL.Any(m => m.userName == realUserName))
                return ResponseHelper.GetErrorResponse("账户已存在");

            user.userName = realUserName;
            user.addTime = DateTime.Now;
            if (userDAL.Add(user))
                return ResponseHelper.GetErrorResponse("系统异常,请稍后重试");
                
            return ResponseHelper.OkResponse("创建用户成功");
        }
    }
}
