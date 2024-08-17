using LabXand.Extensions;
using System.Linq.Expressions;
using System.Reflection;

namespace LabXand.Core;

public static class QueryableExteder
{
    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, List<SortItem> sortItems)
    {
        for (var i = 0; i < sortItems.Count; i++)
            source = CreateSortedQuery(source, sortItems[i], i == 0);
        return source;
    }

    private static IQueryable<TSource> CreateSortedQuery<TSource>(IQueryable<TSource> sources, SortItem sortItem, bool isFirst)
    {
        string prefix = string.Format("{0}By", isFirst ? "Order" : "Then");
        string command = string.Format("{0}{1}", prefix, sortItem.Direction == SortDirection.Descending ? "Descending" : "");
        var type = typeof(TSource);
        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = ExpressionHelper.GetMemberExpression<TSource>(parameter, sortItem.SortFiledsSelector);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var method = GetMethod(command)
            .MakeGenericMethod(typeof(TSource), propertyAccess.Type);
        var resultExpression = Expression.Call(method,
                                      sources.Expression,
                                      orderByExpression);
        return sources.Provider.CreateQuery<TSource>(resultExpression);
    }

    static MethodInfo GetMethod(string command)
      => typeof(Queryable).GetMethods()
        .Where(m => m.Name.Equals(command) && m.GetParameters().Length == 2)
        .Single();

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;
}
