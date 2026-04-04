using VSlices.Core;
using VSlices.Domain.Environments.Persistence;

namespace VSlices;

public static partial class VSlicesCorePrelude
{
    public static FeatureEff<RT, A> transactionScope<RT, A>(K<FeatureEff<RT>, A> ma)
        where RT : HasPersistence<RT> => 
        from _0 in PersistenceEnv<RT>.start().ToFeatureEff()
        from a in ma.As().FullBind(
            Succ: FeatureEff<RT, A> (A v) => PersistenceEnv<RT>.commit().Map(_ => v),
            Fail: FeatureEff<RT, A> (fe) => PersistenceEnv<RT>.rollback()
                                                              .Map(_ => Fail(fe).ToEither<A>()),
            Except: FeatureEff<RT, A> (ex) => PersistenceEnv<RT>.rollback()
                                                                .Bind(_ => Fail(ex).ToEff<A>()))

        select a;

    public static FeatureEff<RT, A> InTransaction<RT, A>(this K<FeatureEff<RT>, A> ma)
        where RT : HasPersistence<RT> =>
        transactionScope(ma);

}
