namespace LabXand.Security;

public interface IUserContextDetector<TUserContext>
        where TUserContext : class, IUserContext
{
    TUserContext UserContext { get; }
}