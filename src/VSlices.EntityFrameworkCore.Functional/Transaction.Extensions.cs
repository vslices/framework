using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EntityFrameworkCore.Functional;

public static class TransactionExtensions
{
    extension(DbContext context)
    {
        public IO<IDbContextTransaction> BeginTransactionIO() =>
            liftIO(async env => await context.Database.BeginTransactionAsync(env.Token));
    }

    extension(IDbContextTransaction transaction)
    {
        public IO<Unit> CommitIO() =>
            liftIO(async env =>
            {
                await transaction.CommitAsync(env.Token);
                return unit;
            });

        public IO<Unit> RollbackIO() =>
            liftIO(async env =>
            {
                await transaction.RollbackAsync(env.Token);
                return unit;
            });

        public IO<Unit> DisposeIO() =>
            liftIO(async () =>
            {
                await transaction.DisposeAsync();
                return unit;
            });
    }
}
