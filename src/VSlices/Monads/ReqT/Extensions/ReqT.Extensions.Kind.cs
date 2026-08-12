using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class ReqTKindExtensions
{
    extension<M, IN, A>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public ReqT<M, IN, A> As() =>
            (ReqT<M, IN, A>)ma;

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FinT<M, A> Onto(IN input) =>
            ma.As().Onto(input);
    }

}
