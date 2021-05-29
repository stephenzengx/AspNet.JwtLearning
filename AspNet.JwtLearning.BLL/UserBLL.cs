﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        public IDAL<tb_user> userDAL;

        public UserBLL(IDAL<tb_user> userDAL)
        {
            this.userDAL = userDAL;
        }

        #region 基础方法
        public int Count(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return userDAL.Count(wherePredicate);
        }

        public bool Add(tb_user entity)
        {
            return userDAL.Add(entity);
        }

        public bool Update(tb_user entity)
        {
            return userDAL.Update(entity);
        }

        public bool Delete(int id)
        {
            return userDAL.Delete(id);
        }

        public tb_user FirstOrDefault(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return userDAL.FirstOrDefault(wherePredicate);
        }

        public List<tb_user> GetList(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return userDAL.GetList(wherePredicate);
        }


        public List<tb_user> GetListByPage<TOrderField>(int pageIndex, int pageSize, Expression<Func<tb_user, bool>> wherePredicate, Expression<Func<tb_user, TOrderField>> orderPredicate,out int totalCount, SortOrder sortOrder = SortOrder.Ascending)
        {
            return userDAL.GetListByPage(pageIndex, pageSize, wherePredicate, orderPredicate,out totalCount, sortOrder);
        }
        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResponseResult Login(LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.PassWord))
            {
                return ResultHelper.GetOkResponse(null,"用户名或密码错误",-1);
            }

            var checkUserName = RSAHelper.Decrypt(model.UserName);
            var checkPassWord = RSAHelper.Decrypt(model.PassWord);

            //1非对称解密前端. 2-加密密码保存 3下次请求api资源，前端直接通过token 跟redis里面token对比
            tb_user findUser = userDAL.FirstOrDefault(m => m.userName == checkUserName && m.isEnable);

            if (findUser == null)
                return ResultHelper.GetOkResponse(null, "用户名或密码错误", -1);

            var rightPassWord = RSAHelper.Decrypt(findUser.passWord);
            if (!rightPassWord.Equals(checkPassWord))
                return ResultHelper.GetOkResponse(null, "用户名或密码错误", -1);

            JwtContainerModel jwtModel = new JwtContainerModel
            {
                UserId = findUser.userId,
                TimeStamp = Utils.GetTimeStamp() 
            };

            return ResultHelper.GetOkResponse(JWTService.GenerateToken(jwtModel));
        }

        /// <summary>
        /// 用户注册(管理员手动创建用户)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ResponseResult Register(tb_user user)
        {
            if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.passWord))
            {
                return ResultHelper.GetErrorResponse("请输入用户名和密码");
            }

            var realUserName = RSAHelper.Decrypt(user.userName);
            if (userDAL.Any(m => m.userName == realUserName))
                return ResultHelper.GetErrorResponse("账户已存在");

            user.userName = realUserName;
            user.addTime = DateTime.Now;
            if (!userDAL.Add(user))
                return ResultHelper.GetErrorResponse("系统异常,请稍后重试");
                
            return ResultHelper.GetOkResponse("创建用户成功");
        }
    }
}
