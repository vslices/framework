namespace VSlices.Domain.Traits;

public interface DomainEntity<TId>
    where TId : Identifier<TId>
{
    TId Id { get; }

}