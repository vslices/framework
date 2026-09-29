namespace VSlices.Monads;

public partial class Flow<ALG, RQ>
{    
    static K<Flow<ALG, RQ>, A> Fallible<Error, Flow<ALG, RQ>>.Fail<A>(Error error) =>
        new Flow<ALG, RQ, A>((_, _) => IO.fail<A>(error));

    static K<Flow<ALG, RQ>, A> Fallible<Error, Flow<ALG, RQ>>.Catch<A>(
        K<Flow<ALG, RQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<ALG, RQ>, A>> Fail) =>
        new Flow<ALG, RQ, A>(
            (s, r) => +fa.RunFlow(s, r)
                .Catch(e => Predicate(e) ? Fail(e).RunFlow(s, r) : IO.fail<A>(e)));

    /// <summary>
    /// Creates a failed <see cref="Flow{ALG, RQ, A}"/> instance with the specified error.
    /// </summary>
    /// <typeparam name="A">The type of the result value that the liftFlow would have produced if successful.</typeparam>
    /// <param name="e">The error that represents the failure.</param>
    /// <returns>A <see cref="Flow{ALG, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<ALG, RQ, A> Fail<A>(Error e) =>
        +Fallible.error<Flow<ALG, RQ>, A>(e);

    /// <summary>
    /// Creates a failed <see cref="Flow{ALG, RQ, A}"/> instance with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result value.</typeparam>
    /// <param name="msg">The error message describing the failure.</param>
    /// <returns>A <see cref="Flow{ALG, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<ALG, RQ, A> Fail<A>(string msg) =>
        Fail<A>(Error.New(msg));

    /// <summary>
    /// Creates a new <see cref="Flow{ALG, RQ, A}"/> instance that represents a failure.
    /// </summary>
    /// <typeparam name="A">The type of the result expected from the liftFlow.</typeparam>
    /// <param name="fe">The failure object containing an <see cref="Error"/>.</param>
    /// <returns>A <see cref="Flow{ALG, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<ALG, RQ, A> Fail<A>(Fail<Error> fe) =>
        Fail<A>(fe.Value);

    /// <summary>
    /// Creates a new <see cref="Flow{ALG, RQ, A}"/> instance that represents a failure with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result that the liftFlow would have produced if it had succeeded.</typeparam>
    /// <param name="fe">The failure object containing the error message.</param>
    /// <returns>A <see cref="Flow{ALG, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<ALG, RQ, A> Fail<A>(Fail<string> fe) =>
        Fail<A>(Error.New(fe.Value));
    
    /// <summary>
    /// Handles errors in a computation by applying a specified predicate and recovery function.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the computation.</typeparam>
    /// <param name="fa">The computation to be executed.</param>
    /// <param name="Predicate">
    /// A function that determines whether an error should be handled.
    /// Returns <c>true</c> if the error matches the condition; otherwise, <c>false</c>.
    /// </param>
    /// <param name="Fail">
    /// A function that provides an alternative computation to execute if the predicate matches the error.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{ALG, RQ, A}"/> instance that represents the result of the computation,
    /// either successfully or after applying the recovery function.
    /// </returns>
    public static Flow<ALG, RQ, A> Catch<A>(
        K<Flow<ALG, RQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<ALG, RQ>, A>> Fail) =>
        +fa.Catch(Predicate, Fail);
    
    static K<Flow<ALG, RQ>, A> Final<Flow<ALG, RQ>>.Finally<X, A>(
        K<Flow<ALG, RQ>, A> fa,
        K<Flow<ALG, RQ>, X> @finally) =>
        new Flow<ALG, RQ, A>(
            (c, r) => fa.RunFlow(c, r)
                .Finally(@finally.RunFlow(c, r)));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="X"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <param name="fa"></param>
    /// <param name="finally"></param>
    /// <returns></returns>
    public static Flow<ALG, RQ, A> Finally<X, A>(
        K<Flow<ALG, RQ>, A> fa,
        K<Flow<ALG, RQ>, X> @finally) =>
        +VFinal.Finally(fa, @finally);

    static K<Flow<ALG, RQ>, A> Readable<Flow<ALG, RQ>, (ALG, RQ)>.Asks<A>(
        Func<(ALG, RQ), A> f) =>
        new Flow<ALG, RQ, A>((s, r) => IO.pure(f((s, r))));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<ALG, RQ, A> Asks<A>(Func<RQ, ALG, A> f) =>
        +Readable.asks<Flow<ALG, RQ>, (ALG, RQ), A>(cr => f(cr.Item2, cr.Item1));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<ALG, RQ, A> Asks<A>(Func<RQ, A> f) =>
        Asks((rq, _) => f(rq));

    static K<Flow<ALG, RQ>, A> Readable<Flow<ALG, RQ>, (ALG, RQ)>.Local<A>(
        Func<(ALG, RQ), (ALG, RQ)> f,
        K<Flow<ALG, RQ>, A> ma) =>
        new Flow<ALG, RQ, A>((s, r) =>
        {
            var (newS, newR) = f((s, r));
            return ma.RunFlow(newS, newR);
        });

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="ma"></param>
    /// <returns></returns>
    public static Flow<ALG, RQ, A> Local<A>(K<Flow<ALG, RQ>, A> ma) =>
        +Readable.local<Flow<ALG, RQ>, (ALG, RQ), A>(cr => cr, ma);
}

file static class VFinal
{
    public static K<F, A> Finally<F, A, X>(K<F, A> ma, K<F, X> mx)
        where F : Final<F> =>
        F.Finally(ma, mx);
}