using VSlices.Monads;

namespace VSlices;

public static partial class ReqReadableExtensions
{
    extension<M, IN, A, B>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {

    }
}
