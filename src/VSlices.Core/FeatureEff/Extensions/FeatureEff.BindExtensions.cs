using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, B> Bind<B>(Func<A, FeatureEff<RT, B>> f) =>
        Effect.Bind(x => f(x).Effect).ToFeatureEff();

    public FeatureEff<RT, B> Bind<B>(Func<A, K<FeatureEff<RT>, B>> f) =>
        Bind(x => f(x).As());

    public FeatureEff<RT, B> Bind<B>(Func<A, IO<B>> f) =>
        Bind(x => f(x).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> Bind<B>(Func<A, K<IO, B>> f) =>
        Bind(x => f(x).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> Bind<B>(Func<A, Eff<RT, B>> f) =>
        Bind(s => f(s).ToFeatureEff());

    public FeatureEff<RT, B> Bind<B>(Func<A, K<Eff<RT>, B>> f) =>
        Bind(s => f(s).ToFeatureEff());

    public FeatureEff<RT, B> Bind<B>(Func<A, Eff<B>> f) =>
        Bind(s => f(s).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> Bind<B>(Func<A, K<Eff, B>> f) =>
        Bind(s => f(s).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> Bind<B>(Func<A, Pure<B>> f) =>
        Map(x => f(x).Value);

    public FeatureEff<RT, B> Bind<B>(Func<A, Fail<Exceptional>> f) =>
        Bind(x => FeatureEff<RT>.Fail<B>(f(x).Value));

    public FeatureEff<RT, B> Bind<B>(Func<A, Fail<FeatureError>> f) =>
        Bind(x => FeatureEff<RT>.Fail<B>(f(x).Value));
}
