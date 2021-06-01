using System;
using System.Linq.Expressions;

namespace AspNet.JwtLearning.Utility.LinqExtension
{
    public class Where<T>
    {
        private Expression<Func<T, bool>> _expressions = e => true;

        public void And(Expression<Func<T, bool>> exp)
        {
            _expressions = _expressions.AndAlso(exp, Expression.AndAlso);
        }

        public void Or(Expression<Func<T, bool>> exp)
        {
            _expressions = _expressions.AndAlso(exp, Expression.OrElse);
        }

        public Expression<Func<T, bool>> GetExpression()
        {
            return _expressions;
        }
    }
}
