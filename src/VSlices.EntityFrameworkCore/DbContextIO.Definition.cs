using LanguageExt;
using LanguageExt.Traits;
using Microsoft.EntityFrameworkCore;

namespace VSlices.EntityFrameworkCore;

public interface HasDbContext<RT, TContext> :
    Has<Eff<RT>, DbContextIO<TContext>>
    where TContext : DbContext;

public interface DbContextIO<TContext>
    where TContext : DbContext
{
    IO<TContext> Context { get; }
}
