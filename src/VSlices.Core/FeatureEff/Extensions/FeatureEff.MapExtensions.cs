using LanguageExt.Pipes;
using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, B> Map<B>(Func<A, B> f) =>
        new(Effect.Map(f));

    public FeatureEff<RT, B> Select<B>(Func<A, B> f) =>
        Map(f);

    public FeatureEff<RT, B> BiMap<B>(
        Func<A, B> Succ,
        Func<FeatureError, B> Fail) =>
        new(Effect.Match(Left: Fail, Right: Succ).As());

    public FeatureEff<RT, B> FullMap<B>(
        Func<A, B> Succ,
        Func<FeatureError, B> Fail,
        Func<Error, B> Except) =>
        new(Effect.Match(Left: Fail, Right: Succ).As()
                  .Match(Succ: identity, Fail: Except));

}
