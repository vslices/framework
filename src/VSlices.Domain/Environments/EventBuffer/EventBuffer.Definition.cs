using VSlices.Domain.Traits;

namespace VSlices.Domain.Environments.EventBuffer;

public interface EventBufferIO
{
    IO<Unit> Track(UntypedAggregateRoot root);

    IO<Unit> Commit();
}
