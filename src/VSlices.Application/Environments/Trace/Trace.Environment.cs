
namespace VSlices.Application.Environments;

public interface HasTrace<RT> : Has<Eff<RT>, TraceIO>;

public partial record TraceEnv<RT>
    where RT : HasTrace<RT>
{
    static Eff<RT, TraceIO> traceIO =>
        Has<Eff<RT>, RT, TraceIO>.ask.As();


}
