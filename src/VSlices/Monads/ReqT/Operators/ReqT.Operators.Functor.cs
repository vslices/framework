using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqFunctorOperatorExtensions
{
    extension<IN, M, A, B>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static ReqT<M, IN, B> operator * (
            K<ReqT<M, IN>, A> ma,
            Func<A, B> f) =>
            +ma.Map(f);
    }
}
