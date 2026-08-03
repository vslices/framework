using VSlices.Monads;

namespace VSlices;

public static partial class ReqMonadExtensions
{
    extension<M, IN, A, B>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        public ReqT<M, IN, B> Bind(Func<A, K<Req<IN>, B>> fb) =>
            ma.As().Bind(fb);

        public ReqT<M, IN, B> Bind(Func<A, Req<IN, B>> fb) =>
            ma.As().Bind(fb);
    }
}
