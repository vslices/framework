using VSlices.Domain.Errors;
using VSlices.Domain.Traits;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    public static Fail<Error> notFound<TRoot>()
        where TRoot : UntypedAggregateRoot =>
        Fail(NotFoundError.Get<TRoot>());
}
