namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
public static partial class ReqExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="ma"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Fin<OUT> RunFin<IN, OUT>(this Req<IN, OUT, IN, OUT> ma, IN input) =>
        ma.RawRun(input, ReqState.New(input))
            .Match(
                Left: Fin.Fail<OUT>,
                Right: r => r switch
                {
                    (_, { IsEmpty: false } e) => Fin.Fail<OUT>(e),
                    var (v, _) => Fin.Succ(v)
                });
    
    extension<IN, OUT>(Req<IN, OUT, IN, OUT> ma)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Either<Error, ReqState<OUT>> RawRun(IN input) =>
            ma.RawRun(input, ReqState.New(input));
    }
    
    extension<IN, OUT, I, O>(K<Req<IN, OUT, I>, O> m)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Req<IN, OUT, I, O> As() =>
            (Req<IN, OUT, I, O>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public Either<Error, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) =>
            m.As().RawRun(input, previous);
    }

    extension<IN, OUT, I, O>(K<Req<IN, OUT>, I, O> m)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Req<IN, OUT, I, O> AsBi() =>
            (Req<IN, OUT, I, O>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public Either<Error, ReqState<O>> RawRunBi(IN input, Either<Error, ReqState<I>> previous) =>
            m.AsBi().RawRun(input, previous);

    }
}
