using VSlices.Monads;

namespace VSlices;

public static partial class ReqFunctorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A> ma)
    {
        public Req<IN, B> Map(Func<A, B> fb) =>
            ma.As().Map(fb);
    }
}
