namespace VSlices.Work;

/// <summary>
/// Sum algebras compose independent WorkFlow vocabularies without making the
/// child algebras know the Feature that embeds them.
/// </summary>
public abstract record AlgebraSumOperation<A, B, T> : K<AlgebraSum<A, B>, T>
    where A : Functor<A>
    where B : Functor<B>;

public sealed record AlgebraAOperation<A, B, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, T>
    where A : Functor<A>
    where B : Functor<B>;

public sealed record AlgebraBOperation<A, B, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, T>
    where A : Functor<A>
    where B : Functor<B>;

public sealed class AlgebraSum<A, B> : Functor<AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public static Free<AlgebraSum<A, B>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B>,
            A,
            AlgebraSum<A, B>,
            T>(program);

    public static Free<AlgebraSum<A, B>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B>,
            B,
            AlgebraSum<A, B>,
            T>(program);

    static K<AlgebraSum<A, B>, Y> Functor<AlgebraSum<A, B>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, X>(var value) =>
                new AlgebraAOperation<A, B, Y>(value.Map(f)),
            AlgebraBOperation<A, B, X>(var value) =>
                new AlgebraBOperation<A, B, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B> : Natural<A, AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public static K<AlgebraSum<A, B>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, T>(value);
}

public readonly struct InjectB<A, B> : Natural<B, AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public static K<AlgebraSum<A, B>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, T>(value);
}

public sealed class AlgebraSumIO<A, B>(
    AlgebraIO<A> a,
    AlgebraIO<B> b)
    : AlgebraIO<AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, T>(var value) =>
                b.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

public abstract record AlgebraSumOperation<A, B, C, T> : K<AlgebraSum<A, B, C>, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>;

public sealed record AlgebraAOperation<A, B, C, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, C, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>;

public sealed record AlgebraBOperation<A, B, C, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, C, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>;

public sealed record AlgebraCOperation<A, B, C, T>(K<C, T> Value)
    : AlgebraSumOperation<A, B, C, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>;

public sealed class AlgebraSum<A, B, C> : Functor<AlgebraSum<A, B, C>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
{
    public static Free<AlgebraSum<A, B, C>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B, C>,
            A,
            AlgebraSum<A, B, C>,
            T>(program);

    public static Free<AlgebraSum<A, B, C>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B, C>,
            B,
            AlgebraSum<A, B, C>,
            T>(program);

    public static Free<AlgebraSum<A, B, C>, T> FromC<T>(Free<C, T> program) =>
        FreeAlgebra.hoist<
            InjectC<A, B, C>,
            C,
            AlgebraSum<A, B, C>,
            T>(program);

    static K<AlgebraSum<A, B, C>, Y> Functor<AlgebraSum<A, B, C>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B, C>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, C, X>(var value) =>
                new AlgebraAOperation<A, B, C, Y>(value.Map(f)),
            AlgebraBOperation<A, B, C, X>(var value) =>
                new AlgebraBOperation<A, B, C, Y>(value.Map(f)),
            AlgebraCOperation<A, B, C, X>(var value) =>
                new AlgebraCOperation<A, B, C, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B, C> : Natural<A, AlgebraSum<A, B, C>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
{
    public static K<AlgebraSum<A, B, C>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, C, T>(value);
}

public readonly struct InjectB<A, B, C> : Natural<B, AlgebraSum<A, B, C>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
{
    public static K<AlgebraSum<A, B, C>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, C, T>(value);
}

public readonly struct InjectC<A, B, C> : Natural<C, AlgebraSum<A, B, C>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
{
    public static K<AlgebraSum<A, B, C>, T> Transform<T>(K<C, T> value) =>
        new AlgebraCOperation<A, B, C, T>(value);
}

public sealed class AlgebraSumIO<A, B, C>(
    AlgebraIO<A> a,
    AlgebraIO<B> b,
    AlgebraIO<C> c)
    : AlgebraIO<AlgebraSum<A, B, C>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B, C>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, C, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, C, T>(var value) =>
                b.Interpret(value),
            AlgebraCOperation<A, B, C, T>(var value) =>
                c.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

public abstract record AlgebraSumOperation<A, B, C, D, T> : K<AlgebraSum<A, B, C, D>, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>;

public sealed record AlgebraAOperation<A, B, C, D, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, C, D, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>;

public sealed record AlgebraBOperation<A, B, C, D, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, C, D, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>;

public sealed record AlgebraCOperation<A, B, C, D, T>(K<C, T> Value)
    : AlgebraSumOperation<A, B, C, D, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>;

public sealed record AlgebraDOperation<A, B, C, D, T>(K<D, T> Value)
    : AlgebraSumOperation<A, B, C, D, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>;

public sealed class AlgebraSum<A, B, C, D> : Functor<AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public static Free<AlgebraSum<A, B, C, D>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B, C, D>,
            A,
            AlgebraSum<A, B, C, D>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B, C, D>,
            B,
            AlgebraSum<A, B, C, D>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D>, T> FromC<T>(Free<C, T> program) =>
        FreeAlgebra.hoist<
            InjectC<A, B, C, D>,
            C,
            AlgebraSum<A, B, C, D>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D>, T> FromD<T>(Free<D, T> program) =>
        FreeAlgebra.hoist<
            InjectD<A, B, C, D>,
            D,
            AlgebraSum<A, B, C, D>,
            T>(program);

