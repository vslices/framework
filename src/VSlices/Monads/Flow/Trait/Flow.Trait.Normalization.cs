namespace VSlices.Monads;

public partial class Flow<RT, REQ>
{
    private Flow() { }

    /// <summary>
    /// Transforms the result of a computation from one type to another using the specified mapping function.
    /// </summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <typeparam name="O">The type of the output value after applying the mapping function.</typeparam>
    /// <param name="f">The mapping function to apply to the input value.</param>
    /// <param name="ma">The computation whose result is to be transformed.</param>
    /// <returns>A new computation with the transformed result.</returns>
    public static Flow<RT, REQ, O> Map<T, O>(
        Func<T, O> f, K<Flow<RT, REQ>, T> ma) =>
        +Functor.map(f, ma);

    /// <summary>
    /// Creates a new <see cref="Flow{RT, REQ, O}"/> by mapping all values of the provided computation 
    /// to a constant value.
    /// </summary>
    /// <typeparam name="T">The type of the input value in the computation.</typeparam>
    /// <typeparam name="O">The type of the constant value to map to.</typeparam>
    /// <param name="b">The constant value to map all elements to.</param>
    /// <param name="ma">The computation to map.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, O}"/> where all values are replaced with the constant value <paramref name="b"/>.</returns>
    public static Flow<RT, REQ, O> ConstMap<T, O>(
        O b, K<Flow<RT, REQ>, T> ma) =>
        Map(_ => b, ma);

    /// <summary>
    /// Creates a new <see cref="Flow{RT, REQ, A}"/> instance with the specified value.
    /// </summary>
    /// <typeparam name="A">The type of the value to encapsulate within the flow.</typeparam>
    /// <param name="value">The value to encapsulate within the flow.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, A}"/> instance containing the specified value.</returns>
    public static Flow<RT, REQ, A> Pure<A>(A value) =>
        +Applicative.pure<Flow<RT, REQ>, A>(value);

    /// <summary>
    /// Creates a new instance of <see cref="Flow{RT, REQ, A}"/> containing the specified value.
    /// </summary>
    /// <typeparam name="A">The type of the value to wrap in the <see cref="Flow{RT, REQ, A}"/>.</typeparam>
    /// <param name="pa">The value to wrap in the <see cref="Flow{RT, REQ, A}"/>.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, A}"/> instance containing the specified value.</returns>
    public static Flow<RT, REQ, A> Pure<A>(Pure<A> pa) =>
        Pure(pa.Value);

    /// <summary>
    /// Gets a <see cref="Flow{RT, REQ, Unit}"/> instance representing a unit value.
    /// </summary>
    /// <remarks>
    /// This property provides a predefined instance of <see cref="Flow{RT, REQ, Unit}"/> 
    /// that can be used when no meaningful value is required, adhering to functional programming principles.
    /// </remarks>
    public static Flow<RT, REQ, Unit> Unit { get; } = Pure(unit);

    /// <summary>
    /// Wraps a value into an <see cref="Option{A}"/> type within the <see cref="Flow{RT, REQ}"/> context.
    /// </summary>
    /// <typeparam name="A">The type of the value to wrap.</typeparam>
    /// <param name="v">The value to wrap in an <see cref="Option{A}"/>.</param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, Option{A}}"/> containing the provided value wrapped in an <see cref="Option{A}"/>.
    /// </returns>
    public static Flow<RT, REQ, Option<A>> Some<A>(A v) =>
        Pure<Option<A>>(v);

    /// <summary>
    /// Creates a <see cref="Flow{RT, REQ, Option{A}}"/> instance representing a "None" value.
    /// </summary>
    /// <typeparam name="A">The type of the value wrapped in the <see cref="Option{A}"/>.</typeparam>
    /// <returns>A <see cref="Flow{RT, REQ, Option{A}}"/> instance containing an "None" value.</returns>
    public static Flow<RT, REQ, Option<A>> None<A>() =>
        Pure<Option<A>>(Option.None);

    /// <summary>
    /// Combines two computations into a single computation, executing them sequentially.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the first computation.</typeparam>
    /// <typeparam name="O">The type of the result produced by the second computation.</typeparam>
    /// <param name="ma">The first computation to execute.</param>
    /// <param name="mb">The second computation to execute.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, O}"/> representing the combined computation.</returns>
    public static Flow<RT, REQ, O> Action<A, O>(
        K<Flow<RT, REQ>, A> ma, 
        K<Flow<RT, REQ>, O> mb) =>
        +Applicative.action(ma, mb);
    
