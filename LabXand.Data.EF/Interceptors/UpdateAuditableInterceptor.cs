using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using LabXand.Security;
using Ardalis.GuardClauses;

namespace LabXand.Data.EF.Interceptors;

public class UpdateAuditableInterceptor<TUserContext>(IUserContextDetector<TUserContext> userContextDetector) : SaveChangesInterceptor
    where TUserContext : class, IUserContext
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
          DbContextEventData eventData,
          InterceptionResult<int> result,
          CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext context)
    {
        Guard.Against.Null(userContextDetector.UserContext, nameof(userContextDetector.UserContext));
        var userContext = userContextDetector.UserContext;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity<TUserContext>>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.SetCreationAuditData(userContext);
            else if (entry.State == EntityState.Modified)
                entry.Entity.SetModificationAuditData(userContext);

        }
    }
}