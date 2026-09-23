using System.Linq.Expressions;
using EntityFrameworkCore.Functional;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using VSlices.Work;

namespace VSlices.Grounding.EntityFrameworkCore;

/// <summary>
/// Grounds <see cref="Repository{A}"/> on an Entity Framework Core projection.
/// </summary>
/// <typeparam name="TContext">Entity Framework Core context.</typeparam>
/// <typeparam name="A">Semantic value exposed by Work.</typeparam>
/// <typeparam name="TProjection">Persistence representation owned by this Grounding.</typeparam>
public sealed class EntityFrameworkRepository<TContext, A, TProjection>(
    TContext context,
    Func<A, TProjection> toProjection,
    Func<TProjection, A> toValue)
    : Repository<A>
    where TContext : DbContext
    where TProjection : class
{
    private readonly DbSet<TProjection> _set = context.Set<TProjection>();

    public IO<A> Create(A value) =>
        from projection in IO.lift(() => toProjection(value))
        from entry in context.AddIO(projection)
        from _ in context.SaveChangesIO()
        from persisted in Detach(entry)
        select toValue(persisted);

    public IO<Seq<A>> Read() =>
        _set.AsNoTracking()
            .ToSeqIO()
            .Map(values => values.Map(toValue));

    public IO<A> Update(A value) =>
        from projection in IO.lift(() => toProjection(value))
        from entry in context.UpdateIO(projection)
        from _ in context.SaveChangesIO()
        from persisted in Detach(entry)
        select toValue(persisted);

    public IO<Unit> Delete(A value) =>
        from projection in IO.lift(() => toProjection(value))
        from entry in context.RemoveIO(projection)
        from _ in context.SaveChangesIO()
        from __ in Detach(entry)
        select default(Unit);

    private static IO<TProjection> Detach(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TProjection> entry) =>
        IO.lift(() =>
        {
            entry.State = EntityState.Detached;

            return entry.Entity;
        });
}

/// <summary>
/// Grounds <see cref="Repository{A, ID}"/> on an Entity Framework Core projection
/// using a storage-native identity predicate.
/// </summary>
public sealed class EntityFrameworkRepository<TContext, A, ID, TProjection>(
    TContext context,
    Func<A, TProjection> toProjection,
    Func<TProjection, A> toValue,
    Func<ID, Expression<Func<TProjection, bool>>> byId)
    : Repository<A, ID>
    where TContext : DbContext
    where TProjection : class
{
    private readonly EntityFrameworkRepository<TContext, A, TProjection> _repository =
        new(context, toProjection, toValue);

    private readonly DbSet<TProjection> _set = context.Set<TProjection>();

    public IO<A> Create(A value) =>
        _repository.Create(value);

    public IO<Seq<A>> Read() =>
        _repository.Read();

    public IO<A> Update(A value) =>
        _repository.Update(value);

    public IO<Unit> Delete(A value) =>
        _repository.Delete(value);

    public OptionT<IO, A> Read(ID id) =>
        _set.AsNoTracking()
            .SingleOrNoneIO(byId(id))
            .Map(toValue);

    public IO<bool> Any(ID id) =>
        _set.AsNoTracking()
            .AnyIO(byId(id));
}

public static class EntityFrameworkRepository
{
    /// <summary>
    /// Creates a repository when the semantic value is also the Entity Framework entity.
    /// </summary>
    public static EntityFrameworkRepository<TContext, A, A> Direct<TContext, A>(
        TContext context)
        where TContext : DbContext
        where A : class =>
        new(context, static value => value, static value => value);

    /// <summary>
    /// Creates an identified repository when the semantic value is also the
    /// Entity Framework entity.
    /// </summary>
    public static EntityFrameworkRepository<TContext, A, ID, A> Direct<TContext, A, ID>(
        TContext context,
        Func<ID, Expression<Func<A, bool>>> byId)
        where TContext : DbContext
        where A : class =>
        new(context, static value => value, static value => value, byId);
}
