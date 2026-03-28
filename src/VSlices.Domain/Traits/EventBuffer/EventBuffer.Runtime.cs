using VSlices.Domain.Traits;

namespace VSlices.Domain.Traits.EventBuffer;

public record EventBuffer<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, EventBufferIO>
{
    static K<M, EventBufferIO> eventBufferIO => Has<M, RT, EventBufferIO>.ask;

    public static K<M, Unit> track(UntypedAggregateRoot root) =>
       eventBufferIO.Bind(io => io.Track(root));

    public static K<M, Unit> commit() =>
        eventBufferIO.Bind(io => io.Commit());
}

public record EventBuffer<RT>
    where RT : Has<Eff<RT>, EventBufferIO>
{
    public static Eff<RT, Unit> track(UntypedAggregateRoot root) =>
        EventBuffer<Eff<RT>, RT>.track(root).As();

    internal static Eff<RT, Unit> commit() =>
        EventBuffer<Eff<RT>, RT>.commit().As();
}