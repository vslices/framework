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

public interface Kleisli<F, M> : Arrow<F>
    where F : Kleisli<F, M>
    where M : Monad<M>
{
    static abstract K<F, A, B> LiftK<A, B>(
        Func<A, K<M, B>> function);
}