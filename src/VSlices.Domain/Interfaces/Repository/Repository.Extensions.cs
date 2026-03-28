using VSlices.Domain.Traits;

namespace VSlices.Domain.Interfaces;

public static class RepositoryExtensions
{
    extension<M, TRoot>(IRepository<M, TRoot> repository)
        where M : MonadIO<M>
        where TRoot : AggregateRoot<TRoot>
    {
        public K<M, TRoot> Add(TRoot root) =>
            repository.AddRange([root]).Map(r => r.First());

        public K<M, TRoot> Update(TRoot root) =>
            repository.UpdateRange([root]).Map(r => r.First());

        public K<M, TRoot> Delete(TRoot root) =>
            repository.DeleteRange([root]).Map(r => r.First());
    }

    extension<M, TRoot, TId>(IRepository<M, TRoot, TId> repository)
        where M : MonadIO<M>, Fallible<Error, M>
        where TRoot : AggregateRoot<TRoot, TId>
        where TId : Identifier<TId>
    {
        public K<M, TRoot> Read(TId id) =>
            repository.ReadOrOption(id)
                      .Match(Some: (TRoot s) => M.Pure(s),
                             None: () => M.Fail<TRoot>(notFound<TRoot>().Value))
                      .Flatten();
    }
}
