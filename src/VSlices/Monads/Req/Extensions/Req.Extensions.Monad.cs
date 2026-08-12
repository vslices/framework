using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqMonadExtensions
{
    extension<IN, A, B>(K<Req<IN>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Req<IN, B> Bind(Func<A, K<Req<IN>, B>> fb) =>
            ma.As().Bind(fb);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Req<IN, B> Bind(Func<A, Req<IN, B>> fb) =>
            ma.As().Bind(fb);
    }
}
