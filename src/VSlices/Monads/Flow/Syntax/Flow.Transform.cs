using VSlices.Monads;

namespace VSlices;

public static class FlowTransformSyntax
{
    extension<RT, RQ, A>(Flow<RT, RQ, A> flow)
    {
        /// <summary>
        /// Adapts a Flow to a larger or different runtime by projecting the
        /// runtime required by the child Flow.
        /// </summary>
        public Flow<RT2, RQ, A> MapRuntime<RT2>(
            Func<RT2, RT> map) =>
            new((runtime, request) =>
                flow.RunFlow(map(runtime), request));

        /// <summary>
        /// Adapts a Flow to a different request by mapping the outer request
        /// into the request required by the child Flow.
        /// </summary>
        public Flow<RT, RQ2, A> MapRequest<RQ2>(
            Func<RQ2, RQ> map) =>
            new((runtime, request) =>
                flow.RunFlow(runtime, map(request)));

        /// <summary>
        /// Adapts both runtime and request while preserving the child result.
        /// </summary>
        public Flow<RT2, RQ2, A> ContraMap<RT2, RQ2>(
            Func<RT2, RT> mapRuntime,
            Func<RQ2, RQ> mapRequest) =>
            new((runtime, request) =>
                flow.RunFlow(
                    mapRuntime(runtime),
                    mapRequest(request)));
    }
}
