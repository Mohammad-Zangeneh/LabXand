using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplyRestriction<T, TIdentifier>(this IQueryable<T> query, List<IRestriction<T, TIdentifier>> restrictions)
    where T : IEntity<TIdentifier>
    where TIdentifier : struct
    {
        restrictions
            .ForEach(restriction => query = query.Where(restriction.Specification.Criteria)
            .TagWith(restriction.Title));
        return query;
    }
}
