using LanguageExt;
using LanguageExt.Traits;

namespace VSlices.Domain.Traits;

public interface DatabaseIO
{
    IO<A> Resolve<A>()
        where A : Repository;
}

public interface HasDatabase<RT> :
    Has<Eff<RT>, DatabaseIO>;

public static class DatabaseEnv
{
    private static Eff<RT, DatabaseIO> accessIO<RT>()
        where RT : HasDatabase<RT> =>
        Has<Eff<RT>, RT, DatabaseIO>.ask.As();

    public static Eff<RT, A> repository<RT, A>()
        where RT : HasDatabase<RT>
        where A : Repository =>
        from database in accessIO<RT>()
        from repository in database.Resolve<A>()
        select repository;
}
