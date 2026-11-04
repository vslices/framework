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

public static partial class VSlicesPrelude
{
    public static Derive<RQ, A> derive<RQ, A>(
        Func<RQ, A> derive) =>
        new(request => IO.pure(derive(request)));

    public static Compute<ALG, A> compute<ALG, A>(
        Func<ALG, IO<A>> compute) =>
        new(compute);
}
