using System.Net;

namespace LabXand.SharedKernel.Exceptions;

public abstract class ExceptionHandlerBase<TException> : IExceptionHandler
    where TException : Exception
{
    public abstract HttpStatusCode HttpCode { get; }

    public virtual bool CanHandle(Exception exception) => exception is TException;

    public abstract string GetUserMessage(Exception exception);
}
