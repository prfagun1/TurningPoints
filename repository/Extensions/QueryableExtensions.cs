using entities;
using System.Linq.Expressions;

namespace repository.Extensions
{
    public static class QueryableExtensions
    {
        public static IOrderedQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string memberPath, Enumerators.SortOrder sortOrder)
        {
            var method = sortOrder == Enumerators.SortOrder.Ascending ? "OrderBy" : "OrderByDescending";

            var parameter = Expression.Parameter(typeof(T), "item");
            var member = memberPath.Split('.').Aggregate((Expression)parameter, Expression.PropertyOrField);
            var keySelector = Expression.Lambda(member, parameter);
            var methodCall = Expression.Call(typeof(Queryable), method, new[] { parameter.Type, member.Type }, source.Expression, Expression.Quote(keySelector));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery(methodCall);
        }


    }
}
