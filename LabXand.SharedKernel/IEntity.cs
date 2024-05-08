namespace LabXand.SharedKernel;

public interface IEntity<TIdentifier> where TIdentifier : struct
{
    TIdentifier Id { get; }
    IList<IDomainEvent> Events { get; }
    string DescribeEntity();
}