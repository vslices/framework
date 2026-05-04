using VSlices.Domain.Traits;

namespace VSlices;

public static class AggregateRootExtensions
{
    extension<TSelf, TId>(TSelf instance)
        where TSelf : AggregateRoot<TSelf, TId>
        where TId : Identifier<TId>
    {
        public bool SameIdentityAs(TSelf other) =>
            instance.Id == other.Id;
    }
}
