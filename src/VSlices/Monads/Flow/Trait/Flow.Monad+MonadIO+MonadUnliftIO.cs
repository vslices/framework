using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Monads;

public partial class Flow<RT, RQ>
{
    static K<Flow<RT, RQ>, B> Monad<Flow<RT, RQ>>.Bind<A, B>(
        K<Flow<RT, RQ>, A> ma,
        Func<A, K<Flow<RT, RQ>, B>> f) =>
        new Flow<RT, RQ, B>(
            (s, r) => ma.RunFlow(s, r)
                .Bind(a => f(a).RunFlow(s, r)));
    
    /// <summary>
    /// Binds a computation to a function that produces a new computation,
    /// enabling the chaining of operations in a monadic liftFlow.
    /// </summary>
    /// <typeparam name="T">The type of the input value of the computation.</typeparam>
    /// <typeparam name="O">The type of the output value of the resulting computation.</typeparam>
    /// <param name="ma">The initial computation to bind.</param>
    /// <param name="fb">
    ///
    /// </param>
    /// <returns>A new computation of type <see cref="Flow{RT, RQ, O}"/>.</returns>
    public static Flow<RT, RQ, O> Bind<T, O>(
        K<Flow<RT, RQ>, T> ma,
        Func<T, K<Flow<RT, RQ>, O>> fb) =>
        +Monad.bind(ma, fb);

    static K<Flow<RT, RQ>, B> Monad<Flow<RT, RQ>>.Recur<A, B>(
        A value,
        Func<A, K<Flow<RT, RQ>, Next<A, B>>> f) =>
        new Flow<RT, RQ, B>((ctx, req) =>
            FinT.lift(
                    IO.liftAsync(async env =>
                    {
                        var current = value;

                        while (true)
                        {
                            var mNext = await f(current).As().RunAsync(ctx, req, env);

                            if (mNext is Fin<Next<A, B>>.Fail(var e))
                            {
                                return Fin.Fail<B>(e);
                            }

                            var next = (Next<A, B>)mNext;

                            if (next.IsDone) return Fin.Succ(next.Done);
                            current = next.Loop;
                        }
                    })
                )
                .Run().As()
                .Bind<B>(ma => ma.Match(IO.pure, IO.fail<B>))
        );

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="mma"></param>
    /// <returns></returns>
    public static Flow<RT, RQ, A> Flatten<A>(
        K<Flow<RT, RQ>, K<Flow<RT, RQ>, A>> mma) =>
        +Monad.flatten(mma);

    public static Flow<RT, RQ, A> Flatten<A>(
        K<Flow<RT, RQ>, Flow<RT, RQ, A>> mma) =>
        +Monad.flatten(mma.Map(ma => ma.Kind()));

    static K<Flow<RT, RQ>, A> MonadIO<Flow<RT, RQ>>.LiftIO<A>(IO<A> ma) =>
        new Flow<RT, RQ, A>((_, _) => ma);

    /// <summary>
    /// Lifts an <see cref="IO{T}"/> computation into the <see cref="Flow{RT, RQ, T}"/> context.
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the <see cref="IO{T}"/> computation.</typeparam>
    /// <param name="ma">The <see cref="IO{T}"/> computation to be lifted.</param>
    /// <returns>A <see cref="Flow{RT, RQ, A}"/> representing the lifted computation.</returns>
    public static Flow<RT, RQ, A> LiftIO<A>(IO<A> ma) =>
        +MonadIO.liftIO<Flow<RT, RQ>, A>(ma);
    
    static K<Flow<RT, RQ>, IO<A>> MonadUnliftIO<Flow<RT, RQ>>.ToIO<A>(
        K<Flow<RT, RQ>, A> ma) =>
        new Flow<RT, RQ, IO<A>>((c, r) => IO.pure(ma.RunFlow(c, r)));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A">The type of the result produced by the computation.</typeparam>
    /// <param name="ma">The monadic computation to convert.</param>
    /// <returns></returns>
    public static Flow<RT, RQ, IO<A>> ToIO<A>(K<Flow<RT, RQ>, A> ma) =>
        +MonadUnliftIO.toIO(ma);
}
