namespace VSlices.Monads;

public partial class Flow<RT, REQ>
{
    public static Flow<RT, REQ, A> LiftIO<A>(Func<RT, REQ, IO<A>> f) =>
        new(f);

    public static Flow<RT, REQ, A> LiftIO<A>(Func<RT, REQ, EnvIO, Task<A>> f) =>
        new((c, r) => IO.liftAsync(e => f(c, r, e)));

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, O> f) =>
        new((c, r) => IO.pure(f(c, r)));

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Eff<O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Eff<RT, O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(c, e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Fin<O>> f) =>
        new((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>));

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<IO, O>> f) =>
        LiftIO((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>).As().Flatten());

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<Eff, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success, Fail: Eff.Fail<O>)
                              .As().Flatten());

    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<Eff<RT>, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success<RT, O>, Fail: Eff.Fail<RT, O>)
                              .As().Flatten());

    public static Flow<RT, REQ, O> Lift<O>(Eff<O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<RT, REQ, O> Lift<O>(Eff<RT, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<RT, REQ, O> Lift<O>(Fin<O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<RT, REQ, O> Lift<O>(FinT<IO, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<RT, REQ, O> Lift<O>(FinT<Eff, O> m) =>
        Lift<O>((_, _) => m);

    public static Flow<RT, REQ, O> Lift<O>(FinT<Eff<RT>, O> m) =>
        Lift<O>((_, _) => m);
}
