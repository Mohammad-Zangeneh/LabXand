using LabXand.SharedKernel;
using LabXand.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class ViolatedRestrictionException(object entity, List<IRestriction> restrictions) : Exception
{
    public List<IRestriction> Restrictions { get; } = restrictions;
}
public class EFRepositoryBase<TAggregateRoot, TIdentifier>(DbContext dbContext) : IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    protected IQueryable<TAggregateRoot>? trackingQuery;
    protected IQueryable<TAggregateRoot>? noTrackingQuery;
    protected List<IRestriction<TAggregateRoot, TIdentifier>> restrictions = [];
    protected readonly DbContext dbContext = dbContext;
    internal INavigationPropertyUpdater<TAggregateRoot> NavigationPropertyUpdater { get; set; }

    public IQueryable<TAggregateRoot> Query => GetQuery();

    public List<IRestriction<TAggregateRoot, TIdentifier>> Restrictions => restrictions;

    public IQueryable<TAggregateRoot> GetQuery(bool trackedQuery = false)
    {
        if (trackedQuery)
        {
            trackingQuery ??= dbContext.Set<TAggregateRoot>();
            return ApplyRestriction(trackingQuery);
        }
        noTrackingQuery ??= ApplyRestriction(dbContext.Set<TAggregateRoot>())
            .AsNoTracking()
            .AsSingleQuery();
        return noTrackingQuery;
    }
    IQueryable<TAggregateRoot> ApplyRestriction(IQueryable<TAggregateRoot> query)
    {
        restrictions.ForEach(restriction => query = query.Where(restriction.Specification.Criteria).TagWith(restriction.Title));
        return query;
    }
    public virtual void Add(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Add(domain);
    public virtual void Edit(TAggregateRoot domain)
    {
        if (NavigationPropertyUpdater is null)
        {
            dbContext.Attach(domain);
            dbContext.Entry(domain).State = EntityState.Modified;
            return;
        }

        var query = dbContext.Set<TAggregateRoot>().AsQueryable();
        NavigationPropertyUpdater.GetIncludePath().ForEach(p => query = query.Include(p));
        var original = query.FirstOrDefault(d => d.Id.Equals(domain.Id));
        if (original is null)
            ExceptionManager.EntityNotFound<TAggregateRoot, TIdentifier>(domain.Id);
        NavigationPropertyUpdater.Update(dbContext, domain, original);

    }
    public virtual async Task RemoveAsync(TIdentifier id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<TAggregateRoot>().FindAsync(id, cancellationToken);
        if (entity is null)
            ExceptionManager.EntityNotFound<TAggregateRoot, TIdentifier>(id);
        dbContext.Set<TAggregateRoot>().Remove(entity);
    }

    void CheckRestriction(TAggregateRoot entity, List<IRestriction<TAggregateRoot, TIdentifier>> restrictions)
    {
        List<IRestriction> violatedRestrictions = [];
        restrictions.ForEach(restriction =>
        {
            if (!restriction.Specification.IsSatisfiedBy(entity))
                violatedRestrictions.Add(restriction);
        });
        if (violatedRestrictions.Count > 0)
            throw new ViolatedRestrictionException(violatedRestrictions);
    }

    public Task<int> CountAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken)
        => query.CountAsync(expression, cancellationToken);

    public Task<TAggregateRoot?> GetAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken)
        => query.Where(expression).FirstOrDefaultAsync(cancellationToken);

    public Task<List<TAggregateRoot>> GetPaginatedItemsAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, int page, int size, CancellationToken cancellationToken)
        => query.Where(expression).Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);

    public Task<List<TAggregateRoot>> GetListAsync(IQueryable<TAggregateRoot> query, Expression<Func<TAggregateRoot, bool>> expression, CancellationToken cancellationToken)
        => query.Where(expression).ToListAsync(cancellationToken);

    public Task<TResult?> GetAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken)
        where TResult : class
        => query.FirstOrDefaultAsync(cancellationToken);

    public Task<List<TResult>> GetPaginatedItemsAsync<TResult>(IQueryable<TResult> query, int page, int size, CancellationToken cancellationToken)
        where TResult : class
        => query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);

    public Task<List<TResult>> GetListAsync<TResult>(IQueryable<TResult> query, CancellationToken cancellationToken)
        where TResult : class
        => query.ToListAsync(cancellationToken);

    public async Task<TAggregateRoot?> GetByIdAsync(TIdentifier identifier, CancellationToken cancellationToken) =>
        await GetByIdAsync<TAggregateRoot, TIdentifier>(identifier, cancellationToken);

    public async Task<TResult?> GetByIdAsync<TResult, TId>(TId identifier, CancellationToken cancellationToken)
        where TResult : EntityBase<TId>
        where TId : struct
        => await dbContext.Set<TResult>().AsNoTracking().FirstOrDefaultAsync(t => t.Id.Equals(identifier), cancellationToken: cancellationToken);

    protected INavigationPropertyUpdater<TAggregateRoot> HasNavigation(INavigationPropertyUpdaterCustomizer<TAggregateRoot>? propertyUpdaterCustomizer = null)
    {
        if (NavigationPropertyUpdater is not null)
            return NavigationPropertyUpdater;
        var rootUpdater = new RootUpdater<TAggregateRoot>(propertyUpdaterCustomizer!);
        NavigationPropertyUpdater = rootUpdater;
        return rootUpdater;
    }
}