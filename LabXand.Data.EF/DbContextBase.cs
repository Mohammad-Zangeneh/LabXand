using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF
{
    internal class DbContextBase : DbContext, IUnitOfWork
    {
        public Task CommitAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);
    }
}
