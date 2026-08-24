namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
/// <typeparam name="IN"></typeparam>
/// <typeparam name="OUT"></typeparam>
/// <typeparam name="I"></typeparam>
/// <typeparam name="O"></typeparam>
/// <param name="RawRunF"></param>
public sealed record Req<IN, OUT, I, O>(Func<IN, Either<Error, ReqState<I>>, Either<Error, ReqState<O>>> RawRunF) :
    K<Req<IN, OUT>, I, O>,
    K<Req<IN, OUT, I>, O>
{
    private Func<IN, Either<Error, ReqState<I>>, Either<Error, ReqState<O>>> RawRunF { get; } = RawRunF;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="previous"></param>
    /// <returns></returns>
    public Either<Error, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) => 
        RawRunF(input, previous);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <returns></returns>
    public Req<IN, OUT, I, FinO> Compose<FinO>(
        K<Req<IN, OUT>, O, FinO> m2) =>
        Req<IN, OUT, I>.Compose(this, m2);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="O2"></typeparam>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="m2"></param>
    /// <param name="m3"></param>
    /// <returns></returns>
    public Req<IN, OUT, I, FinO> Compose<O2, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, FinO> m3) =>
        Req<IN, OUT, I>.Compose(this, m2, m3);

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
    public Req<IN, OUT, I, FinO> Compose<O2, O3, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, O3> m3,
        K<Req<IN, OUT>, O3, FinO> m4) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4);

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
    public Req<IN, OUT, I, FinO> Compose<I2, I3, I4, FinO>(
        K<Req<IN, OUT>, O, I2> m2,
        K<Req<IN, OUT>, I2, I3> m3,
        K<Req<IN, OUT>, I3, I4> m4,
        K<Req<IN, OUT>, I4, FinO> m5) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4, m5);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="FinO"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    public Req<IN, OUT, I, FinO> Bind<FinO>(Func<O, Req<IN, OUT, O, FinO>> f) =>
        Req<IN, OUT, I>.Bind(this, f);

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
    public Req<IN, OUT, I, FinO> Compose<O2, O3, O4, O5, FinO>(
        K<Req<IN, OUT>, O, O2> m2,
        K<Req<IN, OUT>, O2, O3> m3,
        K<Req<IN, OUT>, O3, O4> m4,
        K<Req<IN, OUT>, O4, O5> m5,
        K<Req<IN, OUT>, O5, FinO> m6) =>
        Req<IN, OUT, I>.Compose(this, m2, m3, m4, m5, m6);
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="M"></typeparam>
    /// <returns></returns>
    public ReqK<M, IN, OUT, I, O> ToK<M>()
        where M : Monad<M> =>
        ReqK<M, IN, OUT>.Lift(this);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mf"></param>
    public static implicit operator Req<IN, OUT, I, O>(Pure<O> mf) =>
        Req<IN, OUT, I>.Accept(mf.Value);
}
