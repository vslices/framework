using VSlices.Monads;

namespace VSlices.Application;

/// <summary>
/// Represents an executable feature with a runtime, request, and response contract.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="RT">The runtime required to execute the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
/// <remarks>
/// Concrete features should declare their request and response as nested
/// <c>Request</c> and <c>Response</c> types and bind those types here.
/// C# does not currently support associated types directly, so the generic
/// parameters preserve compile-time enforcement while the nested types provide
/// the canonical nominal contract.
/// </remarks>
public interface Feature<F, RT, REQ, RES>
    where F : Feature<F, RT, REQ, RES>
{
    static abstract Flow<RT, REQ, RES> Get();

    static virtual Fin<RES> Run(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().Run(runtime, input, envIO);

    static virtual RES RunUnsafe(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafe(runtime, input, envIO);

    static virtual Task<Fin<RES>> RunAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunAsync(runtime, input, envIO);

    static virtual Task<RES> RunUnsafeAsync(
        REQ input,
        RT runtime,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(runtime, input, envIO);
}
