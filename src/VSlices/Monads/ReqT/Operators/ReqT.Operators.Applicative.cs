using VSlices.Monads;

namespace VSlices;

public static partial class ReqApplicativeOperatorExtensions
{
    extension<IN, M, A>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {

    }
}
