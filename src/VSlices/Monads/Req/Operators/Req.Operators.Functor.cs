using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqFunctorOperatorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Req<IN, B> operator * (
            K<Req<IN>, A> ma,
            Func<A, B> f) =>
            +ma.Map(f);

        public static Req<IN, B> operator * (
            Func<A, B> f,
            K<Req<IN>, A> ma) =>
            ma * f;
    }
}
