using LanguageExt.Pipes;
using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>
{
    public FeatureEff<RT, B> FullBind<B>(
        Func<A, FeatureEff<RT, B>> Succ,
        Func<FeatureError, FeatureEff<RT, B>> Fail,
        Func<Error, FeatureEff<RT, B>> Except) =>
        Effect.Run().As()
              .SuccessM(r => r.Right(Succ).Left(Fail).RunEff())
              .FailureM(e => Except(e).RunEff())
              .ToFeatureEff();

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, K<FeatureEff<RT>, B>> Succ,
        Func<FeatureError, K<FeatureEff<RT>, B>> Fail,
        Func<Error, K<FeatureEff<RT>, B>> Except) =>
        FullBind(Succ: e => Succ(e).As(), 
                 Fail: e => Fail(e).As(),
                 Except: e => Except(e).As());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, IO<B>> Succ,
        Func<FeatureError, IO<B>> Fail,
        Func<Error, IO<B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff<RT, B>(), 
                 Fail: e => Fail(e).ToFeatureEff<RT, B>(),
                 Except: e => Except(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, K<IO, B>> Succ,
        Func<FeatureError, K<IO, B>> Fail,
        Func<Error, K<IO, B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff<RT, B>(),
                 Fail: e => Fail(e).ToFeatureEff<RT, B>(),
                 Except: e => Except(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, Eff<RT, B>> Succ,
        Func<FeatureError, Eff<RT, B>> Fail,
        Func<Error, Eff<RT, B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff(),
                 Fail: e => Fail(e).ToFeatureEff(),
                 Except: e => Except(e).ToFeatureEff());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, K<Eff<RT>, B>> Succ,
        Func<FeatureError, K<Eff<RT>, B>> Fail,
        Func<Error, K<Eff<RT>, B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff(),
                 Fail: e => Fail(e).ToFeatureEff(),
                 Except: e => Except(e).ToFeatureEff());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, Eff<B>> Succ,
        Func<FeatureError, Eff<B>> Fail,
        Func<Error, Eff<B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff<RT, B>(),
                 Fail: e => Fail(e).ToFeatureEff<RT, B>(),
                 Except: e => Except(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, K<Eff, B>> Succ,
        Func<FeatureError, K<Eff, B>> Fail,
        Func<Error, K<Eff, B>> Except) =>
        FullBind(Succ: a => Succ(a).ToFeatureEff<RT, B>(),
                 Fail: e => Fail(e).ToFeatureEff<RT, B>(),
                 Except: e => Except(e).ToFeatureEff<RT, B>());

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, Pure<B>> Succ,
        Func<FeatureError, Pure<B>> Fail,
        Func<Error, Pure<B>> Except) =>
        FullBind(Succ: FeatureEff<RT, B> (a) => Succ(a),
                 Fail: e => Fail(e),
                 Except: e => Except(e));

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, Fail<Exceptional>> Succ,
        Func<FeatureError, Fail<Exceptional>> Fail,
        Func<Error, Fail<Exceptional>> Except) =>
        FullBind(Succ: FeatureEff<RT, B> (s) => Succ(s), 
                 Fail: (e) => Fail(e),
                 Except: (e) => Except(e));

    public FeatureEff<RT, B> FullBind<B>(
        Func<A, Fail<FeatureError>> Succ,
        Func<FeatureError, Fail<FeatureError>> Fail,
        Func<Error, Fail<FeatureError>> Except) =>
        FullBind(Succ: FeatureEff<RT, B> (x) => Succ(x),
                 Fail: x => Fail(x),
                 Except: x => Except(x));
}
