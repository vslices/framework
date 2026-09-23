using VSlices.Space.Quantities;
using VSlices.Space.Temporal;

namespace VSlices.Work;

/// <summary>
/// Declares that an algebra can express temporal delay independently of clock observation.
/// </summary>
public interface Delay<ALG>
    where ALG : Functor<ALG>, Delay<ALG>
{
    static abstract K<ALG, Unit> For(Duration<double> duration);
    static abstract K<ALG, Unit> Until(Moment moment);
}

/// <summary>
/// Free constructors for temporal-delay operations.
/// </summary>
public static class Delay
{
    public static Free<ALG, Unit> forDuration<ALG>(Duration<double> duration)
        where ALG : Functor<ALG>, Delay<ALG> =>
        Free.lift(ALG.For(duration));

    public static Free<ALG, Unit> until<ALG>(Moment moment)
        where ALG : Functor<ALG>, Delay<ALG> =>
        Free.lift(ALG.Until(moment));
}
