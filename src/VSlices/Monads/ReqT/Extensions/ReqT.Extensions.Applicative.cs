using VSlices.Monads;

namespace VSlices;
/// <summary>
///
/// </summary>
public static partial class ReqTApplicativeExtensions
{
    extension<M, IN, A, B>(K<ReqT<M, IN>, A> ma)
        where M : Monad<M>
    {

    }
}
