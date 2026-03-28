using VSlices.Core.Errors;

namespace VSlices.Core;

public sealed partial record FeatureEff<RT, A>(EitherT<FeatureError, Eff<RT>, A> Effect)
    : K<FeatureEff<RT>, A>
{
    // Constructores desde monads
    public FeatureEff(Eff<RT, A> m) 
        : this(EitherT.lift<FeatureError, Eff<RT>, A>(m)) { }

    public FeatureEff(Eff<A> m) : this(m.As().WithRuntime<RT>()) { }

    public FeatureEff(IO<A> m) : this(Eff<A>.LiftIO(m)) { }

    public FeatureEff(Fin<A> m) : this(m.As().ToEff()) { }

    public FeatureEff(Either<FeatureError, A> m) 
        : this(EitherT.lift<FeatureError, Eff<RT>, A>(m.As())) { }

    public FeatureEff(Either<Exceptional, A> m) 
        : this(m.Right(Fin.Succ).Left(e => Fin.Fail<A>(e))) { }

    public FeatureEff(Eff<RT, Either<FeatureError, A>> m) 
        : this(EitherT.lift(m)) { }

    public FeatureEff(Fin<Either<FeatureError, A>> m)
        : this(m.ToEff()) { }

    public FeatureEff(Eff<RT, K<Either<FeatureError>, A>> m)
        : this(EitherT.lift(m.Map(m => m.As()))) { }

    public FeatureEff(Fin<K<Either<FeatureError>, A>> m)
        : this(m.ToEff()) { }

    // Constructores desde valores
    public FeatureEff(A a) 
        : this(EitherT.Right<FeatureError, Eff<RT>, A>(a)) { }

    public FeatureEff(Pure<A> a) : this(a.Value) { }

    public FeatureEff(FeatureError e) 
        : this(Either.Left<FeatureError, A>(e)) { }

    public FeatureEff(Fail<FeatureError> e) : this(e.Value) { }

    public FeatureEff(Exceptional e) : this(Fin.Fail<A>(e)) { }

    public FeatureEff(Fail<Exceptional> e) : this(e.Value) { }

    // Constructores desde IO con runtime
    public FeatureEff(Func<RT, IO<A>> f) : this(Eff<RT, A>.LiftIO(f)) { }

    public FeatureEff(Func<RT, IO<Pure<A>>> f) 
        : this((rt) => f(rt).Map(a => a.Value)) { }

    public FeatureEff(Func<RT, IO<FeatureError>> f)
        : this(Eff<RT, Either<FeatureError, A>>
              .LiftIO(rt => f(rt).Map(Either.Left<FeatureError, A>))) { }

    public FeatureEff(Func<RT, IO<Fail<FeatureError>>> f)
        : this((rt) => f(rt).Map(a => a.Value)) { }

    public FeatureEff(Func<RT, IO<Exceptional>> f)
        : this(Eff<RT, A>.LiftIO(async (rt) => await f(rt).RunAsync())) { }

    public FeatureEff(Func<RT, IO<Fail<Exceptional>>> f)
        : this((rt) => f(rt).Map(a => a.Value)) { }

    public FeatureEff(Func<RT, IO<Either<FeatureError, A>>> f)
        : this(Eff<RT, Either<FeatureError, A>>.LiftIO((rt) => f(rt))) { }

    public FeatureEff(Func<RT, IO<Either<Exceptional, A>>> f)
        : this(Eff<RT, Eff<RT, A>>
              .LiftIO(async rt => (await f(rt).RunAsync())
                .Right(Fin.Succ)
                .Left(Fin.Fail<A>)
                .ToEff()
                .WithRuntime<RT>())
              .Flatten()) { }

    public FeatureEff(Func<RT, IO<Fin<A>>> f)
        : this(Eff<RT, Eff<RT, A>>
              .LiftIO(async (rt) => (await f(rt).RunAsync())
                .ToEff()
                .WithRuntime<RT>())
              .Flatten()) { }

    public FeatureEff(Func<RT, IO<Fin<Either<FeatureError, A>>>> f)
        : this(Eff<RT, Eff<RT, Either<FeatureError, A>>>
              .LiftIO(rt => f(rt).Map(m => m.ToEff().WithRuntime<RT>()))
              .Flatten()) { }

    // Constructores desde fiones asincronas con runtime
    public FeatureEff(Func<RT, Task<A>> f) 
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Pure<A>>> f) 
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<FeatureError>> f) 
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Fail<FeatureError>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Exceptional>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Fail<Exceptional>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Either<FeatureError, A>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Either<Exceptional, A>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Fin<A>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    public FeatureEff(Func<RT, Task<Fin<Either<FeatureError, A>>>> f)
        : this((rt) => liftIO(() => f(rt))) { }

    // Constructores desde fiones sincronas con runtime
    public FeatureEff(Func<RT, A> f) : this(Eff<RT, A>.Lift(f)) { }

    public FeatureEff(Func<RT, Pure<A>> f) : this(rt => f(rt).Value) { }

    public FeatureEff(Func<RT, FeatureError> f)
        : this(Eff<RT, Either<FeatureError, A>>.Lift(rt => f(rt))) { }

    public FeatureEff(Func<RT, Fail<FeatureError>> f) 
        : this(rt => f(rt).Value) { }

    public FeatureEff(Func<RT, Exceptional> f) 
        : this(Eff<RT, A>.Lift(rt => f(rt))) { }

    public FeatureEff(Func<RT, Fail<Exceptional>> f) 
        : this(rt => f(rt).Value) { }

    public FeatureEff(Func<RT, Either<FeatureError, A>> f)
        : this(Eff<RT, Either<FeatureError, A>>.Lift(f)) { }

    public FeatureEff(Func<RT, Fin<A>> f)
        : this(Eff<RT, A>.Lift(rt => f(rt))) { }

    public FeatureEff(Func<RT, Either<Exceptional, A>> f)
        : this(rt => f(rt).Right(Fin.Succ).Left(Fin.Fail<A>)) { }

    public FeatureEff(Func<RT, Fin<Either<FeatureError, A>>> f)
        : this(Eff<RT, Eff<RT, Either<FeatureError, A>>>
              .Lift(rt => f(rt).ToEff()
                                  .WithRuntime<RT>())
              .Flatten()) { }

    // Constructores desde IO sin runtime
    public FeatureEff(Func<IO<A>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Pure<A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<FeatureError>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Fail<FeatureError>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Exceptional>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Fail<Exceptional>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Either<FeatureError, A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Either<Exceptional, A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Fin<A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<IO<Fin<Either<FeatureError, A>>>> f) : this(_ => f()) { }

    // Constructores desde fiones asincronas sin runtime
    public FeatureEff(Func<Task<A>> f) : this(_ => f()) { }

    public FeatureEff(Func<Task<Pure<A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<Task<FeatureError>> f) : this(_ => f()) { }

    public FeatureEff(Func<Task<Fail<FeatureError>>> f) 
        : this(_ => f()) { }

    public FeatureEff(Func<Task<Exceptional>> f) : this(_ => f()) { }

    public FeatureEff(Func<Task<Fail<Exceptional>>> f) 
        : this(_ => f()) { }

    public FeatureEff(Func<Task<Either<FeatureError, A>>> f) 
        : this(_ => f()) { }

    public FeatureEff(Func<Task<Either<Exceptional, A>>> f) 
        : this(_ => f()) { }

    public FeatureEff(Func<Task<Fin<A>>> f) : this(_ => f()) { }

    public FeatureEff(Func<Task<Fin<Either<FeatureError, A>>>> f)
        : this(_ => f()) { }

    // Constructores desde fiones sincronas sin runtime
    public FeatureEff(Func<A> f) : this(_ => f()) { }

    public FeatureEff(Func<Pure<A>> f) : this(_ => f()) { }

    public FeatureEff(Func<FeatureError> f) 
        : this(_ => f()) { }

    public FeatureEff(Func<Fail<FeatureError>> f)
        : this(_ => f()) { }

    public FeatureEff(Func<Exceptional> f) : this(_ => f()) { }

    public FeatureEff(Func<Fail<Exceptional>> f) : this(_ => f()) { }

    public FeatureEff(Func<Either<FeatureError, A>> f)
        : this(_ => f()) { }

    public FeatureEff(Func<Fin<A>> f) : this(_ => f()) { }

    public FeatureEff(Func<Either<Exceptional, A>> f)
        : this(_ => f()) { }

    public FeatureEff(Func<Fin<Either<FeatureError, A>>> f)
        : this(_ => f()) { }

    public override string ToString() => "FeatureEff";
}
