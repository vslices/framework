using VSlices.Domain.Traits;

namespace VSlices.Domain.Environments.EventBuffer;

public record EventBufferEnv<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, EventBufferIO>
{
    static K<M, EventBufferIO> eventBufferIO => Has<M, RT, EventBufferIO>.ask;

    public static K<M, Unit> track(UntypedAggregateRoot root) =>
       eventBufferIO.Bind(io => io.Track(root));

    public static K<M, Unit> commit() =>
        eventBufferIO.Bind(io => io.Commit());
}

public record EventBufferEnv<RT>
    where RT : Has<Eff<RT>, EventBufferIO>
{
    public static Eff<RT, Unit> track(UntypedAggregateRoot root) =>
        EventBufferEnv<Eff<RT>, RT>.track(root).As();

    internal static Eff<RT, Unit> commit() =>
        EventBufferEnv<Eff<RT>, RT>.commit().As();
}