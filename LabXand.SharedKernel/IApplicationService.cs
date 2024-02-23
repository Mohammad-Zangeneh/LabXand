namespace LabXand.SharedKernel
{
    public interface IApplicationService<TAgregateRoot, TIdentifier>
        where TIdentifier : struct
        where TAgregateRoot : IAgregateRoot<TIdentifier> 
    { 

    }
}