using LanguageExt;
using Microsoft.EntityFrameworkCore;
using VSlices.Work;

namespace VSlices.Grounding.EntityFrameworkCore;

/// <summary>
/// Grounds <see cref="DatabaseIO"/> on an Entity Framework Core context without
/// exposing <see cref="DbContext"/> as a Work capability.
/// </summary>
public sealed class EntityFrameworkDatabaseIO<TContext>(
    IO<TContext> context,
    Func<TContext, Type, Repository> repositoryFactory)
    : DatabaseIO
    where TContext : DbContext
{
    public IO<A> Resolve<A>()
        where A : Repository =>
        from dbContext in context
        from repository in IO.lift(() =>
        {
            var resolved = repositoryFactory(dbContext, typeof(A));

            return resolved is A typed
                ? typed
                : throw new InvalidOperationException(
                    $"Repository factory returned '{resolved.GetType().FullName}' " +
                    $"while '{typeof(A).FullName}' was requested.");
        })
        select repository;
}
