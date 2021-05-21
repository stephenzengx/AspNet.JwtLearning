using AspNet.JwtLearning.Models.AdminEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AspNet.JwtLearning.DAL
{
    public class TenantUserDAL : IDAL<tb_tenant_user>
    {
        public AuthDbContext authDbContext;

        public TenantUserDAL(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public tb_tenant_user FirstOrDefault(Expression<Func<tb_tenant_user, bool>> whereExpression)
        {
            return authDbContext.tb_tenant_users.FirstOrDefault(whereExpression);
        }

        public bool Any(Expression<Func<tb_tenant_user, bool>> whereExpression)
        {
            return authDbContext.tb_tenant_users.Any(whereExpression);
        }

        public bool Add(tb_tenant_user entity)
        {
            authDbContext.tb_tenant_users.Add(entity);
            return authDbContext.SaveChanges() > 0;
        }

        public bool Update(tb_tenant_user entity)
        {
            authDbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;

            return authDbContext.SaveChanges() > 0;
        }

        public bool Delete(int Id)
        {
            //方法1
            var model = FirstOrDefault(m=>m.userId==Id);
            authDbContext.tb_tenant_users.Remove(model);

            //方法2
            //var model = new tb_tenant_user { userId = Id };
            //authDbContext.tb_tenant_users.Attach(model);
            //authDbContext.tb_tenant_users.Remove(model);
            
            return authDbContext.SaveChanges() > 0;
        }

        public List<tb_tenant_user> GetList(Expression<Func<tb_tenant_user, bool>> whereExpression)
        {
            return authDbContext.tb_tenant_users.Where(whereExpression).ToList();
        }

        public List<tb_tenant_user> GetListByPage(int pageIndex, int pageSize, Expression<Func<tb_tenant_user, bool>> whereExpression, Expression<Func<tb_tenant_user, bool>> orderExpression)
        {
            return authDbContext.tb_tenant_users.Where(whereExpression)
                   .Skip((pageIndex-1)*pageSize).Take(pageSize)
                   .OrderBy(orderExpression).ToList();
        }
    }
}
