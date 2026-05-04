using VSlices.Domain.Traits;

namespace VSlices.Domain.Errors;

public sealed record NotFoundError<TRoot>() : Expected("NotFound", 404)
    where TRoot : UntypedAggregateRoot
{
    public static Error Get() => new NotFoundError<TRoot>();
}

