using VSlices.Domain.Traits;

namespace VSlices;

public static class AggregateRootExtensions
{
    extension<SELF, ID>(SELF instance)
        where SELF : AggregateRoot<SELF, ID>
        where ID : Identifier<ID>
    {
        public bool SameIdentityAs(SELF other) =>
            instance.Id == other.Id;
    }
}
