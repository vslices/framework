namespace VSlices.Traits;

/// <summary>
///
/// </summary>
public static class Arrow
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public static K<F, I, O> Lift<F, I, O>(Func<I, O> f)
        where F : Arrow<F> =>
        F.Lift(f);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static K<F, (I, X), (O, X)> First<F, I, O, X>(K<F, I, O> m)
        where F : Arrow<F> =>
        F.First<I, O, X>(m);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="m"></param>
    /// <returns></returns>
    public static K<F, (X, I), (X, O)> Second<F, I, O, X>(K<F, I, O> m)
        where F : Arrow<F> =>
        F.Second<I, O, X>(m);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static K<F, (I1, I2), (O1, O2)> Split<F, I1, O1, I2, O2>(
        K<F, I1, O1> m1,
        K<F, I2, O2> m2)
        where F : Arrow<F> =>
        F.Split(m1, m2);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static K<F, I, (O1, O2)> Fanout<F, I, O1, O2>(
        K<F, I, O1> m1,
        K<F, I, O2> m2)
        where F : Arrow<F> =>
        F.Fanout(m1, m2);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="O3"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="join"></param>
    /// <returns></returns>
    public static K<F, I, O3> Converge<F, I, O1, O2, O3>(
        K<F, I, O1> m1,
        K<F, I, O2> m2,
        K<F, (O1, O2), O3> join)
        where F : Arrow<F> =>
        F.Converge(m1, m2, join);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="F"></typeparam>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static K<F, I, O> Pure<F, I, O>(O value) 
        where F : Arrow<F> =>
        F.Pure<I, O>(value); 
}
