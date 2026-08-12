using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqMonadExtensions
{
    extension<M, IN, A, B>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public ReqT<M, IN, B> Bind(Func<A, K<Req<IN>, B>> fb) =>
            ma.As().Bind(fb);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public ReqT<M, IN, B> Bind(Func<A, Req<IN, B>> fb) =>
            ma.As().Bind(fb);
    }
}
