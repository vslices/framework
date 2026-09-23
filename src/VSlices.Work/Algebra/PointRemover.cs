namespace VSlices.Work;

/// <summary>
/// Declares that an algebra can express removing a point from a semantic space by identity.
/// </summary>
/// <typeparam name="ALG">The work-owned algebra that carries the operation.</typeparam>
/// <typeparam name="POINT">The semantic point type.</typeparam>
/// <typeparam name="ID">The identity used to locate the point.</typeparam>
public interface PointRemover<ALG, POINT, ID>
    where ALG : Functor<ALG>, PointRemover<ALG, POINT, ID>
{
    static abstract K<ALG, Unit> Remove(ID id);
}

/// <summary>
/// Free constructors for point-removal operations.
/// </summary>
public static class PointRemover
{
    public static Free<ALG, Unit> remove<ALG, POINT, ID>(ID id)
        where ALG : Functor<ALG>, PointRemover<ALG, POINT, ID> =>
        Free.lift(ALG.Remove(id));
}
