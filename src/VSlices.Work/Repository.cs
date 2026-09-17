using LanguageExt;

namespace VSlices.Work;

public interface Repository;

public interface Repository<A> : Repository
{
    IO<A> Create(A value);
    IO<Seq<A>> Read();
    IO<A> Update(A value);
    IO<Unit> Delete(A value);
}

public interface Repository<A, ID> : Repository<A>
{
    OptionT<IO, A> Read(ID id);
    IO<bool> Any(ID id);
}
