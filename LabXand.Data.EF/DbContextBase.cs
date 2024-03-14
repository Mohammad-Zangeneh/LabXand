using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class DbContextBase(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);
}
