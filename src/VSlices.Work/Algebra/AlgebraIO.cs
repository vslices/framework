namespace VSlices.Work;

public interface AlgebraIO<ALG>
    where ALG : Functor<ALG>
{
    IO<A> Interpret<A>(K<ALG, A> operation);
}

public static class FreeAlgebra
{
    public static IO<A> interpret<ALG, A>(
        K<Free<ALG>, A> program,
        AlgebraIO<ALG> interpreter)
        where ALG : Functor<ALG> =>
        (Free<ALG, A>)program switch
        {
            Pure<ALG, A>(var value) =>
                IO.pure(value),

            Bind<ALG, A>(var operation) =>
                interpreter
                    .Interpret(operation)
                    .Bind(next => interpret<ALG, A>(next, interpreter)),

            _ => throw new NotSupportedException()
        };

    public static Free<G, A> hoist<N, F, G, A>(Free<F, A> program)
        where N : Natural<F, G>
        where F : Functor<F>
        where G : Functor<G> =>
        program switch
        {
            Pure<F, A>(var value) =>
                Free.pure<G, A>(value),

            Bind<F, A>(var operation) =>
                Free.bind<G, A>(
                    N.Transform(operation)
                        .Map(next => hoist<N, F, G, A>(next))),

            _ => throw new NotSupportedException()
        };
}
