using LanguageExt;
using LanguageExt.Traits;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCore.Functional;

namespace VSlices.EntityFrameworkCore;

public static class DbContextEnv<RT, TContext>
    where RT : HasDbContext<RT, TContext>
    where TContext : DbContext
{
    public static Eff<RT, TContext> getContext =>
        Has<Eff<RT>, RT, DbContextIO<TContext>>
            .ask
            .As()
            .Bind(io => io.Context);

    public static Eff<RT, A> use<A>(Func<TContext, IO<A>> f, bool Dispose = false) =>
        from dbCtx in getContext
        from result in f(dbCtx)
        from _1 in Dispose ? dbCtx.DisposeIO() : IO.pure(Prelude.unit)
        select result;

}