    /// <summary>
    /// Applies a function encapsulated in a monadic context to a value encapsulated in another monadic context.
    /// </summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <typeparam name="O">The type of the output value after applying the function.</typeparam>
    /// <param name="mf">A monadic context containing the function to be applied.</param>
    /// <param name="ma">A monadic context containing the input value.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, O}"/> representing the result of applying the function to the input value.</returns>
    public static Flow<RT, REQ, O> Apply<T, O>(
        K<Flow<RT, REQ>, Func<T, O>> mf,
        K<Flow<RT, REQ>, T> ma) =>
        +Applicative.apply(mf, ma);
    
    /// <summary>
    /// Binds a computation to a function that produces a new computation, 
    /// enabling the chaining of operations in a monadic flow.
    /// </summary>
    /// <typeparam name="T">The type of the input value of the computation.</typeparam>
    /// <typeparam name="O">The type of the output value of the resulting computation.</typeparam>
    /// <param name="ma">The initial computation to bind.</param>
    /// <param name="fb">
    /// A function that takes the result of the initial computation and returns 
    /// a new computation of type <see cref="K{Flow{RT, REQ}, O}"/>.
    /// </param>
    /// <returns>A new computation of type <see cref="Flow{RT, REQ, O}"/>.</returns>
    public static Flow<RT, REQ, O> Bind<T, O>(
        K<Flow<RT, REQ>, T> ma,
        Func<T, K<Flow<RT, REQ>, O>> fb) =>
        +Monad.bind(ma, fb);

    /// <summary>
    /// Recursively processes a value using a specified function, producing a result wrapped in a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the input value to process.</typeparam>
    /// <typeparam name="O">The type of the final output value.</typeparam>
    /// <param name="value">The initial value to process.</param>
    /// <param name="f">
    /// A function that takes the current value and returns a computation wrapped in 
    /// a <see cref="K{Flow{RT, REQ}, Next{T, O}}"/> indicating the next step or the final result.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> representing the result of the recursive computation.
    /// </returns>
    public static Flow<RT, REQ, O> Recur<T, O>(
        T value,
        Func<T, K<Flow<RT, REQ>, Next<T, O>>> f) =>
        +Monad.recur(value, f);

    /// <summary>
    /// Lifts an <see cref="IO{T}"/> computation into the <see cref="Flow{RT, REQ, T}"/> context.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the <see cref="IO{T}"/> computation.</typeparam>
    /// <param name="ma">The <see cref="IO{T}"/> computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, A}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<Flow<RT, REQ>, A>(ma);
    
    /// <summary>
    /// Converts a monadic computation of type <see cref="K{Flow{RT, REQ}, A}"/> 
    /// into a computation that produces an <see cref="IO{A}"/> result.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the computation.</typeparam>
    /// <param name="ma">The monadic computation to convert.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, IO{A}}"/> representing the computation as an IO operation.</returns>
    public static Flow<RT, REQ, IO<A>> ToIO<A>(K<Flow<RT, REQ>, A> ma) =>
        +MonadUnliftIO.toIO(ma);

    /// <summary>
    /// Creates a failed <see cref="Flow{RT, REQ, A}"/> instance with the specified error.
    /// </summary>
    /// <typeparam name="A">The type of the result value that the flow would have produced if successful.</typeparam>
    /// <param name="e">The error that represents the failure.</param>
    /// <returns>A <see cref="Flow{RT, REQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, REQ, A> Fail<A>(Error e) =>
        +Fallible.error<Flow<RT, REQ>, A>(e);

    /// <summary>
    /// Creates a failed <see cref="Flow{RT, REQ, A}"/> instance with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result value.</typeparam>
    /// <param name="msg">The error message describing the failure.</param>
    /// <returns>A <see cref="Flow{RT, REQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, REQ, A> Fail<A>(string msg) =>
        Fail<A>(Error.New(msg));

    /// <summary>
    /// Creates a new <see cref="Flow{RT, REQ, A}"/> instance that represents a failure.
    /// </summary>
    /// <typeparam name="A">The type of the result expected from the flow.</typeparam>
    /// <param name="fe">The failure object containing an <see cref="Error"/>.</param>
    /// <returns>A <see cref="Flow{RT, REQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, REQ, A> Fail<A>(Fail<Error> fe) =>
        Fail<A>(fe.Value);

