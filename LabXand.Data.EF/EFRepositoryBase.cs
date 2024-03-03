using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    public class EFRepositoryBase<TAgregateRoot, TIdentifier>(DbContext dbContext) : IRepository<TAgregateRoot>
        where TIdentifier : struct
        where TAgregateRoot : class, IAgregateRoot
    {
        protected readonly DbContext dbContext = dbContext;

        public IQueryable<TAgregateRoot> Query => dbContext.Set<TAgregateRoot>();

        public void Add(TAgregateRoot domain) => dbContext.Set<TAgregateRoot>().Add(domain);

        public void Edit(TAgregateRoot domain) => dbContext.Entry(domain).State = EntityState.Modified;

        public void Remove(TAgregateRoot domain) => dbContext.Set<TAgregateRoot>().Remove(domain);        
    }
}