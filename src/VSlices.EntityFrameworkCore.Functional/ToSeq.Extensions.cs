using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Functional;

public static partial class QueryableExtensions
{
    extension<A>(IQueryable<A> query)
    {
        public IO<Seq<A>> ToSeqIO() =>
            liftIO(async env => await query.ToArrayAsync(env.Token))
                .Map(e => e.AsIterable().ToSeq());
    }
}
