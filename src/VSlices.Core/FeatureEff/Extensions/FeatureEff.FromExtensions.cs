using VSlices.Core;
using VSlices.Core.Errors;

namespace LanguageExt;

public static class FeatureEffExtensions
{
    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<A> ma) =>
        new(ma.WithRuntime<RT>());

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff, A> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<Either<FeatureError, A>> ma) =>
        new(ma.WithRuntime<RT>());

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff, Either<FeatureError, A>> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<K<Either<FeatureError>, A>> ma) =>
        new(ma.WithRuntime<RT>());

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff, K<Either<FeatureError>, A>> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<RT, A> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff<RT>, A> ma) =>
        ma.As().ToFeatureEff();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<RT, Either<FeatureError, A>> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff<RT>, Either<FeatureError, A>> ma) =>
        ma.As().ToFeatureEff();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Eff<RT, K<Either<FeatureError>, A>> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Eff<RT>, K<Either<FeatureError>, A>> ma) =>
        ma.As().ToFeatureEff();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Either<FeatureError, A> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<Either<FeatureError>, A> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this EitherT<FeatureError, Eff<RT>, A> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<EitherT<FeatureError, Eff<RT>>, A> ma) =>
        ma.As().ToFeatureEff();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this EitherT<FeatureError, Eff, A> ma) =>
        new(ma.MapT(e => e.As().WithRuntime<RT>()));

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<EitherT<FeatureError, Eff>, A> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this IO<A> ma) =>
        new(ma);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this K<IO, A> ma) =>
        ma.As().ToFeatureEff<RT, A>();

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Pure<A> m) =>
        FeatureEff<RT, A>.Success(m.Value);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Fail<Exceptional> m) =>
        FeatureEff<RT, A>.Fail(m.Value);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Fail<FeatureError> m) =>
        FeatureEff<RT, A>.Fail(m.Value);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Lift<A> lift) =>
        FeatureEff<RT, A>.Lift(lift.Function);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Lift<Task<A>> lift) =>
        FeatureEff<RT, A>.Lift(lift.Function);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Lift<RT, A> lift) =>
        FeatureEff<RT, A>.Lift(lift.Function);

    public static FeatureEff<RT, A> ToFeatureEff<RT, A>(this Lift<RT, Task<A>> lift) =>
        FeatureEff<RT, A>.Lift(lift.Function);
}
