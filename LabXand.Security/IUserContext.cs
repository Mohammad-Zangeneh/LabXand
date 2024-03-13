namespace LabXand.Security;

public interface IUserContext
{
    Guid UserId { get; }
    string UserName { get; }
}
