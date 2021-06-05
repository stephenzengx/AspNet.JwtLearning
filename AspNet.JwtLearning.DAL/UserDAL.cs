using AspNet.JwtLearning.Models.AdminEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.JwtLearning.DAL
{
    public class UserDAL : IDAL<tb_user>
    {
        private readonly AuthDbContext authDbContext;

        public UserDAL(AuthDbContext authDbContext)
        {
            this.authDbContext = authDbContext;
        }

        public async Task<tb_user> FirstOrDefaultAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await authDbContext.tb_users.FirstOrDefaultAsync(wherePredicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await authDbContext.tb_users.AnyAsync(wherePredicate);
        }

        public async Task<int> CountAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return wherePredicate!=null ? await authDbContext.tb_users.CountAsync(wherePredicate):await authDbContext.tb_users.CountAsync();      
        }

        public async Task<bool> AddAsync(tb_user entity)
        {
            authDbContext.tb_users.Add(entity);
            return await authDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(tb_user entity)
        {
            var findUser = await FirstOrDefaultAsync(m=>m.userId == entity.userId);
            if (findUser == null)
                throw new KeyNotFoundException("userId not found");
            //authDbContext.Entry(entity).State = System.Data.Entity.EntityState.Modified;
            findUser.userName = entity.userName;
            findUser.age = entity.age;
            findUser.phone = entity.phone;
            findUser.email = entity.email;
            findUser.userName = entity.userName;

            return await authDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            //方法1
            var model = await FirstOrDefaultAsync(m=>m.userId==Id);
            authDbContext.tb_users.Remove(model);

            //方法2
            //var model = new tb_tenant_user { userId = Id };
            //authDbContext.tb_tenant_users.Attach(model);
            //authDbContext.tb_tenant_users.Remove(model);
            
            return await authDbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<tb_user>> GetListAsync(Expression<Func<tb_user, bool>> wherePredicate)
        {
            return await authDbContext.tb_users.Where(wherePredicate).ToListAsync();
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="TOrderFiled"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="wherePredicate"></param>
        /// <param name="orderPredicate"></param>
        /// <param name="totalCount"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public async Task<Tuple<int, IEnumerable<tb_user>>> GetListByPage<TOrderFiled>(int pageIndex, int pageSize, Expression<Func<tb_user, bool>> wherePredicate, Expression<Func<tb_user, TOrderFiled>> orderPredicate, SortOrder sortOrder = SortOrder.Ascending)
        {
            if (pageIndex <= 0)
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, "The pageIndex is one-based and should be larger than zero.");
            if (pageSize <= 0)
                throw new ArgumentOutOfRangeException("pageSize", pageSize, "The pageSize is one-based and should be larger than zero.");

            var query = authDbContext.tb_users.Where(wherePredicate);
            int skip = (pageIndex - 1) * pageSize;
            int take = pageSize;

            var totalCount = await query.CountAsync(wherePredicate);
            //totalCount = authDbContext.tb_users.Count(wherePredicate);

            query = (sortOrder == SortOrder.Ascending) ? query.OrderBy(orderPredicate) : query.OrderByDescending(orderPredicate);
            return new Tuple<int, IEnumerable<tb_user>>(totalCount, await query.Skip(skip).Take(take).ToListAsync());
            //return (sortOrder == SortOrder.Ascending) ? query.OrderBy(orderPredicate).Skip(skip).Take(take).ToList() : query.OrderByDescending(orderPredicate).Skip(skip).Take(take).ToList();
        }
    }
}
