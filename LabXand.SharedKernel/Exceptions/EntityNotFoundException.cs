namespace LabXand.SharedKernel.Exceptions;
public class EntityNotFoundException<TEntity, TIdentifier>(TIdentifier identifier, string objectDescriptor) : Exception($"{objectDescriptor} not found. Key: {identifier}")
    where TEntity : EntityBase<TIdentifier>
    where TIdentifier : struct
{
}
