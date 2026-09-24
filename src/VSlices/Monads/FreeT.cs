using LanguageExt;
using LanguageExt.Traits;

namespace VSlices;

/// <summary>
/// One observable layer of a free monad transformer.
/// </summary>
public abstract record FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Terminal layer of a free monad transformer.
/// </summary>
public sealed record FreeTPure<F, M, A>(A Value)
    : FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Suspended functor layer of a free monad transformer.
/// </summary>
public sealed record FreeTSuspend<F, M, A>(
    K<F, FreeT<F, M, A>> Value)
    : FreeTStep<F, M, A>
    where F : Functor<F>
    where M : Monad<M>;

/// <summary>
/// Free monad transformer.
///
/// The representation is:
///
/// M (Pure A | Suspend (F (FreeT F M A)))
///
/// so composition requires only Functor F and Monad M.
/// </summary>
public sealed record FreeT<F, M, A>(
    K<M, FreeTStep<F, M, A>> runFreeT)
    : K<FreeT<F, M>, A>
    where F : Functor<F>
    where M : Monad<M>
{
    public FreeT<F, M, B> Map<B>(Func<A, B> f) =>
        Bind(value => FreeT.pure<F, M, B>(f(value)));

    public FreeT<F, M, B> Select<B>(Func<A, B> f) =>
        Map(f);

    public FreeT<F, M, B> Bind<B>(
        Func<A, FreeT<F, M, B>> f) =>
        new(
            M.Bind(
                runFreeT,
                step => step switch
                {
                    FreeTPure<F, M, A>(var value) =>
                        f(value).runFreeT,

                    FreeTSuspend<F, M, A>(var suspended) =>
                        M.Pure<FreeTStep<F, M, B>>(
                            new FreeTSuspend<F, M, B>(
                                F.Map(
                                    next => next.Bind(f),
                                    suspended))),

                    _ => throw new NotSupportedException()
                }));

    public FreeT<F, M, B> Bind<B>(
        Func<A, K<FreeT<F, M>, B>> f) =>
        Bind(value => f(value).As());

    public FreeT<F, M, B> Bind<B>(
        Func<A, K<M, B>> f) =>
        Bind(value => FreeT.liftM<F, M, B>(f(value)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, FreeT<F, M, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            bind(value)
                .Map(next => project(value, next)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, K<FreeT<F, M>, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            bind(value)
                .As()
                .Map(next => project(value, next)));

    public FreeT<F, M, C> SelectMany<B, C>(
        Func<A, K<M, B>> bind,
        Func<A, B, C> project) =>
        Bind(value =>
            FreeT
                .liftM<F, M, B>(bind(value))
                .Map(next => project(value, next)));
}

/// <summary>
/// HKT witness and type-class implementation for FreeT.
/// </summary>
public sealed class FreeT<F, M> :
    MonadT<FreeT<F, M>, M>
    where F : Functor<F>
    where M : Monad<M>
{
    static K<FreeT<F, M>, B> Monad<FreeT<F, M>>.Bind<A, B>(
        K<FreeT<F, M>, A> ma,
        Func<A, K<FreeT<F, M>, B>> f) =>
        ma.As().Bind(f);

    static K<FreeT<F, M>, B> Monad<FreeT<F, M>>.Recur<A, B>(
        A value,
        Func<A, K<FreeT<F, M>, Next<A, B>>> f) =>
        // Prototype fallback for the LanguageExt version currently consumed by VSlices.
        // When promoted to the fork this should use the stack-safe recursion machinery
        // available in the target LanguageExt revision.
        f(value)
            .As()
            .Bind(next =>
                next.IsDone
                    ? FreeT.pure<F, M, B>(next.Done)
                    : ((Monad<FreeT<F, M>>)default!).Recur(next.Loop, f).As());

    static K<FreeT<F, M>, B> Functor<FreeT<F, M>>.Map<A, B>(
        Func<A, B> f,
        K<FreeT<F, M>, A> ma) =>
        ma.As().Map(f);

    static K<FreeT<F, M>, A> Applicative<FreeT<F, M>>.Pure<A>(
        A value) =>
        FreeT.pure<F, M, A>(value);

    static K<FreeT<F, M>, B> Applicative<FreeT<F, M>>.Apply<A, B>(
        K<FreeT<F, M>, Func<A, B>> mf,
        K<FreeT<F, M>, A> ma) =>
        mf.As().Bind(
            f => ma.As().Map(f));

    static K<FreeT<F, M>, B> Applicative<FreeT<F, M>>.Apply<A, B>(
        K<FreeT<F, M>, Func<A, B>> mf,
        Memo<FreeT<F, M>, A> ma) =>
        mf.As().Bind(
            f => ma.Value.As().Map(f));

    static K<FreeT<F, M>, A> MonadT<FreeT<F, M>, M>.Lift<A>(
        K<M, A> ma) =>
        FreeT.liftM<F, M, A>(ma);
}

/// <summary>
/// FreeT constructors and lifts.
/// </summary>
public static class FreeT
{
    public static FreeT<F, M, A> pure<F, M, A>(A value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Pure<FreeTStep<F, M, A>>(
                new FreeTPure<F, M, A>(value)));

    /// <summary>
    /// Lift an action from the base monad M.
    /// </summary>
    public static FreeT<F, M, A> liftM<F, M, A>(
        K<M, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Map<A, FreeTStep<F, M, A>>(
                item => new FreeTPure<F, M, A>(item),
                value));

    /// <summary>
    /// Lift one instruction from the free functor F.
    /// </summary>
    public static FreeT<F, M, A> liftF<F, M, A>(
        K<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        new(
            M.Pure<FreeTStep<F, M, A>>(
                new FreeTSuspend<F, M, A>(
                    F.Map(
                        item => pure<F, M, A>(item),
                        value))));

    /// <summary>
    /// Lift an already-described Free program into FreeT without changing its algebra.
    /// </summary>
    public static FreeT<F, M, A> liftFree<F, M, A>(
        Free<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        value switch
        {
            Pure<F, A>(var item) =>
                pure<F, M, A>(item),

            Bind<F, A>(var suspended) =>
                new FreeT<F, M, A>(
                    M.Pure<FreeTStep<F, M, A>>(
                        new FreeTSuspend<F, M, A>(
                            F.Map(
                                next => liftFree<F, M, A>(next),
                                suspended)))),

            _ => throw new NotSupportedException()
        };
}

public static class FreeTExtensions
{
    public static FreeT<F, M, A> As<F, M, A>(
        this K<FreeT<F, M>, A> ma)
        where F : Functor<F>
        where M : Monad<M> =>
        (FreeT<F, M, A>)ma;

    public static FreeT<F, M, A> ToFreeT<F, M, A>(
        this Free<F, A> value)
        where F : Functor<F>
        where M : Monad<M> =>
        FreeT.liftFree<F, M, A>(value);
}
