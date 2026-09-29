namespace VSlices.Monads;

/// <summary>
/// Represents a monadic liftFlow that encapsulates computations involving a executable algebra,
/// a request, and a result.
/// </summary>
/// <remarks>
/// This class provides a functional approach to chaining and
/// composing computations while maintaining immutability and type safety.
/// </remarks>
/// <typeparam name="ALG">The type of the executable algebra used in the liftFlow.</typeparam>
/// <typeparam name="RQ">The type of the request input for the liftFlow.</typeparam>
/// <typeparam name="A">The type of the result produced by the liftFlow. Must be non-null.</typeparam>
public sealed partial class Flow<ALG, RQ, A>(
    Func<ALG, RQ, IO<A>> run)
    : K<Flow<ALG, RQ>, A>
{
    public Flow(Func<ALG, IO<A>> run) 
        : this((rt, _) => run(rt)) { }
    
    public Flow(Func<IO<A>> run)
        : this((_, _) => run()) { }
    
    public Flow(IO<A> run)
        : this((_, _) => run) { }

    /// <summary>
    /// Executes the liftFlow with the provided executable algebra and request,
    /// producing an <see cref="IO{T}"/> result.
    /// </summary>
    /// <param name="state">The executable algebra used to execute the liftFlow.</param>
    /// <param name="request">The request input for the liftFlow.</param>
    /// <returns>An <see cref="IO{T}"/> instance containing the result of the liftFlow execution.</returns>
    public IO<A> RunFlow(ALG state, RQ request) =>
        run(state, request);

    /// <summary>
    /// Executes the liftFlow as an effect with the provided request input,
    /// producing an <see cref="Eff{ALG, A}"/> result.
    /// </summary>
    /// <param name="input">The request input for the liftFlow.</param>
    /// <returns>An <see cref="Eff{ALG, A}"/> instance representing the effectful computation
    /// with the provided request input.</returns>
    public Eff<ALG, A> RunEff(RQ input) =>
        Eff<ALG, A>.LiftIO(state => run(state, input));

    /// <summary>
    /// Converts a pure result value into a liftFlow that produces the value
    /// without depending on the executable algebra or request input.
    /// </summary>
    /// <param name="a">The pure value to convert.</param>
    /// <returns>A liftFlow that produces the provided pure value.</returns>
    public static implicit operator Flow<ALG, RQ, A>(Pure<A> a) =>
        Flow<ALG, RQ>.Pure(a);

    /// <summary>
    /// Converts a failure into a liftFlow that produces the specified error
    /// without executing a successful computation.
    /// </summary>
    /// <param name="a">The failure value to convert.</param>
    /// <returns>A liftFlow representing the provided failure.</returns>
    public static implicit operator Flow<ALG, RQ, A>(Fail<Error> a) =>
        Flow<ALG, RQ>.Fail<A>(a);
    
}
