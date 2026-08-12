using VSlices.Monads;

namespace VSlices;

/// <summary>
///
/// </summary>
public static partial class ReqApplicativeOperatorExtensions
{
    extension<IN, M, A>(K<ReqT<M, IN>, A>)
        where M : Monad<M>
    {

    }
}
