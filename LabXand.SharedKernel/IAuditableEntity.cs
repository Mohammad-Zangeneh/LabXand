namespace LabXand.SharedKernel;

public interface IAuditableEntity<TUserContext> 
{
    void SetCreationAuditData(TUserContext userContext);
    void SetModificationAuditData(TUserContext userContext);
}