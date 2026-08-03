using VSlices.Monads;

namespace VSlices;

public static partial class ReqFunctorOperatorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A>)
    {
        public static Req<IN, B> operator * (
            K<Req<IN>, A> ma,
            Func<A, B> f) =>
            +ma.Map(f);
    }
}
