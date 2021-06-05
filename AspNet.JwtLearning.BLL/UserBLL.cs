using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
        private readonly IDAL<tb_user> userDAL;

        public UserBLL(IDAL<tb_user> userDAL)
        {
            this.userDAL = userDAL;
        }

        #region 基础方法
        public async Task<int> CountAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await userDAL.CountAsync(wherePredicate);
        }

        public async Task<bool> AddAsync(tb_user entity)
        {
            return await userDAL.AddAsync(entity);
        }

        public async Task<bool> UpdateAsync(tb_user entity)
        {
            return await userDAL.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await userDAL.DeleteAsync(id);
        }

        public async Task<tb_user> FirstOrDefaultAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await userDAL.FirstOrDefaultAsync(wherePredicate);
        }

        public async Task<List<tb_user>> GetListAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await userDAL.GetListAsync(wherePredicate);
        }

        public async Task<Tuple<int, IEnumerable<tb_user>>> GetListByPage<TOrderField>(int pageIndex, int pageSize, Expression<Func<tb_user, bool>> wherePredicate, Expression<Func<tb_user, TOrderField>> orderPredicate, SortOrder sortOrder = SortOrder.Ascending)
        {
            return await userDAL.GetListByPage(pageIndex, pageSize, wherePredicate, orderPredicate,sortOrder);
        }
        #endregion

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Login(LoginModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.PassWord))
            {
                return ResponseHelper.GetOkResponse(null,"用户名或密码错误",-1);
            }

            var checkUserName = RSAHelper.Decrypt(model.UserName);
            var checkPassWord = RSAHelper.Decrypt(model.PassWord);

            //1非对称解密前端. 2-加密密码保存 3下次请求api资源，前端直接通过token 跟redis里面token对比
            tb_user findUser = await userDAL.FirstOrDefaultAsync(m => m.userName == checkUserName && m.isEnable);

            if (findUser == null)
                return ResponseHelper.GetOkResponse(null, "用户名或密码错误", -1);

            var rightPassWord = RSAHelper.Decrypt(findUser.passWord);
            if (!rightPassWord.Equals(checkPassWord))
                return ResponseHelper.GetOkResponse(null, "用户名或密码错误", -1);

            JwtContainerModel jwtModel = new JwtContainerModel
            {
                UserId = findUser.userId,
                TimeStamp = Utils.GetTimeStamp() 
            };

            return ResponseHelper.GetOkResponse(JWTService.GenerateToken(jwtModel));
        }

        /// <summary>
        /// 用户注册(管理员手动创建用户)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ResponseResult> Register(tb_user user)
        {
            if (user == null || string.IsNullOrEmpty(user.userName) || string.IsNullOrEmpty(user.passWord))
                return ResponseHelper.GetErrorResponse("请输入用户名和密码");

            var realUserName = RSAHelper.Decrypt(user.userName);
            if (await userDAL.AnyAsync(m => m.userName == realUserName))
                return ResponseHelper.GetErrorResponse("账户已存在");

            user.userName = realUserName;
            user.addTime = DateTime.Now;
            if (! await userDAL.AddAsync(user))
                return ResponseHelper.GetErrorResponse("系统异常,请稍后重试");
                
            return ResponseHelper.GetOkResponse("创建用户成功");
        }

        /// <summary>
        /// 用户菜单按钮权限
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<ResponseResult> GetMenuBtnRight(int userId,int menuId)
        {
            var ret = new List<MenuBtnInfoClass>();
            var systemBtns = await RedisBLL.GetSysMenuButtons();
            if (systemBtns.Count <= 0)
                return ResponseHelper.GetOkResponse(ret);

            var roleId = await RedisBLL.GetRoleId(userId);
            var roleBtns = (await RedisBLL.GetRoleMenuButtons()).Where(m => m.roleId == roleId && m.menuId == menuId).ToList();
            if (roleBtns.Count<=0)
                return ResponseHelper.GetOkResponse(ret);

            var btnIdList = roleBtns.Select(m => m.btnId).ToList();
            foreach (var item in systemBtns.Where(m=>btnIdList.Contains(m.btnId)))
            {
                ret.Add(new MenuBtnInfoClass { 
                    btnId = item.btnId,
                    btnTxt = item.btnTxt,
                    remark = item.remark
                });
            }

            return ResponseHelper.GetOkResponse(ret);
        }
    }
}
