namespace LabXand.SharedKernel
{
    public interface IAgregateRoot<TIdentifier> : IEntity<TIdentifier> where TIdentifier : struct { }
}