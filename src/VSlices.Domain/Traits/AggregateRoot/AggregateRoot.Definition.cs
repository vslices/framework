using System.ComponentModel;

namespace VSlices.Domain.Traits;

[EditorBrowsable(EditorBrowsableState.Never)]
public partial interface UntypedAggregateRoot
{
    Seq<DomainEvent> DequeueEvents();
}

[EditorBrowsable(EditorBrowsableState.Never)]
public partial interface AggregateRoot<TSelf> : UntypedAggregateRoot;

public partial interface AggregateRoot<TSelf, TId> : AggregateRoot<TSelf>, DomainEntity<TId>
    where TSelf : AggregateRoot<TSelf, TId>
    where TId : Identifier<TId>;
