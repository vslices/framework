namespace VSlices.Core;

public sealed partial class FeatureEff
{
    public static FeatureEff<RT, (RT Runtime, EnvIO EnvIO)> getState<RT>() =>
        FeatureEff<RT>.getState;

    public static FeatureEff<RT, A> localCancel<RT, A>(FeatureEff<RT, A> ma) =>
        ma.LocalIO().As();

    public static FeatureEff<OuterRT, A> local<OuterRT, InnerRT, A>(
        Func<OuterRT, InnerRT> f,
        FeatureEff<InnerRT, A> ma) =>
        from state in getState<OuterRT>()
        from localResponse in IO.local(ma.RunIO(f(state.Runtime))).As()
                                .Bind(m => FeatureEff<OuterRT>.Lift(m))
        select localResponse;
}
