namespace LabXand.SharedKernel
{
    public class BusinessEntityBase<TIdentifier> : EntityBase<TIdentifier>, IBusinessEntity<TIdentifier>
        where TIdentifier : struct
    {
        public Guid BusinessId { get; } = Guid.NewGuid();
    }
}