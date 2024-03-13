namespace LabXand.SharedKernel;

public interface IAuditableEntity<TAuditData> where TAuditData : IAuditData
{
    void SetCreationAuditData(TAuditData auditData);
    void SetModificationAuditData(TAuditData auditData);
    void SetDeletionAuditData(TAuditData auditData);
}