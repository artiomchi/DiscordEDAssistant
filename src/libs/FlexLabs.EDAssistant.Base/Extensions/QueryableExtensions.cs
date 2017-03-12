using System;
using System.Linq;
using System.Linq.Expressions;

namespace FlexLabs.EDAssistant.Base.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return source.Where(predicate);
            return source;
        }
    }
}
