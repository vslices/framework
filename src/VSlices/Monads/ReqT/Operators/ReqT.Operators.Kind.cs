using VSlices.Monads;

namespace VSlices;

public static partial class ReqKindOperatorExtension
{
    extension<IN, M, A>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {
        public static ReqT<M, IN, A> operator + (
            K<ReqT<M, IN>, A> ma) =>
            ma.As();
    }

}
