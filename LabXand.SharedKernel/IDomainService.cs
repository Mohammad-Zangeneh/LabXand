namespace LabXand.SharedKernel
{
    public interface IDomainService<TAgregateRoot, TIdentifier> 
        where TAgregateRoot : IAggregateRoot
        where TIdentifier : struct
    {
        TAgregateRoot Create(TAgregateRoot agregate);
        TAgregateRoot CreateAsync(TAgregateRoot agregate, CancellationToken cancellationToken);
        TAgregateRoot Update(TAgregateRoot agregate);
        TAgregateRoot UpdateAsync(TAgregateRoot agregate, CancellationToken cancellationToken);
        bool Delete(TIdentifier identifier);
        Task<bool> DeleteAsync(TIdentifier identifier, CancellationToken cancellationToken);

    }
}