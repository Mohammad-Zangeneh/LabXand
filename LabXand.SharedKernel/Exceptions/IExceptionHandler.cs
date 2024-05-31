using System.Net;

namespace LabXand.SharedKernel.Exceptions;

public interface IExceptionHandler
{
    bool CanHandle(Exception exception);
    string GetUserMessage(Exception exception);
    HttpStatusCode HttpCode { get; }
}
