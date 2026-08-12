// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, A>(K<Flow<C, R>, A>)
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="mx"></param>
        /// <returns></returns>
        public static Flow<C, R, A> operator +(K<Flow<C, R>, A> mx) =>
            mx.As();
    }
}
