namespace LabXand.SharedKernel;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
}

public interface IEntity<TIdentifier> : IEntity where TIdentifier : struct
{
    TIdentifier Id { get; }
    string DescribeEntity();
}