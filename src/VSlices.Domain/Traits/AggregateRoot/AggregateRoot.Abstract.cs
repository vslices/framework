namespace VSlices.Domain.Traits;

public static class AggregateRoot
{
    public abstract record Base<TSelf, TId>(TId Id) : AggregateRoot<TSelf, TId>
        where TSelf : Base<TSelf, TId>
        where TId : Identifier<TId>
    {
        private Seq<DomainEvent> _events = Empty;

        public virtual bool Equals(TSelf? obj) =>
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
    }
}
