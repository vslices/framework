using System.ComponentModel;

namespace VSlices.Domain.Traits;

[EditorBrowsable(EditorBrowsableState.Never)]
public partial interface AggregateRoot<SELF>;

public partial interface AggregateRoot<SELF, ID> :
    AggregateRoot<SELF>, DomainEntity<SELF, ID>
    where SELF : AggregateRoot<SELF, ID>

public partial interface AggregateRoot<SELF, ID, REPR> : 
    AggregateRoot<SELF>, DomainEntity<SELF, ID, REPR>
    where SELF : AggregateRoot<SELF, ID, REPR>
    where ID : Identifier<ID>;