    static K<AlgebraSum<A, B, C, D>, Y> Functor<AlgebraSum<A, B, C, D>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B, C, D>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, C, D, X>(var value) =>
                new AlgebraAOperation<A, B, C, D, Y>(value.Map(f)),
            AlgebraBOperation<A, B, C, D, X>(var value) =>
                new AlgebraBOperation<A, B, C, D, Y>(value.Map(f)),
            AlgebraCOperation<A, B, C, D, X>(var value) =>
                new AlgebraCOperation<A, B, C, D, Y>(value.Map(f)),
            AlgebraDOperation<A, B, C, D, X>(var value) =>
                new AlgebraDOperation<A, B, C, D, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B, C, D> : Natural<A, AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public static K<AlgebraSum<A, B, C, D>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, C, D, T>(value);
}

public readonly struct InjectB<A, B, C, D> : Natural<B, AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public static K<AlgebraSum<A, B, C, D>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, C, D, T>(value);
}

public readonly struct InjectC<A, B, C, D> : Natural<C, AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public static K<AlgebraSum<A, B, C, D>, T> Transform<T>(K<C, T> value) =>
        new AlgebraCOperation<A, B, C, D, T>(value);
}

public readonly struct InjectD<A, B, C, D> : Natural<D, AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public static K<AlgebraSum<A, B, C, D>, T> Transform<T>(K<D, T> value) =>
        new AlgebraDOperation<A, B, C, D, T>(value);
}

public sealed class AlgebraSumIO<A, B, C, D>(
    AlgebraIO<A> a,
    AlgebraIO<B> b,
    AlgebraIO<C> c,
    AlgebraIO<D> d)
    : AlgebraIO<AlgebraSum<A, B, C, D>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B, C, D>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, C, D, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, C, D, T>(var value) =>
                b.Interpret(value),
            AlgebraCOperation<A, B, C, D, T>(var value) =>
                c.Interpret(value),
            AlgebraDOperation<A, B, C, D, T>(var value) =>
                d.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

