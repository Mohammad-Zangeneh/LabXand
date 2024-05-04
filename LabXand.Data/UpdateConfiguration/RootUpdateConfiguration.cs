namespace LabXand.Data;

public class RootUpdateConfiguration<TRoot>(List<string> constantFields) : UpdateConfigarationBase<TRoot>(constantFields)
    where TRoot : class
{
    public RootUpdateConfiguration()
        : this([])
    {

    }
}
