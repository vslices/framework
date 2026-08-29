namespace VSlices.Monads;

public partial class Flow<RT, RQ>
{    
    static K<Flow<RT, RQ>, A> Fallible<Error, Flow<RT, RQ>>.Fail<A>(Error error) =>
        new Flow<RT, RQ, A>((_, _) => IO.fail<A>(error));

    static K<Flow<RT, RQ>, A> Fallible<Error, Flow<RT, RQ>>.Catch<A>(
        K<Flow<RT, RQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<RT, RQ>, A>> Fail) =>
        new Flow<RT, RQ, A>(
            (s, r) => +fa.RunFlow(s, r)
                .Catch(e => Predicate(e) ? Fail(e).RunFlow(s, r) : IO.fail<A>(e)));

    /// <summary>
    /// Creates a failed <see cref="Flow{RT, RQ, A}"/> instance with the specified error.
    /// </summary>
    /// <typeparam name="A">The type of the result value that the liftFlow would have produced if successful.</typeparam>
    /// <param name="e">The error that represents the failure.</param>
    /// <returns>A <see cref="Flow{RT, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, RQ, A> Fail<A>(Error e) =>
        +Fallible.error<Flow<RT, RQ>, A>(e);

    /// <summary>
    /// Creates a failed <see cref="Flow{RT, RQ, A}"/> instance with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result value.</typeparam>
    /// <param name="msg">The error message describing the failure.</param>
    /// <returns>A <see cref="Flow{RT, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, RQ, A> Fail<A>(string msg) =>
        Fail<A>(Error.New(msg));

    /// <summary>
    /// Creates a new <see cref="Flow{RT, RQ, A}"/> instance that represents a failure.
    /// </summary>
    /// <typeparam name="A">The type of the result expected from the liftFlow.</typeparam>
    /// <param name="fe">The failure object containing an <see cref="Error"/>.</param>
    /// <returns>A <see cref="Flow{RT, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, RQ, A> Fail<A>(Fail<Error> fe) =>
        Fail<A>(fe.Value);

    /// <summary>
    /// Creates a new <see cref="Flow{RT, RQ, A}"/> instance that represents a failure with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result that the liftFlow would have produced if it had succeeded.</typeparam>
    /// <param name="fe">The failure object containing the error message.</param>
    /// <returns>A <see cref="Flow{RT, RQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, RQ, A> Fail<A>(Fail<string> fe) =>
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
    /// A new <see cref="Flow{RT, RQ, A}"/> instance that represents the result of the computation,
    /// either successfully or after applying the recovery function.
    /// </returns>
    public static Flow<RT, RQ, A> Catch<A>(
        K<Flow<RT, RQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<RT, RQ>, A>> Fail) =>
        +fa.Catch(Predicate, Fail);
    
    static K<Flow<RT, RQ>, A> Final<Flow<RT, RQ>>.Finally<X, A>(
        K<Flow<RT, RQ>, A> fa,
        K<Flow<RT, RQ>, X> @finally) =>
        new Flow<RT, RQ, A>(
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
    public static Flow<RT, RQ, A> Finally<X, A>(
        K<Flow<RT, RQ>, A> fa,
        K<Flow<RT, RQ>, X> @finally) =>
        +VFinal.Finally(fa, @finally);

    static K<Flow<RT, RQ>, A> Readable<Flow<RT, RQ>, (RT, RQ)>.Asks<A>(
        Func<(RT, RQ), A> f) =>
        new Flow<RT, RQ, A>((s, r) => IO.pure(f((s, r))));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, RQ, A> Asks<A>(Func<RQ, RT, A> f) =>
        +Readable.asks<Flow<RT, RQ>, (RT, RQ), A>(cr => f(cr.Item2, cr.Item1));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static Flow<RT, RQ, A> Asks<A>(Func<RQ, A> f) =>
        Asks((rq, _) => f(rq));

    static K<Flow<RT, RQ>, A> Readable<Flow<RT, RQ>, (RT, RQ)>.Local<A>(
        Func<(RT, RQ), (RT, RQ)> f,
        K<Flow<RT, RQ>, A> ma) =>
        new Flow<RT, RQ, A>((s, r) =>
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
    public static Flow<RT, RQ, A> Local<A>(K<Flow<RT, RQ>, A> ma) =>
        +Readable.local<Flow<RT, RQ>, (RT, RQ), A>(cr => cr, ma);
}

file static class VFinal
{
    public static K<F, A> Finally<F, A, X>(K<F, A> ma, K<F, X> mx)
        where F : Final<F> =>
        F.Finally(ma, mx);
}