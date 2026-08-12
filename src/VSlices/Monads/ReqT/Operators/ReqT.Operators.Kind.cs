using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqKindOperatorExtension
{
    extension<IN, M, A>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <returns></returns>
        public static ReqT<M, IN, A> operator + (
            K<ReqT<M, IN>, A> ma) =>
            ma.As();
    }

}
