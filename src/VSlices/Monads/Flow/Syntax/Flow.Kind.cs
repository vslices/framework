using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ForkExt;
using ForkExt.Common;
using ForkExt.Traits;
using ForkExt.Effects;
using ForkExt.Traits.Domain;
using static ForkExt.Prelude;
using VSlices;
using VSlices.Monads;
using static VSlices.VSlicesPrelude;
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<ALG, RQ, A> As() =>
            (Flow<ALG, RQ, A>)ma;


        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public IO<A> RunFlow(ALG state, RQ request) =>
            ma.As().RunFlow(state, request);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public Fin<A> Run(ALG state, RQ input, EnvIO env) =>
            ma.RunFlow(state, input).RunSafe(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<Fin<A>> RunAsync(ALG state, RQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunSafeAsync(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public A RunUnsafe(ALG state, RQ input, EnvIO env) =>
            ma.RunFlow(state, input).Run(env);

        /// <summary>
        ///
        /// </summary>
        /// <param name="state"></param>
        /// <param name="input"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public async Task<A> RunUnsafeAsync(ALG state, RQ input, EnvIO env) =>
            await ma.RunFlow(state, input).RunAsync(env);
    }

    extension<ALG, RQ, A>(K<Flow<ALG, RQ>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public static Flow<ALG, RQ, A> operator +(K<Flow<ALG, RQ>, A> mx) =>
            mx.As();
    }
}

