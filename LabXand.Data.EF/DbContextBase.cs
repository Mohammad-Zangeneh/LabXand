using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class DbContextBase<TContext>(DbContextOptions<TContext> options) : DbContext(options), IUnitOfWork
    where TContext : DbContext
{
    public Task CommitAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);
}
