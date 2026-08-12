using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqWritableExtensions
{
    extension<M, IN, A>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public ReqT<M, IN, Unit> Tell(Error error) =>
            ma.As().Tell(error);
    }
}
