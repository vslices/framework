namespace VSlices.Monads;

public partial class Flow<C, R>
{
    public static Flow<C, R, A> LiftIO<A>(Func<C, R, IO<A>> f) =>
        new(f);

    public static Flow<C, R, A> LiftIO<A>(Func<C, R, EnvIO, Task<A>> f) =>
        new((c, r) => IO.liftAsync(e => f(c, r, e)));

    public static Flow<C, R, O> Lift<O>(Func<C, R, O> f) =>
        new((c, r) => IO.pure(f(c, r)));

    public static Flow<C, R, O> Lift<O>(Func<C, R, Eff<O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    public static Flow<C, R, O> Lift<O>(Func<C, R, Eff<C, O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(c, e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    public static Flow<C, R, O> Lift<O>(Func<C, R, Fin<O>> f) =>
        new((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>));

    public static Flow<C, R, O> Lift<O>(Func<C, R, FinT<IO, O>> f) =>
        LiftIO((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>).As().Flatten());

    public static Flow<C, R, O> Lift<O>(Func<C, R, FinT<Eff, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success, Fail: Eff.Fail<O>)
                              .As().Flatten());

    public static Flow<C, R, O> Lift<O>(Func<C, R, FinT<Eff<C>, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success<C, O>, Fail: Eff.Fail<C, O>)
                              .As().Flatten());

    public static Flow<C, R, O> Lift<O>(Eff<O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<C, R, O> Lift<O>(Eff<C, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<C, R, O> Lift<O>(Fin<O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<C, R, O> Lift<O>(FinT<IO, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<C, R, O> Lift<O>(FinT<Eff, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<C, R, O> Lift<O>(FinT<Eff<C>, O> m) =>
        Lift<O>((_, _) => m);
}
