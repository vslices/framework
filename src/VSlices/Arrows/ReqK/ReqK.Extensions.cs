// ReSharper disable InconsistentNaming

using VSlices.Monads;

namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
public static partial class ReqKExtensions
{
    /// <param name="mb"></param>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    extension<M, A, B>(ReqK<M, A, B, A, B> mb)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public FinT<M, B> RunFinT(K<M, A> ma) =>
            from value in FinT.lift(ma)
            let runned = mb.RawRun(value).Run()
            from result in FinT.lift(runned.Map(a => a.Match(
                Left: Fin.Fail<B>,
                Right: s => s.IsValid ? Fin.Succ(s.Value) : Fin.Fail<B>(s.Error))))
            select result;

        public FinT<M, B> RunFinT(A a) =>
            mb.RunFinT(M.Pure(a));
    }

    extension<M, A, B>(ReqK<M, A, A, B, B> req)
        where M : Monad<M>
    {
        public ReqK<M, A, A, A, A> ApplyTo(Func<A, B> fb) =>
            ReqK<M, A, A>.Apply(req, fb);
    }

    /// <param name="req"></param>
    /// <typeparam name="RT"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="B"></typeparam>
    extension<RT, A, B>(ReqK<Eff<RT>, A, B, A, B> req)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Eff<RT, B> RunEff(A input) =>
            +req.RunFinT(input).Run()
                .Bind(m => m.Match(Succ: Eff<RT, B>.Pure, Fail: Eff<RT, B>.Fail));
    }

    extension<RT, A, B>(ReqK<Eff<RT>, A, B>.Full req)
    {
        public Eff<RT, B> RunEff(A input) =>
            req.Value.RunEff(input);
    }

    extension<RT, A>(ReqK<Eff<RT>, A>.Full req)
    {
        public Eff<RT, A> RunEff(A input) =>
            req.Value.RunEff(input);
    }

    extension<M, A, B>(ReqK<M, A, B, A, B> ma)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<B>> RawRun(A input) =>
            ma.RawRun(input, ReqState.New(input));
    }

    extension<M, A, B, C, D>(K<ReqK<M, A, B, C>, D> m)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReqK<M, A, B, C, D> As() =>
            (ReqK<M, A, B, C, D>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<D>> RawRun(A input, Either<Error, ReqState<C>> previous) =>
            m.As().RawRun(input, previous);
    }

    extension<M, A, B, C, D>(K<ReqK<M, A, B>, C, D> m)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ReqK<M, A, B, C, D> AsBi() =>
            (ReqK<M, A, B, C, D>)m;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="previous"></param>
        /// <returns></returns>
        public EitherT<Error, M, ReqState<D>> RawRunBi(A input, Either<Error, ReqState<C>> previous) =>
            m.AsBi().RawRun(input, previous);

    }
}
