namespace VSlices.Work;

/// <summary>
/// Declares that an algebra can express writing a point of a semantic space.
/// </summary>
/// <typeparam name="ALG">The service-owned algebra that carries the operation.</typeparam>
/// <typeparam name="POINT">The semantic point type.</typeparam>
public interface PointWriter<ALG, POINT>
    where ALG : Functor<ALG>, PointWriter<ALG, POINT>
{
    static abstract K<ALG, Unit> Write(POINT point);
}

/// <summary>
/// Free constructors for point-writing operations.
/// </summary>
public static class PointWriter
{
    public static Free<ALG, Unit> write<ALG, POINT>(POINT point)
        where ALG : Functor<ALG>, PointWriter<ALG, POINT> =>
        Free.lift(ALG.Write(point));
}
