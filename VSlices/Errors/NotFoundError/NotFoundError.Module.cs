using VSlices.Domain.Traits;

namespace VSlices.Domain.Errors;

public static class NotFoundError
{
    public static Error Get<TRoot>()
        where TRoot : UntypedAggregateRoot =>
        NotFoundError<TRoot>.Get();
}
