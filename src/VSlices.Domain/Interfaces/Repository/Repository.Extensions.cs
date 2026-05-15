using VSlices.Domain;

namespace VSlices;

public static class RepositoryExtensions
{
    extension<RT, TRoot>(IRepository<RT, TRoot> repository)
        where TRoot : AggregateRoot<TRoot>
    {
        public Eff<RT, TRoot> Add(TRoot root) =>
            repository.AddRange([root]).Map(r => r.First());

        public Eff<RT, TRoot> Update(TRoot root) =>
            repository.UpdateRange([root]).Map(r => r.First());

        public Eff<RT, TRoot> Delete(TRoot root) =>
            repository.DeleteRange([root]).Map(r => r.First());
    }

    extension<RT, TRoot, TId>(IRepository<RT, TRoot, TId> repository)
        where TRoot : AggregateRoot<TRoot, TId>
        where TId : Identifier<TId>
    {
        public Eff<RT, TRoot> Read(TId id) =>
            repository.ReadOrOption(id)
                      .Match(Some: Eff<RT, TRoot> (s) => Pure(s),
                             None: Eff<RT, TRoot> () => Error.New(404, $"Entity {typeof(TRoot).Name} NotFound"))
                      .As()
                      .Flatten();
    }
}
