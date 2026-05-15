namespace VSlices.Domain;

public interface HasIO<RT> : Has<Eff<RT>, EnvIO>;

public interface CoreRuntime<SELF> : HasIO<SELF>
    where SELF : CoreRuntime<SELF>
{
}
