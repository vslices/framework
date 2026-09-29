using VSlices.Monads;

namespace VSlices;

public static partial class VSlicesPrelude
{
    public static Derive<RQ, A> derive<RQ, A>(
        Func<RQ, A> derive) =>
        new(request => IO.pure(derive(request)));

    public static Compute<ALG, A> compute<ALG, A>(
        Func<ALG, IO<A>> compute) =>
        new(compute);
}
