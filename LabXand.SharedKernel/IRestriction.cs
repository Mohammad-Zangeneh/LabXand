namespace LabXand.SharedKernel;

public interface IRestriction<T, TIdentifier>
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
{
    ISpecification<T, TIdentifier> Specification { get; }
    RestrictionTypes Type { get; }
    string Title { get; }
    string Description { get; }
}
