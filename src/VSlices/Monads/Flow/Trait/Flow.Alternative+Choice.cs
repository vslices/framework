namespace VSlices.Monads;

public partial class Flow<ALG, RQ>
{
    static K<Flow<ALG, RQ>, A> Choice<Flow<ALG, RQ>>.Choose<A>(K<Flow<ALG, RQ>, A> fa, K<Flow<ALG, RQ>, A> fb) =>
        new Flow<ALG, RQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.RunFlow(s, r)));
    
    /// <summary>
    /// Chooses between two flows, returning the result of the first liftFlow if it succeeds,
    /// or the result of the second liftFlow if the first one fails.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the flows.</typeparam>
    /// <param name="fa">The first liftFlow to be executed.</param>
    /// <param name="fb">The second liftFlow to be executed if the first one fails.</param>
    /// <returns>A new liftFlow that represents the choice between the two provided flows.</returns>
    public static Flow<ALG, RQ, A> Choose<A>(
        K<Flow<ALG, RQ>, A> fa,
        K<Flow<ALG, RQ>, A> fb) =>
        +Choice.choose(fa, fb);

    static K<Flow<ALG, RQ>, A> Choice<Flow<ALG, RQ>>.Choose<A>(K<Flow<ALG, RQ>, A> fa, Memo<Flow<ALG, RQ>, A> fb) =>
        new Flow<ALG, RQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.Value.RunFlow(s, r)));

    static K<Flow<ALG, RQ>, A> Alternative<Flow<ALG, RQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    /// <summary>
    /// Creates an empty <see cref="Flow{ALG, RQ, A}"/> instance, representing the identity element
    /// for the alternative composition of flows.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the liftFlow.</typeparam>
    /// <returns>An empty <see cref="Flow{ALG, RQ, A}"/> instance.</returns>
    public static Flow<ALG, RQ, A> Empty<A>() =>
        +Alternative.empty<Flow<ALG, RQ>, A>();
}
