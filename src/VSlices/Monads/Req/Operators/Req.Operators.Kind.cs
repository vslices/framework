using VSlices.Monads;

namespace VSlices;

public static partial class ReqKindOperatorExtension
{
    extension<IN, A>(K<Req<IN>, A>)
    {
        public static Req<IN, A> operator + (
            K<Req<IN>, A> ma) =>
            ma.As();
    }

}
