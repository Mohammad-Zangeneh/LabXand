using LabXand.SharedKernel;

namespace LabXand.Data.EF;

public class ViolatedRestrictionException(object entity, List<IRestriction> restrictions) : Exception
{
    public object Entity { get; } = entity;
    public List<IRestriction> Restrictions { get; } = restrictions;
}
