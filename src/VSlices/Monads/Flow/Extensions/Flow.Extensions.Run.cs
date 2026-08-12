// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<RT, REQ, RES>(K<Flow<RT, REQ>, RES> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IO<RES> RunFlow(RT state, REQ request) =>
            ma.As().RunFlow(state, request);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public Fin<RES> Run(RT state, REQ input, EnvIO env) =>
            ma.RunFlow(state, input).RunSafe(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<Fin<RES>> RunAsync(RT state, REQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunSafeAsync(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public RES RunUnsafe(RT state, REQ input, EnvIO env) =>
            ma.RunFlow(state, input).Run(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<RES> RunUnsafeAsync(RT state, REQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunAsync(env);
    }
}
