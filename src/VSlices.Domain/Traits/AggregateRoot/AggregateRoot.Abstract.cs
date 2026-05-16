namespace VSlices.Domain.Traits;

/// <summary>
/// 
/// </summary>
public static class AggregateRoot
{
    public abstract record Base<SELF, ID, REPR>(ID Id) : 
        AggregateRoot<SELF, ID, REPR>
        where SELF : Base<SELF, ID, REPR>
        where ID : Identifier<ID>
    {
        private Seq<DomainEvent> _events = Empty;

        public virtual bool Equals(SELF? obj) =>
            obj is not null &&             Id == obj.Id;

        public override int GetHashCode() =>
            Id.GetHashCode();

        protected Unit Raise(DomainEvent @event)
        {
            _events = _events.Add(@event);

            return unit;
        }

        public Seq<DomainEvent> DequeueEvents()
        {

            var events = _events;
            _events = Empty;

            return events;
        }

        public abstract REPR To();
    }
}
