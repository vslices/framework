namespace VSlices.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="F"></typeparam>
public interface Category<F>
    where F : Category<F>
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <returns></returns>
    public static abstract K<F, A, A> Identity<A>();

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static abstract K<F, I1, O> Compose<I1, I2, O>(
        K<F, I1, I2> first,
        K<F, I2, O> second);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <returns></returns>
    public static virtual K<F, I1, O> Compose<I1, I2, I3, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, O> third) =>
        F.Compose(first, F.Compose(second, third));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <param name="fourth"></param>
    /// <returns></returns>
    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, O> fourth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, fourth)));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <param name="fourth"></param>
    /// <param name="fifth"></param>
    /// <returns></returns>
    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, I5, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, O> fifth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, fifth))));

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="I1"></typeparam>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="I5"></typeparam>
    /// <typeparam name="I6"></typeparam>
    /// <typeparam name="O"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="third"></param>
    /// <param name="fourth"></param>
    /// <param name="fifth"></param>
    /// <param name="sixth"></param>
    /// <returns></returns>
    public static virtual K<F, I1, O> Compose<I1, I2, I3, I4, I5, I6, O>(
        K<F, I1, I2> first,
        K<F, I2, I3> second,
        K<F, I3, I4> third,
        K<F, I4, I5> fourth,
        K<F, I5, I6> fifth,
        K<F, I6, O> sixth) =>
        F.Compose(first, F.Compose(second, F.Compose(third, F.Compose(fourth, F.Compose(fifth, sixth)))));
}
