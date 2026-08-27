using LanguageExt;

namespace VSlices.Domain.Traits;

public interface Repository;

public interface Repository<ROOT> : Repository
    where ROOT : AggregateRoot<ROOT>
{
    IO<ROOT> Create(ROOT entity);

    IO<Seq<ROOT>> Read();

    IO<ROOT> Update(ROOT entity);

    IO<Unit> Delete(ROOT entity);
}

public interface Repository<ROOT, ID> : Repository<ROOT>
    where ROOT : AggregateRoot<ROOT, ID>
    where ID : Identifier<ID>
{
    OptionT<IO, ROOT> Read(ID id) =>
        OptionT.lift(Read().Map(rs => rs.Single(r => r.Id == id)));

    IO<bool> Any(ID id) =>
        Read().Map(rs => rs.Any(r => r.Id == id));
}
