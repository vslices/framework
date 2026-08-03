using VSlices.Monads;

namespace VSlices;

public static partial class ReqMonadOperatorExtensions
{
    extension<IN, A, B>(K<Req<IN>, A>)
    {
        public static Req<IN, B> operator >> (
            K<Req<IN>, A> ma,
            Func<A, K<Req<IN>, B>> f) =>
            +ma.Bind(f);
    }
}
