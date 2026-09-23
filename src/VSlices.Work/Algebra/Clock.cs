using VSlices.Space.Temporal;

namespace VSlices.Work;

/// <summary>
/// Declares that an algebra can express observation of the current semantic moment.
/// </summary>
public interface Clock<ALG>
    where ALG : Functor<ALG>, Clock<ALG>
{
    static abstract K<ALG, Moment> Now();
}

/// <summary>
/// Free constructors for clock-observation operations.
/// </summary>
public static class Clock
{
    public static Free<ALG, Moment> now<ALG>()
        where ALG : Functor<ALG>, Clock<ALG> =>
        Free.lift(ALG.Now());
}