public abstract record AlgebraSumOperation<A, B, C, D, E, T> : K<AlgebraSum<A, B, C, D, E>, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed record AlgebraAOperation<A, B, C, D, E, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed record AlgebraBOperation<A, B, C, D, E, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed record AlgebraCOperation<A, B, C, D, E, T>(K<C, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed record AlgebraDOperation<A, B, C, D, E, T>(K<D, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed record AlgebraEOperation<A, B, C, D, E, T>(K<E, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>;

public sealed class AlgebraSum<A, B, C, D, E> : Functor<AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static Free<AlgebraSum<A, B, C, D, E>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B, C, D, E>,
            A,
            AlgebraSum<A, B, C, D, E>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B, C, D, E>,
            B,
            AlgebraSum<A, B, C, D, E>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E>, T> FromC<T>(Free<C, T> program) =>
        FreeAlgebra.hoist<
            InjectC<A, B, C, D, E>,
            C,
            AlgebraSum<A, B, C, D, E>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E>, T> FromD<T>(Free<D, T> program) =>
        FreeAlgebra.hoist<
            InjectD<A, B, C, D, E>,
            D,
            AlgebraSum<A, B, C, D, E>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E>, T> FromE<T>(Free<E, T> program) =>
        FreeAlgebra.hoist<
            InjectE<A, B, C, D, E>,
            E,
            AlgebraSum<A, B, C, D, E>,
            T>(program);

    static K<AlgebraSum<A, B, C, D, E>, Y> Functor<AlgebraSum<A, B, C, D, E>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B, C, D, E>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, C, D, E, X>(var value) =>
                new AlgebraAOperation<A, B, C, D, E, Y>(value.Map(f)),
            AlgebraBOperation<A, B, C, D, E, X>(var value) =>
                new AlgebraBOperation<A, B, C, D, E, Y>(value.Map(f)),
            AlgebraCOperation<A, B, C, D, E, X>(var value) =>
                new AlgebraCOperation<A, B, C, D, E, Y>(value.Map(f)),
            AlgebraDOperation<A, B, C, D, E, X>(var value) =>
                new AlgebraDOperation<A, B, C, D, E, Y>(value.Map(f)),
            AlgebraEOperation<A, B, C, D, E, X>(var value) =>
                new AlgebraEOperation<A, B, C, D, E, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B, C, D, E> : Natural<A, AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static K<AlgebraSum<A, B, C, D, E>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, C, D, E, T>(value);
}

public readonly struct InjectB<A, B, C, D, E> : Natural<B, AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static K<AlgebraSum<A, B, C, D, E>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, C, D, E, T>(value);
}

public readonly struct InjectC<A, B, C, D, E> : Natural<C, AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static K<AlgebraSum<A, B, C, D, E>, T> Transform<T>(K<C, T> value) =>
        new AlgebraCOperation<A, B, C, D, E, T>(value);
}

public readonly struct InjectD<A, B, C, D, E> : Natural<D, AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static K<AlgebraSum<A, B, C, D, E>, T> Transform<T>(K<D, T> value) =>
        new AlgebraDOperation<A, B, C, D, E, T>(value);
}

public readonly struct InjectE<A, B, C, D, E> : Natural<E, AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public static K<AlgebraSum<A, B, C, D, E>, T> Transform<T>(K<E, T> value) =>
        new AlgebraEOperation<A, B, C, D, E, T>(value);
}

public sealed class AlgebraSumIO<A, B, C, D, E>(
    AlgebraIO<A> a,
    AlgebraIO<B> b,
    AlgebraIO<C> c,
    AlgebraIO<D> d,
    AlgebraIO<E> e)
    : AlgebraIO<AlgebraSum<A, B, C, D, E>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B, C, D, E>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, C, D, E, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, C, D, E, T>(var value) =>
                b.Interpret(value),
            AlgebraCOperation<A, B, C, D, E, T>(var value) =>
                c.Interpret(value),
            AlgebraDOperation<A, B, C, D, E, T>(var value) =>
                d.Interpret(value),
            AlgebraEOperation<A, B, C, D, E, T>(var value) =>
                e.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

public abstract record AlgebraSumOperation<A, B, C, D, E, F, T> : K<AlgebraSum<A, B, C, D, E, F>, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraAOperation<A, B, C, D, E, F, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraBOperation<A, B, C, D, E, F, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraCOperation<A, B, C, D, E, F, T>(K<C, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraDOperation<A, B, C, D, E, F, T>(K<D, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraEOperation<A, B, C, D, E, F, T>(K<E, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed record AlgebraFOperation<A, B, C, D, E, F, T>(K<F, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>;

public sealed class AlgebraSum<A, B, C, D, E, F> : Functor<AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B, C, D, E, F>,
            A,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B, C, D, E, F>,
            B,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromC<T>(Free<C, T> program) =>
        FreeAlgebra.hoist<
            InjectC<A, B, C, D, E, F>,
            C,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromD<T>(Free<D, T> program) =>
        FreeAlgebra.hoist<
            InjectD<A, B, C, D, E, F>,
            D,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromE<T>(Free<E, T> program) =>
        FreeAlgebra.hoist<
            InjectE<A, B, C, D, E, F>,
            E,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F>, T> FromF<T>(Free<F, T> program) =>
        FreeAlgebra.hoist<
            InjectF<A, B, C, D, E, F>,
            F,
            AlgebraSum<A, B, C, D, E, F>,
            T>(program);

    static K<AlgebraSum<A, B, C, D, E, F>, Y> Functor<AlgebraSum<A, B, C, D, E, F>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B, C, D, E, F>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraAOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            AlgebraBOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraBOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            AlgebraCOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraCOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            AlgebraDOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraDOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            AlgebraEOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraEOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            AlgebraFOperation<A, B, C, D, E, F, X>(var value) =>
                new AlgebraFOperation<A, B, C, D, E, F, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B, C, D, E, F> : Natural<A, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, C, D, E, F, T>(value);
}

public readonly struct InjectB<A, B, C, D, E, F> : Natural<B, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, C, D, E, F, T>(value);
}

public readonly struct InjectC<A, B, C, D, E, F> : Natural<C, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<C, T> value) =>
        new AlgebraCOperation<A, B, C, D, E, F, T>(value);
}

