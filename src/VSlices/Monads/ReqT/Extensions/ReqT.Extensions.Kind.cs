using VSlices.Monads;

namespace VSlices;

public static class ReqTKindExtensions
{
    extension<M, IN, A>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {
        public ReqT<M, IN, A> As() =>
            (ReqT<M, IN, A>)ma;

        public FinT<M, A> Run(IN input) =>
            ma.As().Run(input);
    }

}
