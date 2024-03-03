using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    public class EFRepositoryBase<TAggregateRoot, TIdentifier>(DbContext dbContext) : IRepository<TAggregateRoot>
        where TIdentifier : struct
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly DbContext dbContext = dbContext;

        public IQueryable<TAggregateRoot> Query => dbContext.Set<TAggregateRoot>();

        public void Add(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Add(domain);

        public void Edit(TAggregateRoot domain) => dbContext.Entry(domain).State = EntityState.Modified;

        public void Remove(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Remove(domain);        
    }
}