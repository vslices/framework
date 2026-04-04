using VSlices.Domain.Environments.EventBuffer;

namespace VSlices.Core;

public static partial class FeatureEffExtensions
{
    public static FeatureEff<RT, A> FlushTrackedEvents<RT, A>(this K<FeatureEff<RT>, A> ma)
        where RT : HasEventBuffer<RT> =>
        from a in ma.As()
        from _0 in EventBufferEnv<RT>.commit()
        select a;
}
