using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace LabXand.Data.EF;

public class DbContextBase : DbContext, IUnitOfWork
{
    public Task CommitAsync(CancellationToken cancellationToken) => SaveChangesAsync(cancellationToken);
}
