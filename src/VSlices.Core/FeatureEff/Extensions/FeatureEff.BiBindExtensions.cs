using LanguageExt.Pipes;
using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, B> BiBind<B>(
        Func<A, FeatureEff<RT, B>> Succ,
        Func<FeatureError, FeatureEff<RT, B>> Fail) =>
        Effect.Run().As()
              .SuccessM(r => r.Right(Succ).Left(Fail).RunEff())
              .FailureM(e => Prelude.Fail(e))
              .ToFeatureEff();

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, K<FeatureEff<RT>, B>> Succ,
        Func<FeatureError, K<FeatureEff<RT>, B>> Fail) =>
        BiBind(Succ, Fail: e => Fail(e).As());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, IO<B>> Succ,
        Func<FeatureError, IO<B>> Fail) =>
        BiBind(a => Succ(a).ToFeatureEff<RT, B>(), 
               e => Fail(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, K<IO, B>> Succ,
        Func<FeatureError, K<IO, B>> Fail) =>
        BiBind(a => Succ(a).ToFeatureEff<RT, B>(),
               e => Fail(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, Eff<RT, B>> Succ,
        Func<FeatureError, Eff<RT, B>> Fail) =>
        BiBind(a => Succ(a).ToFeatureEff(),
               e => Fail(e).ToFeatureEff());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, K<Eff<RT>, B>> Succ,
        Func<FeatureError, K<Eff<RT>, B>> Fail) =>
        BiBind(a => Succ(a).ToFeatureEff(),
               e => Fail(e).ToFeatureEff());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, Eff<B>> Succ,
        Func<FeatureError, Eff<B>> Fail) =>
        BiBind(a => Succ(a).WithRuntime<RT>().ToFeatureEff(),
               e => Fail(e).WithRuntime<RT>().ToFeatureEff());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, K<Eff, B>> Succ,
        Func<FeatureError, K<Eff, B>> Fail) =>
        BiBind(a => Succ(a).ToFeatureEff<RT, B>(),
               e => Fail(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, Pure<B>> Succ,
        Func<FeatureError, Pure<B>> Fail) =>
        BiBind(FeatureEff<RT, B> (a) => Succ(a),
               e => Fail(e));

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, Fail<Exceptional>> Succ,
        Func<FeatureError, Fail<Exceptional>> Fail) =>
        BiBind(FeatureEff<RT, B> (s) => Succ(s), 
               e => Fail(e));

    public FeatureEff<RT, B> BiBind<B>(
        Func<A, Fail<FeatureError>> Succ,
        Func<FeatureError, Fail<FeatureError>> Fail) =>
        BiBind(FeatureEff<RT, B> (x) => Succ(x),
               x => Fail(x));
}
