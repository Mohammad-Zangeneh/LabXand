using LabXand.Core;

namespace LabXand.SharedKernel.Exceptions;

public static class ExceptionManager
{
    static List<IExceptionHandler> exceptionHandlers = [
        new DomainValidationExceptionHandler()
        ];
    public static void RegisterHandler(IExceptionHandler handler) => exceptionHandlers.Add(handler);
    public static IExceptionHandler GetHandler(Exception exception) =>
        exceptionHandlers.FirstOrDefault(e => e.CanHandle(exception)) ?? new DefaultExceptionHandler();

    public static void EntityNotFound<T, TIdentifier>(TIdentifier identifier)
        where T : EntityBase<TIdentifier>
        where TIdentifier : struct
        => throw new EntityNotFoundException<T, TIdentifier>(identifier, TypeHelper.GetClassDescription<T>());
    public static void DomainUnValidate(string message) => throw new DomainValidationException(message);
}
