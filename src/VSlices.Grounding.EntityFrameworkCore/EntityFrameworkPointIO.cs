using System.Linq.Expressions;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using VSlices.Work;

namespace VSlices.Grounding.EntityFrameworkCore;

/// <summary>
/// Grounds point-oriented Work capabilities on an Entity Framework Core projection.
/// This type does not introduce repository semantics.
/// </summary>
public sealed class EntityFrameworkPointIO<TContext, POINT, ID, TProjection>(
    TContext context,
    Func<POINT, TProjection> toProjection,
    Func<TProjection, POINT> toPoint,
    Func<POINT, ID> identity,
    Func<ID, Expression<Func<TProjection, bool>>> byId)
    : PointReader<POINT, ID>,
      PointWriter<POINT>,
      PointRemover<POINT, ID>
    where TContext : DbContext
    where TProjection : class
{
    private readonly DbSet<TProjection> set = context.Set<TProjection>();

    public OptionT<IO, POINT> ReadOrDefault(ID id) =>
        OptionT.lift<IO, POINT>(
            Prelude.liftIO(async env =>
            {
                var projection = await set.AsNoTracking()
                    .SingleOrDefaultAsync(byId(id), env.Token)
                    .ConfigureAwait(false);

                return projection is null
                    ? Option<POINT>.None
                    : Prelude.Some(toPoint(projection));
            }));

    public IO<Unit> Write(POINT point) =>
        Prelude.liftIO(async env =>
        {
            var projection = toProjection(point);
            var exists = await set.AsNoTracking()
                .AnyAsync(byId(identity(point)), env.Token)
                .ConfigureAwait(false);

            var entry = exists
                ? context.Update(projection)
                : await context.AddAsync(projection, env.Token)
                    .ConfigureAwait(false);

            await context.SaveChangesAsync(env.Token).ConfigureAwait(false);
            entry.State = EntityState.Detached;
            return default(Unit);
        });

    public IO<Unit> Remove(ID id) =>
        Prelude.liftIO(async env =>
        {
            var existing = await set.AsNoTracking()
                .SingleOrDefaultAsync(byId(id), env.Token)
                .ConfigureAwait(false);

            if (existing is null)
                return default(Unit);

            var entry = context.Remove(existing);
            await context.SaveChangesAsync(env.Token).ConfigureAwait(false);
            entry.State = EntityState.Detached;
            return default(Unit);
        });
}

public static class EntityFrameworkPointIO
{
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
