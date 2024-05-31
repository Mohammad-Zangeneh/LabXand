using System.Net;

namespace LabXand.SharedKernel.Exceptions;

public class DefaultExceptionHandler : IExceptionHandler
{
    public HttpStatusCode HttpCode => HttpStatusCode.InternalServerError;

    public bool CanHandle(Exception exception) => true;

    public string GetUserMessage(Exception exception) => "خطا در اجرای برنامه";
}
