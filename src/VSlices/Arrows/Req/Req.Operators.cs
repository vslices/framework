namespace VSlices.Monads;

/// <summary>
/// 
/// </summary>
public static partial class ReqOperators
{
    extension<IN, OUT, I, O>(K<Req<IN, OUT>, I, O>)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static Req<IN, OUT, I, O> operator +(
            K<Req<IN, OUT>, I, O> ma) =>
            ma.AsBi();
    }

    extension<IN, OUT, I, O, FinO>(K<Req<IN, OUT>, I, O>)
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static Req<IN, OUT, I, FinO> operator >>(
            K<Req<IN, OUT>, I, O> ma,
            K<Req<IN, OUT>, O, FinO> mb) =>
            Req<IN, OUT>.Compose(ma, mb);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="fb"></param>
        /// <returns></returns>
        public static Req<IN, OUT, I, FinO> operator *(
            K<Req<IN, OUT>, I, O> ma,
            Func<O, FinO> fb) =>
            ma >> Req<IN, OUT>.Transform(fb);

    }
}