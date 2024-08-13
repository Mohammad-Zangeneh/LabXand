namespace LabXand.SharedKernel;

public interface IDomainEvent 
{
    DateTime OccurredOn { get; }
}