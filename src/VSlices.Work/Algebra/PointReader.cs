namespace VSlices.Work;

/// <summary>
/// Declares that an algebra can express reading a point from a semantic space by identity.
/// </summary>
/// <typeparam name="ALG">The service-owned algebra that carries the operation.</typeparam>
/// <typeparam name="POINT">The semantic point type.</typeparam>
/// <typeparam name="ID">The identity used to locate the point.</typeparam>
public interface PointReader<ALG, POINT, ID>
    where ALG : Functor<ALG>, PointReader<ALG, POINT, ID>
{
    static abstract K<ALG, Option<POINT>> Read(ID id);
}

/// <summary>
/// Free constructors for point-reading operations.
/// </summary>
public static class PointReader
{
    public static Free<ALG, Option<POINT>> read<ALG, POINT, ID>(ID id)
        where ALG : Functor<ALG>, PointReader<ALG, POINT, ID> =>
        Free.lift(ALG.Read(id));
}
