namespace VSlices.Work;

public abstract record AlgebraSumOperation<L, R, A> : K<AlgebraSum<L, R>, A>
    where L : Functor<L>
    where R : Functor<R>;

public sealed record LeftAlgebraOperation<L, R, A>(K<L, A> Value)
    : AlgebraSumOperation<L, R, A>
    where L : Functor<L>
    where R : Functor<R>;

public sealed record RightAlgebraOperation<L, R, A>(K<R, A> Value)
    : AlgebraSumOperation<L, R, A>
    where L : Functor<L>
    where R : Functor<R>;

public sealed class AlgebraSum<L, R> : Functor<AlgebraSum<L, R>>
    where L : Functor<L>
    where R : Functor<R>
{
    static K<AlgebraSum<L, R>, B> Functor<AlgebraSum<L, R>>.Map<A, B>(
        Func<A, B> f,
        K<AlgebraSum<L, R>, A> ma) =>
        ma switch
        {
            LeftAlgebraOperation<L, R, A>(var value) =>
                new LeftAlgebraOperation<L, R, B>(value.Map(f)),
            RightAlgebraOperation<L, R, A>(var value) =>
                new RightAlgebraOperation<L, R, B>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectLeft<L, R> : Natural<L, AlgebraSum<L, R>>
    where L : Functor<L>
    where R : Functor<R>
{
    public static K<AlgebraSum<L, R>, A> Transform<A>(K<L, A> value) =>
        new LeftAlgebraOperation<L, R, A>(value);
}

public readonly struct InjectRight<L, R> : Natural<R, AlgebraSum<L, R>>
    where L : Functor<L>
    where R : Functor<R>
{
    public static K<AlgebraSum<L, R>, A> Transform<A>(K<R, A> value) =>
        new RightAlgebraOperation<L, R, A>(value);
}

public sealed class AlgebraSumIO<L, R>(
    AlgebraIO<L> left,
    AlgebraIO<R> right)
    : AlgebraIO<AlgebraSum<L, R>>
    where L : Functor<L>
    where R : Functor<R>
{
    public IO<A> Interpret<A>(K<AlgebraSum<L, R>, A> operation) =>
        operation switch
        {
            LeftAlgebraOperation<L, R, A>(var value) => left.Interpret(value),
            RightAlgebraOperation<L, R, A>(var value) => right.Interpret(value),
            _ => throw new NotSupportedException()
        };
}
