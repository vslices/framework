using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqKindOperatorExtension
{
    extension<IN, A>(K<Req<IN>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static Req<IN, A> operator + (
            K<Req<IN>, A> ma) =>
            ma.As();
    }

}
