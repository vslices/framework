namespace VSlices.Domain.Traits;

public interface DomainEntity<SELF, ID> : DomainType<SELF>
    where SELF : DomainType<SELF>
    where ID : Identifier<ID>
{
    ID Id { get; }

}

public interface DomainEntity<SELF, ID, REPR> :
    DomainEntity<SELF, ID>
    where SELF : DomainType<SELF, REPR>
    where ID : Identifier<ID>;
