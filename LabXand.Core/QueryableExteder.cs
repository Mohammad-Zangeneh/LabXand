using LabXand.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core;

public static class QueryableExteder
{
    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, List<SortItem> sortItems)
    {
        sortItems.ForEach(item => source = CreateSortedQuery(source, item));
        return source;
    }

    private static IQueryable<TSource> CreateSortedQuery<TSource>(IQueryable<TSource> sources, SortItem sortItem)
    {
        bool isOrderableQuery = sources is IOrderedQueryable<TSource>;

        string prefix = string.Format("{0}By", isOrderableQuery ? "Then" : "Order");
        string command = string.Format("{0}{1}", prefix, sortItem.Direction == SortDirection.Descending ? "Descending" : "");
        var type = typeof(TSource);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = ExpressionHelper.GetMemberExpression<TSource>(parameter, sortItem.SortFiledsSelector);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, ((PropertyInfo)propertyAccess.Member).PropertyType },
                                      sources.Expression, Expression.Quote(orderByExpression));
        return sources.Provider.CreateQuery<TSource>(resultExpression);
    }
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;
}
