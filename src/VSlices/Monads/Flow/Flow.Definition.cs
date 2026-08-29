namespace VSlices.Monads;

/// <summary>
/// Represents a monadic liftFlow that encapsulates computations involving a runtime environment,
/// a request, and a result.
/// </summary>
/// <remarks>
/// This class provides a functional approach to chaining and
/// composing computations while maintaining immutability and type safety.
/// </remarks>
/// <typeparam name="RT">The type of the runtime environment used in the liftFlow.</typeparam>
/// <typeparam name="RQ">The type of the request input for the liftFlow.</typeparam>
/// <typeparam name="A">The type of the result produced by the liftFlow. Must be non-null.</typeparam>
public sealed partial class Flow<RT, RQ, A>(
    Func<RT, RQ, IO<A>> run)
    : K<Flow<RT, RQ>, A>
{
    public Flow(Func<RT, IO<A>> run) 
        : this((rt, _) => run(rt)) { }
    
    public Flow(Func<IO<A>> run)
        : this((_, _) => run()) { }
    
    public Flow(IO<A> run)
        : this((_, _) => run) { }

    /// <summary>
    /// Executes the liftFlow with the provided runtime environment and request,
    /// producing an <see cref="IO{T}"/> result.
    /// </summary>
    /// <param name="state">The runtime environment used to execute the liftFlow.</param>
    /// <param name="request">The request input for the liftFlow.</param>
    /// <returns>An <see cref="IO{T}"/> instance containing the result of the liftFlow execution.</returns>
    public IO<A> RunFlow(RT state, RQ request) =>
        run(state, request);

    /// <summary>
    /// Executes the liftFlow as an effect with the provided request input,
    /// producing an <see cref="Eff{RT, A}"/> result.
    /// </summary>
    /// <param name="input">The request input for the liftFlow.</param>
    /// <returns>An <see cref="Eff{RT, A}"/> instance representing the effectful computation
    /// with the provided request input.</returns>
    public Eff<RT, A> RunEff(RQ input) =>
        Eff<RT, A>.LiftIO(state => run(state, input));

    /// <summary>
    /// Converts a pure result value into a liftFlow that produces the value
    /// without depending on the runtime environment or request input.
    /// </summary>
    /// <param name="a">The pure value to convert.</param>
    /// <returns>A liftFlow that produces the provided pure value.</returns>
    public static implicit operator Flow<RT, RQ, A>(Pure<A> a) =>
        Flow<RT, RQ>.Pure(a);

    /// <summary>
    /// Converts a failure into a liftFlow that produces the specified error
    /// without executing a successful computation.
    /// </summary>
    /// <param name="a">The failure value to convert.</param>
    /// <returns>A liftFlow representing the provided failure.</returns>
    public static implicit operator Flow<RT, RQ, A>(Fail<Error> a) =>
        Flow<RT, RQ>.Fail<A>(a);
    
}
