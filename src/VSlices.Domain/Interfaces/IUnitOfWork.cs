namespace VSlices.Domain.Interfaces;

public interface IUnitOfWork<RT>
{
    Eff<RT, Unit> Commit();

    Eff<RT, T> GetRepository<T>()
        where T : IRepository;
}
