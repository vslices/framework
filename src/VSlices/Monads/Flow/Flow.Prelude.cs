using VSlices.Monads;

namespace VSlices;

public static partial class VSlicesPrelude
{
    /// <summary>
    /// Lifts a function into a <see cref="Flow{RT, REQ, RES}"/> monad, enabling it to be used
    /// within a functional flow context.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow.</typeparam>
    /// <param name="f">
    /// The function to be lifted
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided function
    /// within the flow monad.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, IO<RES>> f) =>
        Flow<RT, REQ>.LiftIO(f);

    /// <summary>
    /// Lifts a synchronous function into a <see cref="Flow{RT, REQ, RES}"/> monad, allowing it to be used
    /// within a functional flow context without the need for asynchronous handling.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow.</typeparam>
    /// <param name="f">
    /// A function that takes the runtime and IO environment as well as the request as inputs and returns a 
    /// <see cref="Task{RES}"/> representing the computation.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided function
    /// within the flow monad.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, EnvIO, Task<RES>> f) =>
        Flow<RT, REQ>.LiftIO(f);

    /// <summary>
    /// Lifts a synchronous function into a <see cref="Flow{RT, REQ, RES}"/> monad, allowing it to be used
    /// within a functional flow context without the need for asynchronous handling.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow.</typeparam>
    /// <param name="f">
    /// A function that takes the runtime environment and request as inputs and returns a 
    /// <typeparamref name="RES"/> representing the computation.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided function
    /// within the flow monad.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, RES> f) =>
        Flow<RT, REQ>.Lift(f);

    /// <summary>
    /// Lifts a function into a <see cref="Flow{RT, REQ, RES}"/> monad, enabling functional composition
    /// and chaining of computations.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow. Must be non-null.</typeparam>
    /// <param name="f">
    /// A function that takes the runtime environment and request as inputs and returns a 
    /// <see cref="FinT{IO, RES}"/> representing the computation.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> that encapsulates the lifted computation.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, FinT<IO, RES>> f) =>
        liftFlow((RT c, REQ r) =>
            f(c, r).Run().As().Bind<RES>(ma => ma.Match(IO.pure, IO.fail<RES>)));
    
    /// <summary>
    /// Lifts a function into a <see cref="Flow{RT, REQ, RES}"/> monad, enabling it to be used within the flow.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow. Must be non-null.</typeparam>
    /// <param name="f">
    /// A function that takes a runtime environment, a request, and an <see cref="EnvIO"/> instance, 
    /// and returns a <see cref="Task{TResult}"/> containing a <see cref="Fin{RES}"/> result.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the provided function.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, EnvIO, Task<Fin<RES>>> f) =>
        liftFlow((RT c, REQ r) => FinT.lift(IO.liftAsync(e => f(c, r, e))));
    
    /// <summary>
    /// Lifts a function into a <see cref="Flow{RT, REQ, RES}"/> monad, enabling functional composition
    /// and chaining of computations.
    /// </summary>
    /// <typeparam name="RT">The type of the runtime environment used in the flow.</typeparam>
    /// <typeparam name="REQ">The type of the request input for the flow.</typeparam>
    /// <typeparam name="RES">The type of the result produced by the flow. Must be non-null.</typeparam>
    /// <param name="f">
    /// A function that takes a runtime environment of type <typeparamref name="RT"/> and a request of type
    /// <typeparamref name="REQ"/>, and produces a <see cref="Fin{RES}"/> result.
    /// </param>
    /// <returns>
    /// A <see cref="Flow{RT, REQ, RES}"/> instance that encapsulates the lifted function.
    /// </returns>
    public static Flow<RT, REQ, RES> liftFlow<RT, REQ, RES>(
        Func<RT, REQ, Fin<RES>> f) =>
        liftFlow((RT c, REQ r) => FinT.lift<IO, RES>(f(c, r)));

}
