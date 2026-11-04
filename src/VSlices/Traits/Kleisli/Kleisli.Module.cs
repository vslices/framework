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
using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Traits;

/// <summary>
/// 
/// </summary>
public static class Kleisli
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="M"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static K<F, A, B> LiftK<F, M, A, B>(Func<A, K<M, B>> f)
        where F : Kleisli<F, M>
        where M : Monad<M> =>
        F.LiftK(f);
}
