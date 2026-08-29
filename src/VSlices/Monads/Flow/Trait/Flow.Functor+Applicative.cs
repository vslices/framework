namespace VSlices.Monads;

public partial class Flow<RT, RQ>
{
    static K<Flow<RT, RQ>, B> Functor<Flow<RT, RQ>>.Map<A, B>(
        Func<A, B> f, K<Flow<RT, RQ>, A> ma) =>
        new Flow<RT, RQ, B>((s, r) => ma.RunFlow(s, r).Map(f));

    /// <summary>
    /// Transforms the result of a computation from one type to another using the specified mapping function.
    /// </summary>
    /// <typeparam name="A">The type of the input value.</typeparam>
    /// <typeparam name="B">The type of the output value after applying the mapping function.</typeparam>
    /// <param name="f">The mapping function to apply to the input value.</param>
    /// <param name="ma">The computation whose result is to be transformed.</param>
    /// <returns>A new computation with the transformed result.</returns>
    public static Flow<RT, RQ, B> Map<A, B>(
        Func<A, B> f, K<Flow<RT, RQ>, A> ma) =>
        +Functor.map(f, ma);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    /// <param name="f"></param>
    /// <param name="ma"></param>
    /// <returns></returns>
    public static Flow<RT, RQ, B> Map<A, B>(
        Func<A, B> f, Flow<RT, RQ, A> ma) =>
        +Functor.map(f, ma);


    /// <summary>
    /// Creates a new <see cref="Flow{RT, RQ, O}"/> by mapping all values of the provided computation
    /// to a constant value.
    /// </summary>
    /// <typeparam name="A">The type of the input value in the computation.</typeparam>
    /// <typeparam name="B">The type of the constant value to map to.</typeparam>
    /// <param name="b">The constant value to map all elements to.</param>
    /// <param name="ma">The computation to map.</param>
    /// <returns>A new <see cref="Flow{RT, RQ, O}"/> where all values are replaced with the constant value <paramref name="b"/>.</returns>
    public static Flow<RT, RQ, B> ConstMap<A, B>(
        B b, K<Flow<RT, RQ>, A> ma) =>
        Map(_ => b, ma);
    
    static K<Flow<RT, RQ>, A> Applicative<Flow<RT, RQ>>.Pure<A>(A value) =>
        new Flow<RT, RQ, A>((_, _) => IO.pure(value));
    
    /// <summary>
    /// Creates a new <see cref="Flow{RT, RQ, A}"/> instance with the specified value.
    /// </summary>
    /// <typeparam name="A">The type of the value to encapsulate within the liftFlow.</typeparam>
    /// <param name="value">The value to encapsulate within the liftFlow.</param>
    /// <returns>A new <see cref="Flow{RT, RQ, A}"/> instance containing the specified value.</returns>
    public static Flow<RT, RQ, A> Pure<A>(A value) =>
        +Applicative.pure<Flow<RT, RQ>, A>(value);

    /// <summary>
    /// Creates a new instance of <see cref="Flow{RT, RQ, A}"/> containing the specified value.
    /// </summary>
    /// <typeparam name="A">The type of the value to wrap in the <see cref="Flow{RT, RQ, A}"/>.</typeparam>
    /// <param name="pa">The value to wrap in the <see cref="Flow{RT, RQ, A}"/>.</param>
    /// <returns>A new <see cref="Flow{RT, RQ, A}"/> instance containing the specified value.</returns>
    public static Flow<RT, RQ, A> Pure<A>(Pure<A> pa) =>
        Pure(pa.Value);

    static K<Flow<RT, RQ>, B> Applicative<Flow<RT, RQ>>.Apply<A, B>(
        K<Flow<RT, RQ>, Func<A, B>> mf,
        K<Flow<RT, RQ>, A> ma) =>
        new Flow<RT, RQ, B>(
            (s, r) => mf.As().RunFlow(s, r)
                .Apply(ma.RunFlow(s, r)));

    static K<Flow<RT, RQ>, B> Applicative<Flow<RT, RQ>>.Apply<A, B>(
        K<Flow<RT, RQ>, Func<A, B>> mf,
        Memo<Flow<RT, RQ>, A> ma) =>
        new Flow<RT, RQ, B>(
            (s, r) => mf.As().RunFlow(s, r)
                .Apply(ma.Value.RunFlow(s, r)));

    /// <summary>
    /// Applies a function encapsulated in a monadic context to a value encapsulated in another monadic context.
    /// </summary>
    /// <typeparam name="T">The type of the input value.</typeparam>
    /// <typeparam name="O">The type of the output value after applying the function.</typeparam>
    /// <param name="mf">A monadic context containing the function to be applied.</param>
    /// <param name="ma">A monadic context containing the input value.</param>
    /// <returns>A new <see cref="Flow{RT, RQ, O}"/> representing the result of applying the function to the input value.</returns>
    public static Flow<RT, RQ, O> Apply<T, O>(
        K<Flow<RT, RQ>, Func<T, O>> mf,
        K<Flow<RT, RQ>, T> ma) =>
        +Applicative.apply(mf, ma);

    /// <summary>
    /// Combines two computations into a single computation, executing them sequentially.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the first computation.</typeparam>
    /// <typeparam name="O">The type of the result produced by the second computation.</typeparam>
    /// <param name="ma">The first computation to execute.</param>
    /// <param name="mb">The second computation to execute.</param>
    /// <returns>A new <see cref="Flow{RT, RQ, O}"/> representing the combined computation.</returns>
    public static Flow<RT, RQ, O> Action<A, O>(
        K<Flow<RT, RQ>, A> ma,
        K<Flow<RT, RQ>, O> mb) =>
        +Applicative.action(ma, mb);
}