    /// <summary>
    /// Creates a new <see cref="Flow{RT, REQ, A}"/> instance that represents a failure with the specified error message.
    /// </summary>
    /// <typeparam name="A">The type of the result that the flow would have produced if it had succeeded.</typeparam>
    /// <param name="fe">The failure object containing the error message.</param>
    /// <returns>A <see cref="Flow{RT, REQ, A}"/> instance representing the failure.</returns>
    public static Flow<RT, REQ, A> Fail<A>(Fail<string> fe) =>
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
    /// A new <see cref="Flow{RT, REQ, A}"/> instance that represents the result of the computation,
    /// either successfully or after applying the recovery function.
    /// </returns>
    public static Flow<RT, REQ, A> Catch<A>(
        K<Flow<RT, REQ>, A> fa,
        Func<Error, bool> Predicate,
        Func<Error, K<Flow<RT, REQ>, A>> Fail) =>
        +fa.Catch(Predicate, Fail);

    /// <summary>
    /// Chooses between two flows, returning the result of the first flow if it succeeds,
    /// or the result of the second flow if the first one fails.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the flows.</typeparam>
    /// <param name="fa">The first flow to be executed.</param>
    /// <param name="fb">The second flow to be executed if the first one fails.</param>
    /// <returns>A new flow that represents the choice between the two provided flows.</returns>
    public static Flow<RT, REQ, A> Choose<A>(
        K<Flow<RT, REQ>, A> fa,
        K<Flow<RT, REQ>, A> fb) =>
        +Choice.choose(fa, fb);

    /// <summary>
    /// Chooses between two flows, returning the result of the first flow if it succeeds,
    /// or the result of the second flow if the first flow fails.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the flows.</typeparam>
    /// <param name="fa">The first flow to evaluate.</param>
    /// <param name="fb">The second flow to evaluate if the first flow fails.</param>
    /// <returns>A new flow that represents the result of the chosen flow.</returns>
    public static Flow<RT, REQ, A> Choose<A>(
        K<Flow<RT, REQ>, A> fa,
        Memo<Flow<RT, REQ>, A> fb) =>
        +Choice.choose(fa, fb.Value);

    /// <summary>
    /// Creates an empty <see cref="Flow{RT, REQ, A}"/> instance, representing the identity element
    /// for the alternative composition of flows.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the flow.</typeparam>
    /// <returns>An empty <see cref="Flow{RT, REQ, A}"/> instance.</returns>
    public static Flow<RT, REQ, A> Empty<A>() =>
        +Alternative.empty<Flow<RT, REQ>, A>();

    /// <summary>
    /// Combines two flows into a single flow by applying a semigroup operation.
    /// </summary>
    /// <typeparam name="A">The type of the value contained in the flows.</typeparam>
    /// <param name="mx">The first flow to combine.</param>
    /// <param name="my">The second flow to combine.</param>
    /// <returns>A new flow that represents the combination of the two input flows.</returns>
    public static Flow<RT, REQ, A> Combine<A>(
        K<Flow<RT, REQ>, A> mx, 
        K<Flow<RT, REQ>, A> my) =>
        +SemigroupK.combine(mx, my);

    /// <summary>
    /// Ensures that a finalizing computation is executed after the main computation,
    /// regardless of whether the main computation succeeds or fails.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the main computation.</typeparam>
    /// <typeparam name="X">The type of the result produced by the finalizing computation.</typeparam>
    /// <param name="fa">The main computation to execute.</param>
    /// <param name="fx">The finalizing computation to execute after the main computation.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, A}"/> that represents the combined computation.</returns>
    public static Flow<RT, REQ, A> Finally<A, X>(
        K<Flow<RT, REQ>, A> fa,
        K<Flow<RT, REQ>, X> fx) =>
        +(fa | Final.final(fx));
    
    /// <summary>
    /// Creates a new <see cref="Flow{RT, REQ, A}"/> by applying the provided function
    /// to the runtime and request values.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes the runtime (<typeparamref name="RT"/>) and request (<typeparamref name="REQ"/>)
    /// values as input and produces a result of type <typeparamref name="A"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{RT, REQ, A}"/> that encapsulates the result of applying the function
    /// to the runtime and request values.
    /// </returns>
    public static Flow<RT, REQ, A> Asks<A>(Func<RT, REQ, A> f) =>
        +Readable.asks<Flow<RT, REQ>, (RT, REQ), A>(cr => f(cr.Item1, cr.Item2));

    /// <summary>
    /// Executes a computation in a modified environment.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the computation.</typeparam>
    /// <param name="ma">The computation to execute in the modified environment.</param>
    /// <returns>A new <see cref="Flow{RT, REQ, A}"/> representing the computation in the modified environment.</returns>
    public static Flow<RT, REQ, A> Local<A>(K<Flow<RT, REQ>, A> ma) =>
        +Readable.local<Flow<RT, REQ>, (RT, REQ), A>(cr => cr, ma);
}
