using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    public class EFRepositoryBase<TAggregateRoot>(DbContext dbContext) : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly DbContext dbContext = dbContext;

        public IQueryable<TAggregateRoot> Query => dbContext.Set<TAggregateRoot>().AsNoTracking();
        public Task<int> CountAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class => query.CountAsync(cancellationToken);
        public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class => query.FirstOrDefaultAsync(cancellationToken);
        public Task<List<T>> GetListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken) where T : class => query.ToListAsync(cancellationToken);
        public Task<List<T>> GetPaginatedItemsAsync<T>(IQueryable<T> query, int page, int size, CancellationToken cancellationToken) where T : class => query.Skip((page - 1) * size).Take(size).ToListAsync(cancellationToken);
        public virtual void Add(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Add(domain);
        public virtual void Edit(TAggregateRoot domain)
        {
            dbContext.Entry(domain).State = EntityState.Modified;
        }
        public virtual void Remove(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Remove(domain);
    }
}