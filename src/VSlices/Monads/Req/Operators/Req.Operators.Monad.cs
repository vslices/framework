using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqMonadOperatorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Req<IN, B> operator >> (
            K<Req<IN>, A> ma,
            Func<A, K<Req<IN>, B>> f) =>
            +ma.Bind(f);

        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static Req<IN, B> operator >> (
            K<Req<IN>, A> ma,
            K<Req<IN>, B> mb) =>
            ma >> (_ => mb);
    }

    extension<IN, A>(K<Req<IN>, A>)
    {
        public static Req<IN, A> operator >> (
            K<Req<IN>, A> ma, K<Req<IN>, Unit> mb) =>
            ma >> (a => mb * (_ => a));
    }
}
