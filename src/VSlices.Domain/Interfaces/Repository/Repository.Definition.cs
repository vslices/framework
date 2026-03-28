using System.ComponentModel;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Interfaces;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository;


[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository<M, TRoot> : IRepository
    where M : MonadIO<M>
    where TRoot : AggregateRoot<TRoot>
{
    K<M, Seq<TRoot>> AddRange(Seq<TRoot> roots);

    K<M, Seq<TRoot>> UpdateRange(Seq<TRoot> aggregateRoot);

    K<M, Seq<TRoot>> DeleteRange(Seq<TRoot> aggregateRoot);

}

public interface IRepository<M, TRoot, TId> : IRepository<M, TRoot>
    where M : MonadIO<M>
    where TRoot : AggregateRoot<TRoot, TId>
    where TId : Identifier<TId>
{
    OptionT<M, TRoot> ReadOrOption(TId id);
}
