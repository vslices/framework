using System.ComponentModel;
using VSlices.Domain.Traits;

namespace VSlices.Domain.Interfaces;

[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository;


[EditorBrowsable(EditorBrowsableState.Never)]
public interface IRepository<RT, A> : IRepository
    where A : AggregateRoot<A>
{
    Eff<RT, Seq<A>> AddRange(Seq<A> roots);

    Eff<RT, Seq<A>> UpdateRange(Seq<A> aggregateRoot);

    Eff<RT, Seq<A>> DeleteRange(Seq<A> aggregateRoot);

}

public interface IRepository<RT, ROOT, ID> : IRepository<RT, ROOT>
    where ROOT : AggregateRoot<ROOT, ID>
    where ID : Identifier<ID>
{
    OptionT<Eff<RT>, ROOT> ReadOrOption(ID id);
}
