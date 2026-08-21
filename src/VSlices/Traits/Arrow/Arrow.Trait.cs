namespace VSlices.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="F"></typeparam>
public interface Arrow<F> : Category<F>
    where F : Arrow<F>
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="function"></param>
    /// <returns></returns>
    public static abstract K<F, I, O> Lift<I, O>(
        Func<I, O> function);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="arrow"></param>
    /// <returns></returns>
    public static abstract K<F, (I, X), (O, X)> First<I, O, X>(
        K<F, I, O> arrow);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <typeparam name="X"></typeparam>
    /// <param name="arrow"></param>
    /// <returns></returns>
    public static virtual K<F, (X, I), (X, O)> Second<I, O, X>(
        K<F, I, O> arrow) =>
        F.Compose(
            F.Lift<(X, I), (I, X)>(x => (x.Item2, x.Item1)),
            F.Compose(
                F.First<I, O, X>(arrow),
                F.Lift<(O, X), (X, O)>(x => (x.Item2, x.Item1))));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static virtual K<F, (I1, I2), (O1, O2)> Split<I1, O1, I2, O2>(
        K<F, I1, O1> m1,
        K<F, I2, O2> m2) =>
        F.Compose(
            F.First<I1, O1, I2>(m1),
            F.Second<I2, O2, O1>(m2));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <returns></returns>
    public static virtual K<F, I, (O1, O2)> Fanout<I, O1, O2>(
        K<F, I, O1> m1,
        K<F, I, O2> m2) =>
        F.Compose(F.Lift<I, (I, I)>(v => (v, v)), F.Split(m1, m2));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O1"></typeparam>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="O3"></typeparam>
    /// <param name="m1"></param>
    /// <param name="m2"></param>
    /// <param name="join"></param>
    /// <returns></returns>
    public static virtual K<F, I, O3> Converge<I, O1, O2, O3>(
        K<F, I, O1> m1,
        K<F, I, O2> m2,
        K<F, (O1, O2), O3> join) =>
        F.Compose(F.Fanout(m1, m2), join);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static virtual K<F, I, O> Pure<I, O>(O value) =>
        F.Lift<I, O>(_ => value);
}
