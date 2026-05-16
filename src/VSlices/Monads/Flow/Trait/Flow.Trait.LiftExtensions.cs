namespace VSlices.Monads;

public partial class Flow<RT, REQ>
{
    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, A}"/> context, enabling the function
    /// to operate within the monadic flow by utilizing the provided runtime and request parameters.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the lifted function.</typeparam>
    /// <param name="f">
    /// A function that takes runtime (<typeparamref name="RT"/>) and request (<typeparamref name="REQ"/>)
    /// parameters and returns an <see cref="IO{A}"/> computation.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, A}"/> instance representing the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, A> LiftIO<A>(Func<RT, REQ, IO<A>> f) =>
        new(f);
    
    /// <summary>
    /// Lifts a function that produces an effectful computation into the <see cref="Flow{RT, REQ, A}"/> context.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the effectful computation.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a computation of type <see cref="K{IO, A}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, A}"/> instance that encapsulates the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, A> LiftIO<A>(Func<RT, REQ, K<IO, A>> f) =>
        new((rt, req) => f(rt, req).As());

    /// <summary>
    /// Lifts a function that takes an environment, a requirement, and an asynchronous computation
    /// into a <see cref="Flow{RT, REQ, A}"/>.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the asynchronous computation.</typeparam>
    /// <param name="f">
    /// A function that takes an environment of type <typeparamref name="RT"/>, 
    /// a requirement of type <typeparamref name="REQ"/>, and an <see cref="EnvIO"/> 
    /// to produce a <see cref="Task{A}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, A}"/> that encapsulates the asynchronous computation.
    /// </returns>
    public static Flow<RT, REQ, A> LiftIO<A>(Func<RT, REQ, EnvIO, Task<A>> f) =>
        new((c, r) => IO.liftAsync(e => f(c, r, e)));

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/> 
    /// and returns a value of type <typeparamref name="O"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> instance that encapsulates the provided function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, O> f) =>
        new((c, r) => IO.pure(f(c, r)));

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a value of type <see cref="Eff{O}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{RT, REQ, O}"/> instance that encapsulates the provided function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Eff<O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context, allowing it to be composed
    /// within the monadic flow.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two inputs of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and produces a value of type <see cref="K{Eff, O}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> that represents the lifted function within the flow context.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<Eff, O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> monad, allowing it to operate within the context of the monad.
    /// </summary>
    /// <typeparam name="O">The type of the output value produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and produces a result of type <see cref="Eff{RT, O}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{RT, REQ, O}"/> instance that encapsulates the provided function within the monadic context.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Eff<RT, O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(c, e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a value of type <see cref="K{Eff{RT}, O}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{RT, REQ, O}"/> instance that represents the lifted function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<Eff<RT>, O>> f) =>
        new((c, r) => IO.env.Bind(e => f(c, r).Run(c, e).Match(Succ: IO.pure, Fail: IO.fail<O>)));

    /// <summary>
    /// Lifts a function that produces a <see cref="Fin{T}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a <see cref="Fin{T}"/> representing a computation that may succeed or fail.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> that encapsulates the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, Fin<O>> f) =>
        new((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>));

    /// <summary>
    /// Lifts a function that produces a result of type <typeparamref name="O"/> 
    /// within a context of <see cref="K{Fin, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and 
    /// <typeparamref name="REQ"/>, and returns a result of type <see cref="K{Fin, O}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> representing the lifted function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<Fin, O>> f) =>
        new((c, r) => f(c, r).As().Match(Succ: IO.pure, Fail: IO.fail<O>));

    /// <summary>
    /// Lifts a function that produces a <see cref="FinT{TMonad, TResult}"/> into a <see cref="Flow{RT, REQ, TResult}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a <see cref="FinT{TMonad, TResult}"/> representing a computation that may succeed or fail.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, TResult}"/> that represents the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<IO, O>> f) =>
        LiftIO((c, r) => f(c, r).Match(Succ: IO.pure, Fail: IO.fail<O>).As().Flatten());

    /// <summary>
    /// Lifts a computation represented by a function into the <see cref="Flow{RT, REQ, O}"/> monad.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the computation.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a computation of type <see cref="K{FinT{IO}, O}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> instance that encapsulates the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<FinT<IO>, O>> f) =>
        LiftIO((c, r) => f(c, r).As().Match(Succ: IO.pure, Fail: IO.fail<O>).As().Flatten());

    /// <summary>
    /// Lifts a function that produces a <see cref="FinT{Eff, T}"/> result into a <see cref="Flow{RT, REQ, T}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the output value produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a <see cref="FinT{Eff, T}"/> representing a computation that may succeed or fail.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, T}"/> that encapsulates the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<Eff, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success, Fail: Eff.Fail<O>)
                              .As().Flatten());

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context.
    /// </summary>
    /// <typeparam name="O">The type of the output value produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two arguments of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and returns a value wrapped in a <see cref="K{FinT{Eff}, O}"/> context.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> instance that represents the lifted function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<FinT<Eff>, O>> f) =>
        Lift((c, r) => f(c, r).As().Match(Succ: Eff.Success, Fail: Eff.Fail<O>)
                              .As().Flatten());

    /// <summary>
    /// Lifts a function that produces a <see cref="FinT{Eff{RT}, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the output value.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters, <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and produces a <see cref="FinT{Eff{RT}, O}"/>.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, O}"/> that encapsulates the lifted function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, FinT<Eff<RT>, O>> f) =>
        Lift((c, r) => f(c, r).Match(Succ: Eff.Success<RT, O>, Fail: Eff.Fail<RT, O>)
                              .As().Flatten());

    /// <summary>
    /// Lifts a function into the <see cref="Flow{RT, REQ, O}"/> context.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the function.</typeparam>
    /// <param name="f">
    /// A function that takes two parameters of types <typeparamref name="RT"/> and <typeparamref name="REQ"/>, 
    /// and produces a value of type <see cref="K{FinT{Eff{RT}}, O}"/>.
    /// </param>
    /// <returns>
    /// A new <see cref="Flow{RT, REQ, O}"/> instance that represents the lifted function.
    /// </returns>
    public static Flow<RT, REQ, O> Lift<O>(Func<RT, REQ, K<FinT<Eff<RT>>, O>> f) =>
        Lift((c, r) => f(c, r).As().Match(Succ: Eff.Success<RT, O>, Fail: Eff.Fail<RT, O>)
                              .As().Flatten());

    /// <summary>
    /// Lifts a computation of type <see cref="K{Eff, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<Eff, O> m) =>
        Lift<O>((_, _) => m);

    /// <summary>
    /// Lifts a computation of type <see cref="K{Eff{RT}, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the output produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<Eff<RT>, O> m) =>
        Lift<O>((_, _) => m);

    /// <summary>
    /// Lifts a computation of type <see cref="K{Fin, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<Fin, O> m) =>
        Lift<O>((_, _) => m);

    /// <summary>
    /// Lifts a computation represented by a <see cref="K{TMonad, TResult}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted, represented as a <see cref="K{FinT{IO}, O}"/>.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> that encapsulates the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<FinT<IO>, O> m) =>
        Lift<O>((_, _) => m);

    /// <summary>
    /// Lifts a computation of type <see cref="K{FinT{Eff}, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<FinT<Eff>, O> m) =>
        Lift<O>((_, _) => m);

    /// <summary>
    /// Lifts a computation of type <see cref="K{FinT{Eff{RT}}, O}"/> into a <see cref="Flow{RT, REQ, O}"/>.
    /// </summary>
    /// <typeparam name="O">The type of the result produced by the computation.</typeparam>
    /// <param name="m">The computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, REQ, O}"/> representing the lifted computation.</returns>
    public static Flow<RT, REQ, O> Lift<O>(K<FinT<Eff<RT>>, O> m) =>
        Lift<O>((_, _) => m);
}
