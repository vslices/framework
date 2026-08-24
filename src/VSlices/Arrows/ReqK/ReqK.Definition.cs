namespace VSlices.Arrows;

public sealed record ReqK<M, IN, OUT, I, O>(
    Func<IN, Either<Error, ReqState<I>>, EitherT<Error, M, ReqState<O>>> RawRunF) :
    K<ReqK<M, IN, OUT>, I, O>,
    K<ReqK<M, IN, OUT, I>, O>
    where M : Monad<M>
{
    private Func<IN, Either<Error, ReqState<I>>, EitherT<Error, M, ReqState<O>>> RawRunF { get; } = RawRunF;

    public EitherT<Error, M, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) =>
        RawRunF(input, previous);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Compose<FinO>(
        K<ReqK<M, IN, OUT>, O, FinO> m2) =>
        ReqK<M, IN, OUT, I>.Compose(this, m2);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Compose<O2, FinO>(
        K<ReqK<M, IN, OUT>, O, O2> m2,
        K<ReqK<M, IN, OUT>, O2, FinO> m3) =>
        ReqK<M, IN, OUT, I>.Compose(this, m2, m3);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="O3"></typeparam>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Compose<O2, O3, FinO>(
        K<ReqK<M, IN, OUT>, O, O2> m2,
        K<ReqK<M, IN, OUT>, O2, O3> m3,
        K<ReqK<M, IN, OUT>, O3, FinO> m4) =>
        ReqK<M, IN, OUT, I>.Compose(this, m2, m3, m4);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="I2"></typeparam>
    /// <typeparam name="I3"></typeparam>
    /// <typeparam name="I4"></typeparam>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Compose<I2, I3, I4, FinO>(
        K<ReqK<M, IN, OUT>, O, I2> m2,
        K<ReqK<M, IN, OUT>, I2, I3> m3,
        K<ReqK<M, IN, OUT>, I3, I4> m4,
        K<ReqK<M, IN, OUT>, I4, FinO> m5) =>
        ReqK<M, IN, OUT, I>.Compose(this, m2, m3, m4, m5);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Bind<FinO>(Func<O, ReqK<M, IN, OUT, O, FinO>> f) =>
        ReqK<M, IN, OUT, I>.Bind(this, f);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="O3"></typeparam>
    /// <typeparam name="O4"></typeparam>
    /// <typeparam name="O5"></typeparam>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <param name="m4"></param>
    /// <param name="m5"></param>
    /// <param name="m6"></param>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, FinO> Compose<O2, O3, O4, O5, FinO>(
        K<ReqK<M, IN, OUT>, O, O2> m2,
        K<ReqK<M, IN, OUT>, O2, O3> m3,
        K<ReqK<M, IN, OUT>, O3, O4> m4,
        K<ReqK<M, IN, OUT>, O4, O5> m5,
        K<ReqK<M, IN, OUT>, O5, FinO> m6) =>
        ReqK<M, IN, OUT, I>.Compose(this, m2, m3, m4, m5, m6);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mf"></param>
    public static implicit operator ReqK<M, IN, OUT, I, O>(Pure<O> mf) =>
        ReqK<M, IN, OUT, I>.Accept(mf.Value);
}
