using LabXand.SharedKernel;

namespace LabXand.Data.EF;

public class ViolatedRestrictionException(string message, object entity, List<IRestriction> restrictions) : Exception(message)
{
    public object Entity { get; } = entity;
    public List<IRestriction> Restrictions { get; } = restrictions;
}
