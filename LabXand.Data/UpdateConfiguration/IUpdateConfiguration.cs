namespace LabXand.Data;

public interface IUpdateConfiguration<TRoot>
    where TRoot : class
{
    List<string> ConstantFields { get; set; }
}
