using VSlices.Monads;

namespace VSlices;

public static partial class ReqMonadExtensions
{
    extension<IN, A, B>(K<Req<IN>, A> ma)
    {
        public Req<IN, B> Bind(Func<A, K<Req<IN>, B>> fb) =>
            ma.As().Bind(fb);

        public Req<IN, B> Bind(Func<A, Req<IN, B>> fb) =>
            ma.As().Bind(fb);
    }
}
