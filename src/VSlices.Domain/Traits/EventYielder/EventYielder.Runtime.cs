using VSlices.Domain.Traits;

namespace VSlices.Domain.Traits.EventYielder;

public record EventYielder<M, RT>
    where M : MonadIO<M>
    where RT : Has<M, EventYielderIO>
{
    static K<M, EventYielderIO> eventYielderIO => Has<M, RT, EventYielderIO>.ask;

    public static K<M, DomainEvent> yield() =>
        eventYielderIO.Bind(io => io.Yield());
}

public record EventYielder<RT>
    where RT  : Has<Eff<RT>, EventYielderIO>
{
    public static Eff<RT, DomainEvent> yield() =>
        EventYielder<Eff<RT>, RT>.yield().As();
}