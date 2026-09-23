using System.Linq.Expressions;
using EntityFrameworkCore.Functional;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using VSlices.Work;

namespace VSlices.Grounding.EntityFrameworkCore;

/// <summary>
/// Grounds point-oriented Work capabilities on an Entity Framework Core projection.
///
/// This type deliberately does not expose Repository semantics. It realizes only the
/// point operations explicitly requested by Work.
/// </summary>
public sealed class EntityFrameworkPointIO<TContext, POINT, ID, TProjection>(
    TContext context,
    Func<POINT, TProjection> toProjection,
    Func<TProjection, POINT> toPoint,
    Func<POINT, ID> identity,
    Func<ID, Expression<Func<TProjection, bool>>> byId)
    : PointReaderIO<POINT, ID>,
      PointWriterIO<POINT>,
      PointRemoverIO<POINT, ID>
    where TContext : DbContext
    where TProjection : class
{
    private readonly DbSet<TProjection> set = context.Set<TProjection>();

    public IO<Option<POINT>> Read(ID id) =>
        set.AsNoTracking()
            .SingleOrNoneIO(byId(id))
            .Run()
            .As()
            .Map(option => option.Map(toPoint));

    public IO<Unit> Write(POINT point) =>
        from projection in IO.lift(() => toProjection(point))
        from exists in set
            .AsNoTracking()
            .AnyIO(byId(identity(point)))
        from entry in (
            exists
                ? context.UpdateIO(projection)
                : context.AddIO(projection))
        from _ in context.SaveChangesIO()
        from __ in Detach(entry)
        select default(Unit);

    public IO<Unit> Remove(ID id) =>
        from existing in set
            .AsNoTracking()
            .SingleOrNoneIO(byId(id))
            .Run()
            .As()
        from removed in existing.Match(
            Some: projection =>
                from entry in context.RemoveIO(projection)
                from _ in context.SaveChangesIO()
                from __ in Detach(entry)
                select default(Unit),
            None: static () =>
                IO.pure(default(Unit)))
        select removed;

    private static IO<TProjection> Detach(
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TProjection> entry) =>
        IO.lift(() =>
        {
            entry.State = EntityState.Detached;
            return entry.Entity;
        });
}

public static class EntityFrameworkPointIO
{
    /// <summary>
    /// Creates point grounding when the semantic point is also the EF entity.
    /// </summary>
    public static EntityFrameworkPointIO<TContext, POINT, ID, POINT>
        Direct<TContext, POINT, ID>(
            TContext context,
            Func<POINT, ID> identity,
            Func<ID, Expression<Func<POINT, bool>>> byId)
        where TContext : DbContext
        where POINT : class =>
        new(
            context,
            static point => point,
            static point => point,
            identity,
            byId);
}
