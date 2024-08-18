using LabXand.SharedKernel;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LabXand.Data.EF.Interceptors;

public class DomainEventInterceptor(IEventDispatcher eventDispatcher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = eventData.Context!.ChangeTracker.Entries<EntityBase>()
            .Where(e => e.Entity.DomainEvents.Count > 0)
            .Select(e => e.Entity)
            .ToList();

        if (entitiesWithEvents.Count > 0)
        {
            var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();

            foreach (var domainEvent in domainEvents)
            {
                await eventDispatcher.DispatchAsync(domainEvent, cancellationToken);
            }

            entitiesWithEvents.ForEach(entity => entity.ClearDomainEvents());
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
}
