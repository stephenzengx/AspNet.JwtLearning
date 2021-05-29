using AspNet.JwtLearning.Models.AdminEntity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace AspNet.JwtLearning.DAL
{
    public class TenantUserDAL : IDAL<tb_user>
    {
        public AuthDbContext authDbContext;

        public TenantUserDAL(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public tb_user FirstOrDefault(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return authDbContext.tb_users.FirstOrDefault(wherePredicate);
        }

        public bool Any(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return authDbContext.tb_users.Any(wherePredicate);
        }

        public int Count(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return authDbContext.tb_users.Count(wherePredicate);

            //to do
            if (wherePredicate == null)
                return authDbContext.tb_users.Count();           
        }

        public bool Add(tb_user entity)
        {
            authDbContext.tb_users.Add(entity);
            return authDbContext.SaveChanges() > 0;
        }

        public bool Update(tb_user entity)
        {
            var findUser = FirstOrDefault(m=>m.userId == entity.userId);
            if (findUser == null)
                throw new KeyNotFoundException("userId not found");
            //authDbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            findUser.userName = entity.userName;
            findUser.age = entity.age;
            findUser.phone = entity.phone;
            findUser.email = entity.email;
            findUser.userName = entity.userName;

            return authDbContext.SaveChanges() > 0;
        }

        public bool Delete(int Id)
        {
            //方法1
            var model = FirstOrDefault(m=>m.userId==Id);
            authDbContext.tb_users.Remove(model);

            //方法2
            //var model = new tb_tenant_user { userId = Id };
            //authDbContext.tb_tenant_users.Attach(model);
            //authDbContext.tb_tenant_users.Remove(model);
            
            return authDbContext.SaveChanges() > 0;
        }

        public List<tb_user> GetList(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return authDbContext.tb_users.Where(wherePredicate).ToList();
        }

        //OrderSort排序字段， SortOrder 字段排序顺序
        public List<tb_user> GetListByPage<TOrderFiled>(int pageIndex, int pageSize, Expression<Func<tb_user, bool>> wherePredicate, Expression<Func<tb_user, TOrderFiled>> orderPredicate, out int totalCount, SortOrder sortOrder = SortOrder.Ascending)
        {
            // to do wherePredicate 判空，orderPredicate  判空

            if (pageIndex <= 0)
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, "The pageIndex is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            var query = authDbContext.tb_users.Where(wherePredicate);
            int skip = (pageIndex - 1) * pageSize;
            int take = pageSize;

            totalCount = authDbContext.tb_users.Count(wherePredicate);           

            if(sortOrder == SortOrder.Ascending)
                return query.OrderBy(orderPredicate).Skip(skip).Take(take).ToList();

            //return authDbContext.tb_tenant_users.Where(wherePredicate)
            //            .OrderBy(orderPredicate)
            //            .Skip((pageIndex - 1) * pageSize).Take(pageSize)
            //            .ToList();

            return query.OrderByDescending(orderPredicate).Skip(skip).Take(take).ToList();
        }
    }
}
