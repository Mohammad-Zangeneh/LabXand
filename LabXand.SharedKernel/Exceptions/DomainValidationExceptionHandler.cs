using System.Net;

namespace LabXand.SharedKernel.Exceptions;

public class DomainValidationExceptionHandler : ExceptionHandlerBase<DomainValidationException>
{
    public override HttpStatusCode HttpCode => HttpStatusCode.BadRequest;

    public override string GetUserMessage(Exception exception) => exception.Message;
}
