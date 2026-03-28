using System.ComponentModel;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Interfaces;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository;


[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository<RT, TRoot> : IRepository
    where TRoot : AggregateRoot<TRoot>
{
    Eff<RT, Seq<TRoot>> AddRange(Seq<TRoot> roots);

    Eff<RT, Seq<TRoot>> UpdateRange(Seq<TRoot> aggregateRoot);

    Eff<RT, Seq<TRoot>> DeleteRange(Seq<TRoot> aggregateRoot);

}

public interface IRepository<RT, TRoot, TId> : IRepository<RT, TRoot>
    where TRoot : AggregateRoot<TRoot, TId>
    where TId : Identifier<TId>
{
    OptionT<Eff<RT>, TRoot> ReadOrOption(TId id);
}
