using LabXand.SharedKernel;
using LabXand.SharedKernel.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabXand.Data.EF;

public class EFRepositoryBase<TAggregateRoot, TIdentifier>(DbContext dbContext) : IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
    where TIdentifier : struct
{
    protected readonly DbContext dbContext = dbContext;

    public IQueryable<TAggregateRoot> Query => dbContext.Set<TAggregateRoot>().AsNoTracking();
    public virtual void Add(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Add(domain);
    public virtual void Edit(TAggregateRoot domain)
    {
        dbContext.Attach(domain);
        dbContext.Entry(domain).State = EntityState.Modified;
    }
    public virtual async Task RemoveAsync(TIdentifier id, CancellationToken cancellationToken)
    {
        var entity = await dbContext.Set<TAggregateRoot>().FindAsync(id, cancellationToken);
        if (entity is null)
            ExceptionManager.EntityNotFound<TAggregateRoot, TIdentifier>(id);
        dbContext.Set<TAggregateRoot>().Remove(entity);
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
        await dbContext.Set<TAggregateRoot>().FindAsync([identifier], cancellationToken: cancellationToken);

    public async Task<TResult?> GetByIdAsync<TResult, TId>(TId identifier, CancellationToken cancellationToken)
        where TResult : class
        => await dbContext.Set<TResult>().FindAsync([identifier], cancellationToken: cancellationToken);
}