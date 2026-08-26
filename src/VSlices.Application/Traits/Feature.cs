using VSlices.Monads;

namespace VSlices.Application;


/// <summary>
/// Represents a feature that defines a contract for implementing a specific functionality
/// with a runtime environment, request, and response types.
/// </summary>
/// <remarks>
/// This trait is designed to be implemented by concrete feature classes, it is the base for every functionality
/// inside a system that uses VSlices
/// </remarks>
/// <typeparam name="F">
/// The type of the feature implementation. Must inherit from <see cref="Feature{F, RT, REQ, RES}"/>.
/// </typeparam>
/// <typeparam name="RT">The type of the runtime environment required for the feature.</typeparam>
/// <typeparam name="REQ">The type of the request input for the feature.</typeparam>
/// <typeparam name="RES">The type of the response output produced by the feature.</typeparam>
public interface Feature<F, RT, REQ, RES>
    where F : Feature<F, RT, REQ, RES>
{
    /// <summary>
    /// Gets the name of the feature.
    /// </summary>
    /// <remarks>
    /// This property provides a unique identifier or descriptive name for the feature.
    /// It is intended to be implemented by concrete feature classes to specify their name.
    /// </remarks>
    static abstract string Name { get; }
    

    /// <summary>
    /// Retrieves the flow definition for the feature.
    /// </summary>
    /// <remarks>
    /// This method provides the core flow logic for the feature, defining how the runtime environment,
    /// request, and response types interact.
    /// </remarks>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance representing the flow logic for the feature.
    /// </returns>
    static abstract Flow<RT, REQ, RES> Get();

    /// <summary>
    /// Executes the feature's flow logic using the provided input, runtime environment, and environment I/O.
    /// </summary>
    /// <param name="input">The request input for the feature.</param>
    /// <param name="runtime">The runtime environment required for the feature.</param>
    /// <param name="envIO">The environment I/O used to execute the flow.</param>
    /// <returns>
    /// A <see cref="Fin{RES}"/> instance representing the result of the feature's flow execution.
    /// </returns>
    /// <remarks>
    /// This method provides a default implementation for running the feature's flow logic safely.
    /// It utilizes the flow definition provided by the feature implementation.
    /// </remarks>
    static virtual Fin<RES> Run(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().Run(runtime, input, envIO);

    /// <summary>
    /// Executes the feature's flow logic in an unsafe manner, bypassing safety mechanisms.
    /// </summary>
    /// <remarks>
    /// This method directly runs the feature's flow logic without encapsulating the result in a
    /// <see cref="Fin{T}"/> or other safety constructs. It is intended for scenarios where the
    /// caller is confident that the operation will succeed and does not require additional safety checks.
    /// </remarks>
    /// <param name="input">The input request for the feature.</param>
    /// <param name="runtime">The runtime environment required for the feature.</param>
    /// <param name="envIO">The environment input/output context for executing the flow.</param>
    /// <returns>The result of the feature's flow logic execution.</returns>
    /// <exception cref="Exception">
    /// Any exception that occurs during the execution of the flow logic will propagate to the caller.
    /// </exception>
    static virtual RES RunUnsafe(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafe(runtime, input, envIO);

    /// <summary>
    /// Executes the feature asynchronously using the provided input, runtime environment, and environment I/O.
    /// </summary>
    /// <param name="input">The request input for the feature.</param>
    /// <param name="runtime">The runtime environment required for the feature.</param>
    /// <param name="envIO">The environment I/O used during execution.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a <see cref="Fin{T}"/>
    /// representing the outcome of the feature execution, which can either be a success or a failure.
    /// </returns>
    /// <remarks>
    /// This method provides an asynchronous execution flow for the feature, ensuring non-blocking operations
    /// and allowing integration with asynchronous workflows.
    /// </remarks>
    static virtual Task<Fin<RES>> RunAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunAsync(runtime, input, envIO);

    /// <summary>
    /// Executes the feature's flow logic asynchronously in an unsafe manner.
    /// </summary>
    /// <remarks>
    /// This method runs the feature's flow logic without enforcing safety checks,
    /// which may result in exceptions if the flow encounters errors.
    /// </remarks>
    /// <param name="input">The input request for the feature.</param>
    /// <param name="runtime">The runtime environment required to execute the feature.</param>
    /// <param name="envIO">The environment input/output context for the execution.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing the response output produced by the feature.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown if an error occurs during the execution of the feature's flow logic.
    /// </exception>
    static virtual Task<RES> RunUnsafeAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(runtime, input, envIO);
}

/// <summary>
/// Represents a feature interface that defines a contract for implementing a specific functionality
/// with a runtime environment and request type.
/// </summary>
/// <remarks>
/// This interface is a specialized version of <see cref="Feature{F, RT, REQ, RES}"/> where the response type is <see cref="Unit"/>.
/// It is designed to be implemented by concrete feature classes, serving as the base for functionalities
/// that do not produce a specific response output.
/// </remarks>
/// <typeparam name="F">
/// The type of the feature implementation. Must inherit from <see cref="Feature{F, RT, REQ}"/>.
/// </typeparam>
/// <typeparam name="RT">The type of the runtime environment required for the feature.</typeparam>
/// <typeparam name="REQ">The type of the request input for the feature.</typeparam>
public interface Feature<F, RT, REQ> : Feature<F, RT, REQ, Unit>
    where F : Feature<F, RT, REQ>;
