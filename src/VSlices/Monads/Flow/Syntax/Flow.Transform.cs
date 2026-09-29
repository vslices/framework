using VSlices.Monads;

namespace VSlices;

public static class FlowTransformSyntax
{
    /// <summary>
    /// Adapts a Flow to a larger or different runtime by projecting the
    /// runtime required by the child Flow.
    /// </summary>
    public static Flow<ALG2, RQ, A> MapRuntime<ALG, RQ, A, ALG2>(
        this Flow<ALG, RQ, A> flow,
        Func<ALG2, ALG> map) =>
        new((runtime, request) =>
            flow.RunFlow(map(runtime), request));

    /// <summary>
    /// Adapts a Flow to a different request by mapping the outer request
    /// into the request required by the child Flow.
    /// </summary>
    public static Flow<ALG, RQ2, A> MapRequest<ALG, RQ, A, RQ2>(
        this Flow<ALG, RQ, A> flow,
        Func<RQ2, RQ> map) =>
        new((runtime, request) =>
            flow.RunFlow(runtime, map(request)));

    /// <summary>
    /// Adapts both runtime and request while preserving the child result.
    /// </summary>
    public static Flow<ALG2, RQ2, A> ContraMap<ALG, RQ, A, ALG2, RQ2>(
        this Flow<ALG, RQ, A> flow,
        Func<ALG2, ALG> mapRuntime,
        Func<RQ2, RQ> mapRequest) =>
        new((runtime, request) =>
            flow.RunFlow(
                mapRuntime(runtime),
                mapRequest(request)));
}
