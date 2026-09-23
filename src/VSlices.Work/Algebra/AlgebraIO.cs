namespace VSlices.Work;

/// <summary>
/// Interprets one operation from a service-owned algebra into IO.
/// </summary>
/// <typeparam name="ALG">The algebra being interpreted.</typeparam>
public interface AlgebraIO<ALG>
    where ALG : Functor<ALG>
{
    IO<A> Interpret<A>(K<ALG, A> operation);
}

/// <summary>
/// Declares that a runtime can interpret the specified algebra.
/// </summary>
public interface HasAlgebra<ALG, RT> : Has<Eff<RT>, AlgebraIO<ALG>>
    where ALG : Functor<ALG>;

/// <summary>
/// Functions for interpreting free programs over an algebra.
/// </summary>
public static class Algebra
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

            _ => throw new NotSupportedException(
                $"Unknown Free<{typeof(ALG).Name}> program node.")
        };
}

/// <summary>
/// Runtime access to algebra interpretation.
/// </summary>
public static class AlgebraEnv<ALG, RT>
    where ALG : Functor<ALG>
    where RT : HasAlgebra<ALG, RT>
{
    private static Eff<RT, AlgebraIO<ALG>> accessIO =>
        Has<Eff<RT>, RT, AlgebraIO<ALG>>.ask.As();

    public static Eff<RT, A> run<A>(K<Free<ALG>, A> program) =>
        accessIO.Bind(interpreter => Algebra.interpret(program, interpreter));
}
