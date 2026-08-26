using VSlices.Monads;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface Feature<F, REQ, RES>
    where F : Feature<F, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract string Name { get; }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <returns></returns>
    static abstract Flow<RT, REQ, RES> Get<RT>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="runtime"></param>
    /// <param name="envIO"></param>
    /// <returns></returns>
    static virtual Fin<RES> Run<RT>(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get<RT>().Run(runtime, input, envIO);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <param name="input"></param>
    /// <param name="runtime"></param>
    /// <param name="envIO"></param>
    /// <returns></returns>
    static virtual RES RunUnsafe<RT>(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get<RT>().RunUnsafe(runtime, input, envIO);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <param name="input"></param>
    /// <param name="runtime"></param>
    /// <param name="envIO"></param>
    /// <returns></returns>
    static virtual Task<Fin<RES>> RunAsync<RT>(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get<RT>().RunAsync(runtime, input, envIO);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <param name="input"></param>
    /// <param name="runtime"></param>
    /// <param name="envIO"></param>
    /// <returns></returns>
    static virtual Task<RES> RunUnsafeAsync<RT>(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get<RT>().RunUnsafeAsync(runtime, input, envIO);
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
public interface Feature<F, REQ> : Feature<F, REQ, Unit>
    where F : Feature<F, REQ>;
