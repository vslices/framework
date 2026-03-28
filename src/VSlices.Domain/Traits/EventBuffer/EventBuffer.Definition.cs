namespace VSlices.Domain.Traits;

public interface EventBufferIO
{
    IO<Unit> Track(UntypedAggregateRoot root);

    IO<Unit> Commit();
}
