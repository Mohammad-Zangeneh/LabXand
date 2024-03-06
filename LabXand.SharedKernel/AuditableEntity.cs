namespace LabXand.SharedKernel
{
    public interface IAuditData;
    public interface IAuditableEntity<TAuditData> where TAuditData : IAuditData
    {
        void SetAuditData(TAuditData auditData);
    }
}