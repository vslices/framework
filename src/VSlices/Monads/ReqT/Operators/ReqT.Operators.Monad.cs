using VSlices.Monads;

namespace VSlices;

public static partial class ReqMonadOperatorExtensions
{
    extension<IN, M, A, B>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {
        public static ReqT<M, IN, B> operator >> (
            K<ReqT<M, IN>, A> ma,
            Func<A, K<ReqT<M, IN>, B>> f) =>
            +ma.Bind(f);
    }
}
