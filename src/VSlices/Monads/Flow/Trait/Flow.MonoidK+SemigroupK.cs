using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads;

public partial class Flow<RT, RQ>
{
    static K<Flow<RT, RQ>, A> SemigroupK<Flow<RT, RQ>>.Combine<A>(
        K<Flow<RT, RQ>, A> lhs, K<Flow<RT, RQ>, A> rhs) =>
        lhs | @catch(e1 => rhs | @catch(e2 => Fail<A>(e1 + e2)));

    static K<Flow<RT, RQ>, A> MonoidK<Flow<RT, RQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    /// <summary>
    /// Combines two flows into a single liftFlow by applying a semigroup operation.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the flows.</typeparam>
    /// <param name="mx">The first liftFlow to combine.</param>
    /// <param name="my">The second liftFlow to combine.</param>
    /// <returns>A new liftFlow that represents the combination of the two input flows.</returns>
    public static Flow<RT, RQ, A> Combine<A>(
        K<Flow<RT, RQ>, A> mx,
        K<Flow<RT, RQ>, A> my) =>
        +SemigroupK.combine(mx, my);
}
