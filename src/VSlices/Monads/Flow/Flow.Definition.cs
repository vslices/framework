namespace VSlices.Monads;

public sealed partial class Flow<C, R, A>(
    Func<C, R, IO<A>> run)
    : K<Flow<C, R>, A>
    where A : notnull
{
    public IO<A> RunFlow(C state, R request) =>
        run(state, request);

    public Eff<C, A> RunEff(R input) =>
        Eff<C, A>.LiftIO(state => run(state, input));

    public static implicit operator Flow<C, R, A>(Pure<A> a) =>
        Flow<C, R>.Pure(a);

    public static implicit operator Flow<C, R, A>(Fail<Error> a) =>
        Flow<C, R>.Fail<A>(a);

}
