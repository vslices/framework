namespace VSlices.Monads;

public partial class Flow<RT, RQ>
{
    static K<Flow<RT, RQ>, A> Choice<Flow<RT, RQ>>.Choose<A>(K<Flow<RT, RQ>, A> fa, K<Flow<RT, RQ>, A> fb) =>
        new Flow<RT, RQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.RunFlow(s, r)));
    
    /// <summary>
    /// Chooses between two flows, returning the result of the first liftFlow if it succeeds,
    /// or the result of the second liftFlow if the first one fails.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the flows.</typeparam>
    /// <param name="fa">The first liftFlow to be executed.</param>
    /// <param name="fb">The second liftFlow to be executed if the first one fails.</param>
    /// <returns>A new liftFlow that represents the choice between the two provided flows.</returns>
    public static Flow<RT, RQ, A> Choose<A>(
        K<Flow<RT, RQ>, A> fa,
        K<Flow<RT, RQ>, A> fb) =>
        +Choice.choose(fa, fb);

    static K<Flow<RT, RQ>, A> Choice<Flow<RT, RQ>>.Choose<A>(K<Flow<RT, RQ>, A> fa, Memo<Flow<RT, RQ>, A> fb) =>
        new Flow<RT, RQ, A>(
            (s, r) => +fa.RunFlow(s, r) | @catch(_ => fb.Value.RunFlow(s, r)));

    static K<Flow<RT, RQ>, A> Alternative<Flow<RT, RQ>>.Empty<A>() =>
        Fail<A>(Error.Empty);

    /// <summary>
    /// Creates an empty <see cref="Flow{RT, RQ, A}"/> instance, representing the identity element
    /// for the alternative composition of flows.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the liftFlow.</typeparam>
    /// <returns>An empty <see cref="Flow{RT, RQ, A}"/> instance.</returns>
    public static Flow<RT, RQ, A> Empty<A>() =>
        +Alternative.empty<Flow<RT, RQ>, A>();
}
