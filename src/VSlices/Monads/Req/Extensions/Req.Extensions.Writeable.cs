using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqWritableExtensions
{
    extension<IN, A>(K<Req<IN>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public Req<IN, Unit> Tell(Error error) =>
            ma.As().Tell(error);
    }
}
