namespace VSlices.Monads;

/// <summary>
/// Represents a monadic flow that encapsulates computations involving a runtime environment,
/// a request, and a result.
/// </summary>
/// <remarks>
/// This class provides a functional approach to chaining and
/// composing computations while maintaining immutability and type safety.
/// </remarks>
/// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
/// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
/// <typeparam name="RES">The type of the result produced by the flow. Must be non-null.</typeparam>
public sealed partial class Flow<RT, REQ, RES>(
    Func<RT, REQ, IO<RES>> run)
    : K<Flow<RT, REQ>, RES>
{
    /// <summary>
    /// Executes the flow with the provided runtime environment and request,
    /// producing an <see cref="IO{T}"/> result.
    /// </summary>
    /// <param name="state">The runtime environment used to execute the flow.</param>
    /// <param name="request">The request input for the flow.</param>
    /// <returns>An <see cref="IO{T}"/> instance containing the result of the flow execution.</returns>
    public IO<RES> RunFlow(RT state, REQ request) =>
        run(state, request);

    /// <summary>
    /// Executes the flow as an effect with the provided request input,
    /// producing an <see cref="Eff{RT, RES}"/> result.
    /// </summary>
    /// <param name="input">The request input for the flow.</param>
    /// <returns>An <see cref="Eff{RT, RES}"/> instance representing the effectful computation
    /// with the provided request input.</returns>
    public Eff<RT, RES> RunEff(REQ input) =>
        Eff<RT, RES>.LiftIO(state => run(state, input));

    /// <summary>
    ///
    /// </summary>
    /// <param name="a">The pure value to convert.</param>
    /// <returns>The converted flow instance.</returns>
    public static implicit operator Flow<RT, REQ, RES>(Pure<RES> a) =>
        Flow<RT, REQ>.Pure(a);

    /// <summary>
    ///
    /// </summary>
    /// <param name="a">The fail value to convert.</param>
    /// <returns>The converted flow instance.</returns>
    public static implicit operator Flow<RT, REQ, RES>(Fail<Error> a) =>
        Flow<RT, REQ>.Fail<RES>(a);

}