public readonly struct InjectD<A, B, C, D, E, F> : Natural<D, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<D, T> value) =>
        new AlgebraDOperation<A, B, C, D, E, F, T>(value);
}

public readonly struct InjectE<A, B, C, D, E, F> : Natural<E, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<E, T> value) =>
        new AlgebraEOperation<A, B, C, D, E, F, T>(value);
}

public readonly struct InjectF<A, B, C, D, E, F> : Natural<F, AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public static K<AlgebraSum<A, B, C, D, E, F>, T> Transform<T>(K<F, T> value) =>
        new AlgebraFOperation<A, B, C, D, E, F, T>(value);
}

public sealed class AlgebraSumIO<A, B, C, D, E, F>(
    AlgebraIO<A> a,
    AlgebraIO<B> b,
    AlgebraIO<C> c,
    AlgebraIO<D> d,
    AlgebraIO<E> e,
    AlgebraIO<F> f)
    : AlgebraIO<AlgebraSum<A, B, C, D, E, F>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B, C, D, E, F>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, C, D, E, F, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, C, D, E, F, T>(var value) =>
                b.Interpret(value),
            AlgebraCOperation<A, B, C, D, E, F, T>(var value) =>
                c.Interpret(value),
            AlgebraDOperation<A, B, C, D, E, F, T>(var value) =>
                d.Interpret(value),
            AlgebraEOperation<A, B, C, D, E, F, T>(var value) =>
                e.Interpret(value),
            AlgebraFOperation<A, B, C, D, E, F, T>(var value) =>
                f.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

public abstract record AlgebraSumOperation<A, B, C, D, E, F, G, T> : K<AlgebraSum<A, B, C, D, E, F, G>, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraAOperation<A, B, C, D, E, F, G, T>(K<A, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraBOperation<A, B, C, D, E, F, G, T>(K<B, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraCOperation<A, B, C, D, E, F, G, T>(K<C, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraDOperation<A, B, C, D, E, F, G, T>(K<D, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraEOperation<A, B, C, D, E, F, G, T>(K<E, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraFOperation<A, B, C, D, E, F, G, T>(K<F, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed record AlgebraGOperation<A, B, C, D, E, F, G, T>(K<G, T> Value)
    : AlgebraSumOperation<A, B, C, D, E, F, G, T>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>;

public sealed class AlgebraSum<A, B, C, D, E, F, G> : Functor<AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromA<T>(Free<A, T> program) =>
        FreeAlgebra.hoist<
            InjectA<A, B, C, D, E, F, G>,
            A,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromB<T>(Free<B, T> program) =>
        FreeAlgebra.hoist<
            InjectB<A, B, C, D, E, F, G>,
            B,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromC<T>(Free<C, T> program) =>
        FreeAlgebra.hoist<
            InjectC<A, B, C, D, E, F, G>,
            C,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromD<T>(Free<D, T> program) =>
        FreeAlgebra.hoist<
            InjectD<A, B, C, D, E, F, G>,
            D,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromE<T>(Free<E, T> program) =>
        FreeAlgebra.hoist<
            InjectE<A, B, C, D, E, F, G>,
            E,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromF<T>(Free<F, T> program) =>
        FreeAlgebra.hoist<
            InjectF<A, B, C, D, E, F, G>,
            F,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    public static Free<AlgebraSum<A, B, C, D, E, F, G>, T> FromG<T>(Free<G, T> program) =>
        FreeAlgebra.hoist<
            InjectG<A, B, C, D, E, F, G>,
            G,
            AlgebraSum<A, B, C, D, E, F, G>,
            T>(program);

    static K<AlgebraSum<A, B, C, D, E, F, G>, Y> Functor<AlgebraSum<A, B, C, D, E, F, G>>.Map<X, Y>(
        Func<X, Y> f,
        K<AlgebraSum<A, B, C, D, E, F, G>, X> ma) =>
        ma switch
        {
            AlgebraAOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraAOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraBOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraBOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraCOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraCOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraDOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraDOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraEOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraEOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraFOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraFOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            AlgebraGOperation<A, B, C, D, E, F, G, X>(var value) =>
                new AlgebraGOperation<A, B, C, D, E, F, G, Y>(value.Map(f)),
            _ => throw new NotSupportedException()
        };
}

public readonly struct InjectA<A, B, C, D, E, F, G> : Natural<A, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<A, T> value) =>
        new AlgebraAOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectB<A, B, C, D, E, F, G> : Natural<B, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<B, T> value) =>
        new AlgebraBOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectC<A, B, C, D, E, F, G> : Natural<C, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<C, T> value) =>
        new AlgebraCOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectD<A, B, C, D, E, F, G> : Natural<D, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<D, T> value) =>
        new AlgebraDOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectE<A, B, C, D, E, F, G> : Natural<E, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<E, T> value) =>
        new AlgebraEOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectF<A, B, C, D, E, F, G> : Natural<F, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<F, T> value) =>
        new AlgebraFOperation<A, B, C, D, E, F, G, T>(value);
}

