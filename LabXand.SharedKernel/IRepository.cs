namespace LabXand.SharedKernel
{
    public interface IRepository<TAggregateRoot, TIdentifier> : IReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : struct
    {
        void Add(TAggregateRoot domain);
        void Edit(TAggregateRoot domain);
        Task RemoveAsync(TIdentifier identifier, CancellationToken cancellationToken);
    }
}