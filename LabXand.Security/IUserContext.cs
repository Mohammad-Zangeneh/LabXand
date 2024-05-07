namespace LabXand.Security;

public interface IUserContext
{
    string UserName { get; }
}
public interface IUserContext<TId> : IUserContext
{
    TId UserId { get; }
}