// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<RT, REQ, RES>(K<Flow<RT, REQ>, RES> ma)
    {
        public IO<RES> RunFlow(RT state, REQ request) =>
            ma.As().RunFlow(state, request);

        public Fin<RES> Run(RT state, REQ input, EnvIO env) =>
            ma.RunFlow(state, input).RunSafe(env);

        public async Task<Fin<RES>> RunAsync(RT state, REQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunSafeAsync(env);

        public RES RunUnsafe(RT state, REQ input, EnvIO env) =>
            ma.RunFlow(state, input).Run(env);

        public async Task<RES> RunUnsafeAsync(RT state, REQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunAsync(env);
    }
}