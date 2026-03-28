using VSlices.Domain.Traits;

namespace VSlices.Domain.Environments.EventBuffer;

public interface HasEventBuffer<TSelf> : Has<Eff<TSelf>, EventBufferIO>;

public record EventBufferEnv<RT>
    where RT : HasEventBuffer<RT>
{
    static Eff<RT, EventBufferIO> eventBufferIO => 
        Has<Eff<RT>, RT, EventBufferIO>.ask.As();

    public static Eff<RT, Unit> track(UntypedAggregateRoot root) =>
       eventBufferIO.Bind(io => io.Track(root));

    public static Eff<RT, Unit> commit() =>
        eventBufferIO.Bind(io => io.Commit());
}
