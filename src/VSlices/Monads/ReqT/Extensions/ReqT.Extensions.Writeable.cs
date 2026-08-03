using VSlices.Monads;

namespace VSlices;

public static partial class ReqWritableExtensions
{
    extension<M, IN, A>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        public ReqT<M, IN, Unit> Tell(Error error) =>
            ma.As().Tell(error);
    }
}
