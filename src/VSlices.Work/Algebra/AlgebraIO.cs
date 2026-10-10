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


/// <summary>
/// Experimental interpretation of a FreeT Work program whose base monad is Fin.
///
/// Semantic failure remains a Fin value while Grounding effects remain IO.
/// </summary>
public static class FreeTAlgebra
{
    public static IO<Fin<A>> interpret<ALG, A>(
        FreeT<ALG, Fin, A> program,
        AlgebraIO<ALG> interpreter)
        where ALG : Functor<ALG>
    {
        var layer = program.runFreeT.As();

        return layer.Match(
            Succ: step =>
                step switch
                {
                    FreeTPure<ALG, Fin, A>(var value) =>
                        IO.pure(Fin.Succ(value)),

                    FreeTSuspend<ALG, Fin, A>(var operation) =>
                        interpreter
                            .Interpret(operation)
                            .Bind(next => interpret(next, interpreter)),

                    _ => throw new NotSupportedException()
                },
            Fail: error =>
                IO.pure(Fin.Fail<A>(error)));
    }
}
