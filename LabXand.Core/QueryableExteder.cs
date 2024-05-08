using LabXand.Extensions;
using System.Linq.Expressions;

namespace LabXand.Core;

public static class QueryableExteder
{
    public static IQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, List<SortItem> sortItems)
    {
        sortItems.ForEach(s =>
            {
                if (source is IOrderedQueryable<TSource>)
                {
                    source = s.Direction == SortDirection.Descending ?
                        ((IOrderedQueryable<TSource>)source).ThenByDescending(ExpressionHelper.CreatePropertySelector<TSource>(s.SortFiledsSelector)) :
                        ((IOrderedQueryable<TSource>)source).ThenBy(ExpressionHelper.CreatePropertySelector<TSource>(s.SortFiledsSelector));
                }
                else
                {
                    source = s.Direction == SortDirection.Descending ?
                        source.OrderByDescending(ExpressionHelper.CreatePropertySelector<TSource>(s.SortFiledsSelector)) :
                        source.OrderBy(ExpressionHelper.CreatePropertySelector<TSource>(s.SortFiledsSelector));
                }
            });
        return source;
    }
    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
        => condition ? source.Where(predicate) : source;

    public static IEnumerable<TSource> WhereIf<TSource>(this IEnumerable<TSource> source, bool condition, Func<TSource, bool> predicate)
        => condition ? source.Where(predicate) : source;
}
