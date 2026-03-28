using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial class FeatureEff<RT> 
    : Fallible<FeatureError, FeatureEff<RT>>,
      Fallible<Exceptional, FeatureEff<RT>>,
      Final<FeatureEff<RT>>,
      Readable<FeatureEff<RT>, RT>,
      MonadUnliftIO<FeatureEff<RT>>
{
    public static K<FeatureEff<RT>, A> Pure<A>(A value) =>
        FeatureEff<RT, A>.Pure(value);

    public static FeatureEff<RT, A> Success<A>(A value) =>
        +Pure(value);

    public static K<FeatureEff<RT>, B> Fail<B>(Exceptional error) =>
        FeatureEff<RT, B>.Fail(error);

    public static K<FeatureEff<RT>, A> Catch<A>(
        K<FeatureEff<RT>, A> fa,
        Func<Exceptional, bool> Predicate,
        Func<Exceptional, K<FeatureEff<RT>, A>> Fail) =>
        fa.As().RunEff()
          .Catch(Predicate: e => e is Exceptional fe && Predicate(fe),
                 Fail: e => Fail((Exceptional)e).As().Effect.Run())
          .ToFeatureEff();

    public static K<FeatureEff<RT>, B> Fail<B>(FeatureError error) =>
        FeatureEff<RT, B>.Fail(error);

    public static K<FeatureEff<RT>, A> Catch<A>(
        K<FeatureEff<RT>, A> ma,
        Func<FeatureError, bool> Predicate,
        Func<FeatureError, K<FeatureEff<RT>, A>> Fail) =>
        ma.As().RunEff()
          .Catch(Predicate: e => e is FeatureError fe && Predicate(fe), 
                 Fail: e => Fail((FeatureError)e).As().Effect.Run())
          .ToFeatureEff();

    public static K<FeatureEff<RT>, A> Finally<X, A>(
        K<FeatureEff<RT>, A> fa, 
        K<FeatureEff<RT>, X> @finally) =>
        fa.As().RunEff()
          .Finally(@finally.RunEff())
          .ToFeatureEff();
    
    public static K<FeatureEff<RT>, A> Asks<A>(Func<RT, A> f) => 
        Readable.asks<FeatureEff<RT>, RT, A>(f);

    public static K<FeatureEff<RT>, A> Local<A>(Func<RT, RT> f, K<FeatureEff<RT>, A> ma) => 
        Readable.local(f, ma);

    public static K<FeatureEff<RT>, IO<A>> ToIO<A>(K<FeatureEff<RT>, A> ma) =>
        ma.As().Map(a => IO.pure(a));
    
    public static K<FeatureEff<RT>, A> LiftIO<A>(IO<A> ma) => 
        FeatureEff<RT, A>.LiftIO(ma);
    
    public static K<FeatureEff<RT>, B> Bind<A, B>(K<FeatureEff<RT>, A> ma, Func<A, K<FeatureEff<RT>, B>> f) => 
        ma.Bind(f);
    
    public static K<FeatureEff<RT>, B> Recur<A, B>(A value, Func<A, K<FeatureEff<RT>, Next<A, B>>> f) =>
        liftFeat(async (RT env) =>
        {
            while (true)
            {
                Fin<Either<FeatureError, Next<A, B>>> mNext = await f(value).As().RunFeatureAsync(env, EnvIO.New());
                if (mNext.IsFail)
                {
                    var error = mNext.FailSpan().ToArray().First();

                    return Fin.Fail<Either<FeatureError, B>>(error);
                }

                var next = (Next<A, B>)mNext.SuccSpan().ToArray().First();
                if (next.IsDone) return Fin.Succ<Either<FeatureError, B>>(next.Done);

                value = next.Loop;
            }
        });
    
    public static K<FeatureEff<RT>, B> Apply<A, B>(K<FeatureEff<RT>, Func<A, B>> mf, K<FeatureEff<RT>, A> ma) =>
        mf.As().Effect
          .Apply(ma.As().Effect)
          .ToFeatureEff();
    
    public static K<FeatureEff<RT>, B> Apply<A, B>(K<FeatureEff<RT>, Func<A, B>> mf, Memo<FeatureEff<RT>, A> ma) =>
        mf.As().Effect
          .Apply(ma.Value.As().Effect).As()
          .ToFeatureEff();

    public static K<FeatureEff<RT>, B> Map<A, B>(Func<A, B> f, K<FeatureEff<RT>, A> ma) => 
        ma.As().Map(f);

    public static FeatureEff<RT, (RT Runtime, EnvIO EnvIO)> getState =>
        from runtime in +Readable.ask<FeatureEff<RT>, RT>()
        from envIO in envIO
        select (runtime, envIO);
}
