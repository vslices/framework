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

namespace VSlices.Monads;

public partial class Flow<ALG, RQ>
{
    static K<Flow<ALG, RQ>, A> SemigroupK<Flow<ALG, RQ>>.Combine<A>(
        K<Flow<ALG, RQ>, A> lhs, K<Flow<ALG, RQ>, A> rhs) =>
        lhs | @catch(e1 => rhs | @catch(e2 => Fail<A>(e1 + e2)));

    static K<Flow<ALG, RQ>, A> MonoidK<Flow<ALG, RQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    /// <summary>
    /// Combines two flows into a single liftFlow by applying a semigroup operation.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the flows.</typeparam>
    /// <param name="mx">The first liftFlow to combine.</param>
    /// <param name="my">The second liftFlow to combine.</param>
    /// <returns>A new liftFlow that represents the combination of the two input flows.</returns>
    public static Flow<ALG, RQ, A> Combine<A>(
        K<Flow<ALG, RQ>, A> mx,
        K<Flow<ALG, RQ>, A> my) =>
        +SemigroupK.combine(mx, my);
}
