﻿using LabXand.SharedKernel;
using LabXand.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Text;

namespace LabXand.Data.EF;
public class EFRepositoryBase<TAggregateRoot, TIdentifier>(DbContext dbContext) :
    IRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
{
    public EFRepositoryBase(DbContext dbContext, IServiceProvider serviceProvider) : this(dbContext)
    {
        this.serviceProvider = serviceProvider;
    }
    protected IQueryable<TAggregateRoot>? trackingQuery;
    protected IQueryable<TAggregateRoot>? noTrackingQuery;
    protected List<IRestriction<TAggregateRoot, TIdentifier>> registeredRestrictions = [];
    protected List<IRestriction<TAggregateRoot, TIdentifier>> currentRestrictions = [];
    protected bool restrictionIsInit = false;
    protected readonly DbContext dbContext = dbContext;
    private readonly IServiceProvider? serviceProvider;

    internal INavigationPropertyUpdater<TAggregateRoot> NavigationPropertyUpdater { get; set; }

    public IQueryable<TAggregateRoot> Query => GetQuery();

    public IQueryable<TAggregateRoot> GetQuery(bool trackedQuery = false)
    {
        InitRestrictions();

        if (trackedQuery)
        {
            trackingQuery ??= dbContext.Set<TAggregateRoot>();
            return trackingQuery.ApplyRestriction(currentRestrictions);
        }
        noTrackingQuery ??= dbContext.Set<TAggregateRoot>()
            .ApplyRestriction(currentRestrictions)
            .AsNoTracking()
            .AsSingleQuery();
        return noTrackingQuery;
    }

    private void InitRestrictions()
    {
        if (restrictionIsInit)
            return;

        if (serviceProvider is null)
            registeredRestrictions = [];
        else
            registeredRestrictions = serviceProvider.GetServices<IRestriction<TAggregateRoot, TIdentifier>>().ToList();
        currentRestrictions = registeredRestrictions;
        restrictionIsInit = true;
    }

    protected void ApplyRestriction(ApplyRestrictionTypes applyRestrictionTypes, List<IRestriction<TAggregateRoot, TIdentifier>>? restrictions = null)
    {
        InitRestrictions();
        switch (applyRestrictionTypes)
        {
            case ApplyRestrictionTypes.None:
                currentRestrictions = [];
                break;
            case ApplyRestrictionTypes.All:
                currentRestrictions = registeredRestrictions;
                break;
            case ApplyRestrictionTypes.Custom:
                currentRestrictions = restrictions!;
                break;
        };
    }
    public virtual void Add(TAggregateRoot domain)
    {
        CheckRestriction(domain, RestrictionTypes.OnAdd);
        dbContext.Set<TAggregateRoot>().Add(domain);
    }
    public virtual void Edit(TAggregateRoot domain)
    {
        CheckRestriction(domain, RestrictionTypes.OnEdit);

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
        MapDomainEvents(domain, original!);
        NavigationPropertyUpdater.Update(dbContext, domain, original!);
    }

    private void MapDomainEvents(TAggregateRoot source, TAggregateRoot destination)
    {
        source.DomainEvents.ToList().ForEach(destination!.AddDomainEvent);
    }

    public virtual async Task RemoveAsync(TIdentifier id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<TAggregateRoot>().FindAsync(id, cancellationToken);
        if (entity is null)
            ExceptionManager.EntityNotFound<TAggregateRoot, TIdentifier>(id);

        CheckRestriction(entity, RestrictionTypes.OnDelete);
        dbContext.Set<TAggregateRoot>().Remove(entity);
    }

    protected List<IRestriction<TAggregateRoot, TIdentifier>> GetRestrictions(RestrictionTypes restrictionTypes)
        => currentRestrictions.Where(r => r.Type.HasFlag(restrictionTypes)).ToList();

    void CheckRestriction(TAggregateRoot entity, RestrictionTypes restrictionType)
    {
        var restrictions = GetRestrictions(restrictionType);
        List<IRestriction<TAggregateRoot, TIdentifier>> violatedRestrictions = [];
        restrictions.ForEach(restriction =>
        {
            if (!restriction.Specification.IsSatisfiedBy(entity))
                violatedRestrictions.Add(restriction);
        });
        if (violatedRestrictions.Count > 0)
        {
            StringBuilder stringBuilder = new();
            violatedRestrictions.ForEach(r => stringBuilder.AppendLine(r.GetMessage(entity)));
            throw new ViolatedRestrictionException(stringBuilder.ToString(), entity, violatedRestrictions.Cast<IRestriction>().ToList());
        }
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