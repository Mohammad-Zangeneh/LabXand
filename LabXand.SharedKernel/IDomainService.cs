namespace LabXand.SharedKernel
{
    public interface IDomainService<TAggregateRoot, TIdentifier> 
        where TAggregateRoot : IAggregateRoot
        where TIdentifier : struct
    {
        TAggregateRoot Create(TAggregateRoot agregate);
        TAggregateRoot CreateAsync(TAggregateRoot agregate, CancellationToken cancellationToken);
        TAggregateRoot Update(TAggregateRoot agregate);
        TAggregateRoot UpdateAsync(TAggregateRoot agregate, CancellationToken cancellationToken);
        bool Delete(TIdentifier identifier);
        Task<bool> DeleteAsync(TIdentifier identifier, CancellationToken cancellationToken);

    }
}