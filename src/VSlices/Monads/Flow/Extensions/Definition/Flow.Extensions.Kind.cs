// Resharper disable CheckNamespace

using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<C, R, A>(K<Flow<C, R>, A> ma)
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Flow<C, R, A> As() =>
            (Flow<C, R, A>)ma;
    }
}
