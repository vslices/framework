// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, A>(K<Flow<C, R>, A> ma)
    {
        public IO<A> RunFlow(C state, R request) =>
            ma.As().RunFlow(state, request);

        public Fin<A> Run(C state, R input, EnvIO env) =>
            ma.RunFlow(state, input).RunSafe(env);

        public async Task<Fin<A>> RunAsync(C state, R input, EnvIO env) =>
            await ma.RunFlow(state, input).RunSafeAsync(env);

        public A RunUnsafe(C state, R input, EnvIO env) =>
            ma.RunFlow(state, input).Run(env);

        public async Task<A> RunUnsafeAsync(C state, R input, EnvIO env) =>
            await ma.RunFlow(state, input).RunAsync(env);
    }
}