using VSlices.Monads;

namespace VSlices;

public static class FlowTransformSyntax
{
    /// <summary>
    /// Adapts a Flow to a larger or different runtime by projecting the
    /// runtime required by the child Flow.
    /// </summary>
    public static Flow<RT2, RQ, A> MapRuntime<RT, RQ, A, RT2>(
        this Flow<RT, RQ, A> flow,
        Func<RT2, RT> map) =>
        new((runtime, request) =>
            flow.RunFlow(map(runtime), request));

    /// <summary>
    /// Adapts a Flow to a different request by mapping the outer request
    /// into the request required by the child Flow.
    /// </summary>
    public static Flow<RT, RQ2, A> MapRequest<RT, RQ, A, RQ2>(
        this Flow<RT, RQ, A> flow,
        Func<RQ2, RQ> map) =>
        new((runtime, request) =>
            flow.RunFlow(runtime, map(request)));

    /// <summary>
    /// Adapts both runtime and request while preserving the child result.
    /// </summary>
    public static Flow<RT2, RQ2, A> ContraMap<RT, RQ, A, RT2, RQ2>(
        this Flow<RT, RQ, A> flow,
        Func<RT2, RT> mapRuntime,
        Func<RQ2, RQ> mapRequest) =>
        new((runtime, request) =>
            flow.RunFlow(
                mapRuntime(runtime),
                mapRequest(request)));
}
