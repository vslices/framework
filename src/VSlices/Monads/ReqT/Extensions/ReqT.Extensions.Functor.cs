using VSlices.Monads;

namespace VSlices;

public static partial class ReqTFunctorExtensions
{
    extension<M, IN, A, B>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        public ReqT<M, IN, B> Map(Func<A, B> fb) =>
            ma.As().Map(fb);
    }
}
