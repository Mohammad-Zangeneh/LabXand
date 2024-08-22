namespace LabXand.SharedKernel;

public interface IRestriction
{
    RestrictionTypes Type { get; }
    string Title { get; }
}

public interface IRestriction<T, TIdentifier> : IRestriction
        where T : IEntity<TIdentifier>
        where TIdentifier : struct
{
    ISpecification<T, TIdentifier> Specification { get; }
    string GetMessage(T entity);

}
