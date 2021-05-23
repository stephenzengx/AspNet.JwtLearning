using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace AspNet.JwtLearning.DAL
{
    public interface IDAL<T> where T :class
    {

        T FirstOrDefault(Expression<Func<T, bool>> wherePredicate);

        bool Any(Expression<Func<T, bool>> wherePredicate);

        int Count(Expression<Func<T, bool>> wherePredicate);

        bool Add(T entity);

        bool Update(T entity);

        bool Delete(int Id);

        List<T> GetList(Expression<Func<T, bool>> wherePredicate);

        //默认升序
        List<T> GetListByPage<TOrderFiled>(int pageIndex, int pageSize, Expression<Func<T, bool>> wherePredicate, Expression<Func<T, TOrderFiled>> orderExpression, out int totalCount, SortOrder sortOrder);
    }
}