public readonly struct InjectG<A, B, C, D, E, F, G> : Natural<G, AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public static K<AlgebraSum<A, B, C, D, E, F, G>, T> Transform<T>(K<G, T> value) =>
        new AlgebraGOperation<A, B, C, D, E, F, G, T>(value);
}

public sealed class AlgebraSumIO<A, B, C, D, E, F, G>(
    AlgebraIO<A> a,
    AlgebraIO<B> b,
    AlgebraIO<C> c,
    AlgebraIO<D> d,
    AlgebraIO<E> e,
    AlgebraIO<F> f,
    AlgebraIO<G> g)
    : AlgebraIO<AlgebraSum<A, B, C, D, E, F, G>>
    where A : Functor<A>
    where B : Functor<B>
    where C : Functor<C>
    where D : Functor<D>
    where E : Functor<E>
    where F : Functor<F>
    where G : Functor<G>
{
    public IO<T> Interpret<T>(K<AlgebraSum<A, B, C, D, E, F, G>, T> operation) =>
        operation switch
        {
            AlgebraAOperation<A, B, C, D, E, F, G, T>(var value) =>
                a.Interpret(value),
            AlgebraBOperation<A, B, C, D, E, F, G, T>(var value) =>
                b.Interpret(value),
            AlgebraCOperation<A, B, C, D, E, F, G, T>(var value) =>
                c.Interpret(value),
            AlgebraDOperation<A, B, C, D, E, F, G, T>(var value) =>
                d.Interpret(value),
            AlgebraEOperation<A, B, C, D, E, F, G, T>(var value) =>
                e.Interpret(value),
            AlgebraFOperation<A, B, C, D, E, F, G, T>(var value) =>
                f.Interpret(value),
            AlgebraGOperation<A, B, C, D, E, F, G, T>(var value) =>
                g.Interpret(value),
            _ => throw new NotSupportedException()
        };
}

[Obsolete("Use InjectA<A, B>.")]
public readonly struct InjectLeft<A, B> : Natural<A, AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public static K<AlgebraSum<A, B>, T> Transform<T>(K<A, T> value) =>
        InjectA<A, B>.Transform(value);
}

[Obsolete("Use InjectB<A, B>.")]
public readonly struct InjectRight<A, B> : Natural<B, AlgebraSum<A, B>>
    where A : Functor<A>
    where B : Functor<B>
{
    public static K<AlgebraSum<A, B>, T> Transform<T>(K<B, T> value) =>
        InjectB<A, B>.Transform(value);
}
