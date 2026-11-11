using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFrameworkCore.Functional;

// ReSharper disable once CheckNamespace
public static class DbContextExtensions
{
    extension<T>(IDbContextFactory<T> factory)
        where T : DbContext
    {
        public IO<T> CreateDbContextIO() =>
            liftIO(async env => await factory.CreateDbContextAsync(env.Token));
    }

    extension(DbContext context)
    {
        public IO<EntityEntry<A>> AddIO<A>(A entity)
            where A : class =>
            liftIO(async env => await context.AddAsync(entity, env.Token));

        public IO<EntityEntry<A>> UpdateIO<A>(A entity)
            where A : class =>
            liftIO(env => context.Update(entity).AsTask());

        public IO<EntityEntry<A>> RemoveIO<A>(A entity)
            where A : class =>
            liftIO(_ => context.Remove(entity).AsTask());

        public IO<int> SaveChangesIO() =>
            liftIO(async env => await context.SaveChangesAsync(env.Token));

        public IO<Unit> DisposeIO() =>
            liftIO(async () => await context.DisposeAsync());

        public IO<Unit> ClearIO() =>
            liftIO(_ =>
            {
                context.ChangeTracker.Clear();

                return Task.CompletedTask;
            });

        public IO<Unit> RemoveRangeIO<A>(IEnumerable<A> ma)
            where A : class =>
            liftIO(() =>
            {
                context.RemoveRange(ma);

                return unit.AsTask();
            });

        public IO<Unit> AddRangeIO<A>(IEnumerable<A> ma)
            where A : class =>
            liftIO(async () => await context.AddRangeAsync(ma));
    }
}
