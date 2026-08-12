using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqFunctorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="fb"></param>
        /// <returns></returns>
        public Req<IN, B> Map(Func<A, B> fb) =>
            ma.As().Map(fb);
    }
}
