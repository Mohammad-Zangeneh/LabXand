namespace LabXand.SharedKernel
{
    public interface IUnitOfWork 
    { 
        Task CommitAsync(CancellationToken cancellationToken);
    }
}