using System;
using System.Collections.Generic;
using System.Text;
using VSlices.Monads;

namespace VSlices;

public static partial class VSlicesPrelude
{
    // IO<A>
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, IO<A>> f) =>
        Flow<C, R>.LiftIO(f);
    
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, EnvIO, Task<A>> f) =>
        Flow<C, R>.LiftIO(f);
    
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, A> f) =>
        Flow<C, R>.LiftIO<A>((c, r) => f(c, r));

    // FinT<IO, A>
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, FinT<IO, A>> f) =>
        liftFlow((C c, R r) =>
            f(c, r).Run().As().Bind<A>(ma => ma.Match(IO.pure, IO.fail<A>)));
    
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, EnvIO, Task<Fin<A>>> f) =>
        liftFlow((C c, R r) => FinT.lift(IO.liftAsync(e => f(c, r, e))));
    
    public static Flow<C, R, A> liftFlow<C, R, A>(Func<C, R, Fin<A>> f) =>
        liftFlow((C c, R r) => FinT.lift<IO, A>(f(c, r)));

}
