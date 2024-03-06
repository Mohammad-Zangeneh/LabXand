namespace LabXand.SharedKernel
{
    public interface IBusinessEntity<TIdentifier> where TIdentifier : struct
    {
        Guid BusinessId { get; }
    }
}