using VSlices.Monads;

namespace VSlices.Work;

/// <summary>
/// Represents an executable feature with an executable algebra, request, and response contract.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="ALG">The executable algebra required to run the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
/// <remarks>
/// Concrete features should declare their request and response as nested
/// <c>Request</c> and <c>Response</c> types and bind those types here.
/// C# does not currently support associated types directly, so the generic
/// parameters preserve compile-time enforcement while the nested types provide
/// the canonical nominal contract.
/// </remarks>
public interface Feature<F, ALG, REQ, RES>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Flow<ALG, REQ, RES> Get();

    static virtual Fin<RES> Run(
        REQ input,
        ALG algebra,
        EnvIO envIO) =>
        F.Get().Run(algebra, input, envIO);

    static virtual RES RunUnsafe(
        REQ input,
        ALG algebra,
        EnvIO envIO) =>
        F.Get().RunUnsafe(algebra, input, envIO);

    static virtual Task<Fin<RES>> RunAsync(
        REQ input,
        ALG algebra,
        EnvIO envIO) =>
        F.Get().RunAsync(algebra, input, envIO);

    static virtual Task<RES> RunUnsafeAsync(
        REQ input,
        ALG algebra,
        EnvIO envIO) =>
        F.Get().RunUnsafeAsync(algebra, input, envIO);
}
