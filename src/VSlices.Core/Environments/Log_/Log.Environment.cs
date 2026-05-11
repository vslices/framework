namespace VSlices.Application.Environments;

public interface HasLog<RT> : Has<Eff<RT>, LogIO>;

public partial record LogEnv<RT>
    where RT : HasLog<RT>
{
    static Eff<RT, LogIO> traceIO =>
        Has<Eff<RT>, RT, LogIO>.ask.As();


}
