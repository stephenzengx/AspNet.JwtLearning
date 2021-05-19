using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AspNet.JwtLearning.DAL
{
    public interface IDAL<T> where T :class
    {
        List<T> GetList();

        List<T> GetList(Expression<Func<T,bool>> whereExpression);

        List<T> GetListByPage(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, bool>> orderExpression);

        bool Add(T entity);

        bool Update(T entity);

        bool Delete(int Id);
    }
}
