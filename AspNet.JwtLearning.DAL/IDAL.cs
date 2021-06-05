using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNet.JwtLearning.DAL
{
    public interface IDAL<T> where T :class
    {

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> wherePredicate);

        Task<bool> AnyAsync(Expression<Func<T, bool>> wherePredicate);

        Task<int> CountAsync(Expression<Func<T, bool>> wherePredicate);

        Task<bool> AddAsync(T entity);

        Task<bool> UpdateAsync(T entity);

        Task<bool> DeleteAsync(int Id);

        Task<List<T>> GetListAsync(Expression<Func<T, bool>> wherePredicate);

        //默认升序
        List<T> GetListByPage<TOrderFiled>(int pageIndex, int pageSize, Expression<Func<T, bool>> wherePredicate, Expression<Func<T, TOrderFiled>> orderExpression, out int totalCount, SortOrder sortOrder);
    }
}
