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
namespace VSlices.Traits;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
public interface ArrowApply<F> : Arrow<F>
    where F : ArrowApply<F>
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <returns></returns>
    static abstract K<F, (K<F, I, O> Arrow, I Input), O> Apply<I, O>();

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    static virtual K<F, I, O> Bind<I, A, O>(
        K<F, I, A> first,
        Func<A, K<F, A, O>> next) =>
        F.Compose(
            first,
            F.Compose(
                F.Lift<A, (K<F, A, O> Arrow, A Input)>(
                    value => (next(value), value)),
                F.Apply<A, O>()));
}