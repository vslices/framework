using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<RT, RQ, A>(K<Flow<RT, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<RT, RQ, A> As() =>
            (Flow<RT, RQ, A>)ma;


        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IO<A> RunFlow(RT state, RQ request) =>
            ma.As().RunFlow(state, request);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public Fin<A> Run(RT state, RQ input, EnvIO env) =>
            ma.RunFlow(state, input).RunSafe(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<Fin<A>> RunAsync(RT state, RQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunSafeAsync(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public A RunUnsafe(RT state, RQ input, EnvIO env) =>
            ma.RunFlow(state, input).Run(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<A> RunUnsafeAsync(RT state, RQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunAsync(env);
    }

    extension<RT, RQ, A>(K<Flow<RT, RQ>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public static Flow<RT, RQ, A> operator +(K<Flow<RT, RQ>, A> mx) =>
            mx.As();
    }
}

