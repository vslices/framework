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
