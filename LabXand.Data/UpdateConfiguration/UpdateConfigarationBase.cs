namespace LabXand.Data;

public abstract class UpdateConfigarationBase<TRoot>(List<string> constantFields) : IUpdateConfiguration<TRoot>
    where TRoot : class
{
    public List<string> ConstantFields { get; set; } = constantFields;
}
