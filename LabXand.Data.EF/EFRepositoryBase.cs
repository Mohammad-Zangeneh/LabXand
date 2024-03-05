using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    public class EFRepositoryBase<TAggregateRoot>(DbContext dbContext) : IRepository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        protected readonly DbContext dbContext = dbContext;

        public IQueryable<TAggregateRoot> Query => dbContext.Set<TAggregateRoot>().AsNoTracking();

        public void Add(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Add(domain);  

        public void Edit(TAggregateRoot domain)
        {
            dbContext.Entry(domain).State = EntityState.Modified;
        }

        public void Remove(TAggregateRoot domain) => dbContext.Set<TAggregateRoot>().Remove(domain);
    }
}