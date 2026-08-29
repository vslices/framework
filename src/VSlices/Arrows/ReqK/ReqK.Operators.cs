namespace VSlices.Arrows;

/// <summary>
/// 
/// </summary>
public static partial class ReqKOperators
{
    extension<M, IN, OUT, I, O>(K<ReqK<M, IN, OUT>, I, O>)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static ReqK<M, IN, OUT, I, O> operator +(
            K<ReqK<M, IN, OUT>, I, O> ma) =>
            ma.AsBi();
    }

    extension<M, IN, OUT, I, O, FinO>(K<ReqK<M, IN, OUT>, I, O>)
        where M : Monad<M>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static ReqK<M, IN, OUT, I, FinO> operator >>(
            K<ReqK<M, IN, OUT>, I, O> ma,
            K<ReqK<M, IN, OUT>, O, FinO> mb) =>
            ReqK<M, IN, OUT>.Compose(ma, mb);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="fb"></param>
        /// <returns></returns>
        public static ReqK<M, IN, OUT, I, FinO> operator *(
            K<ReqK<M, IN, OUT>, I, O> ma,
            Func<O, FinO> fb) =>
            ma >> ReqK<M, IN, OUT>.Transform(fb);

    }
}