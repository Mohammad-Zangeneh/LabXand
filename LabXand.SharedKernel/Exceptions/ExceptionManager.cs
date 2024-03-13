using LabXand.Core;

namespace LabXand.SharedKernel.Exceptions;

public static class ExceptionManager
{
    public static void EntityNotFound<T, TIdentifier>(TIdentifier identifier)
        where T : EntityBase<TIdentifier>
        where TIdentifier : struct
        => throw new EntityNotFoundException<T, TIdentifier>(identifier, TypeHelper.GetClassDescription<T>());
}
