using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
public static partial class ReqKExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="ma"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static FinT<M, OUT> RunFinT<M, IN, OUT>(this ReqK<M, IN, OUT, IN, OUT> ma, IN input)
        where M : Monad<M> =>
        FinT.lift(ma.RawRun(input).Run()
                    .Map(either => either.Match(
                        Left: Fin.Fail<OUT>,
                        Right: s => s.IsValid ? Fin.Succ(s.Value) : Fin.Fail<OUT>(s.Error))));

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OUT"></typeparam>
    /// <param name="req"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public static Eff<RT, OUT> RunEff<RT, IN, OUT>(
        this ReqK<Eff<RT>, IN, OUT, IN, OUT> req,
        IN input) =>
        +req.RunFinT(input).Run()
            .Bind(m => m.Match(Succ: Eff<RT, OUT>.Pure, Fail: Eff<RT, OUT>.Fail));

    public static Eff<RT, OUT> RunEff<RT, IN, OUT>(
        this ReqK<Eff<RT>, IN, OUT>.Full req,
        IN input) =>
        req.Value.RunEff(input);

    public static Eff<RT, IN> RunEff<RT, IN>(
        this ReqK<Eff<RT>, IN>.Full req,
        IN input) =>
        req.Value.RunEff(input);

    extension<M, IN, OUT>(ReqK<M, IN, OUT, IN, OUT> ma)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<OUT>> RawRun(IN input) =>
            ma.RawRun(input, ReqState.New(input));
    }

    extension<M, IN, OUT, I, O>(K<ReqK<M, IN, OUT, I>, O> m)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReqK<M, IN, OUT, I, O> As() =>
            (ReqK<M, IN, OUT, I, O>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<O>> RawRun(IN input, Either<Error, ReqState<I>> previous) =>
            m.As().RawRun(input, previous);
    }

    extension<M, IN, OUT, I, O>(K<ReqK<M, IN, OUT>, I, O> m)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReqK<M, IN, OUT, I, O> AsBi() =>
            (ReqK<M, IN, OUT, I, O>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<O>> RawRunBi(IN input, Either<Error, ReqState<I>> previous) =>
            m.AsBi().RawRun(input, previous);

    }
}
