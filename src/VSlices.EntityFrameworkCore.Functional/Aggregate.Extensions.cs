using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<long> LongCountIO() =>
            liftIO(async env => await query.LongCountAsync(env.Token));

        public IO<A> MinIO() =>
            liftIO(async env => await query.MinAsync(env.Token));

        public IO<A> MaxIO() =>
            liftIO(async env => await query.MaxAsync(env.Token));
    }

    extension(IQueryable<int> query)
    {
        public IO<int> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<int?> query)
    {
        public IO<int?> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double?> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<long> query)
    {
        public IO<long> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<long?> query)
    {
        public IO<long?> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double?> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<float> query)
    {
        public IO<float> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<float> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<float?> query)
    {
        public IO<float?> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<float?> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<double> query)
    {
        public IO<double> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<double?> query)
    {
        public IO<double?> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<double?> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<decimal> query)
    {
        public IO<decimal> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<decimal> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }

    extension(IQueryable<decimal?> query)
    {
        public IO<decimal?> SumIO() =>
            liftIO(async env => await query.SumAsync(env.Token));

        public IO<decimal?> AverageIO() =>
            liftIO(async env => await query.AverageAsync(env.Token));
    }
}
