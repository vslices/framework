// Resharper disable CheckNamespace
using VSlices.Monads;

namespace VSlices;

public static partial class FlowExtensions
{
    extension<S, R, A>(K<Flow<S, R>, A>)
        where A : notnull
    {
        public static Flow<S, R, A> operator +(K<Flow<S, R>, A> mx) =>
            mx.As();
    }
}
