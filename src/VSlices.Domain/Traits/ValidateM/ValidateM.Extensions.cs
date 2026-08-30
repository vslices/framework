using LanguageExt;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class ValidateMExt
{
    extension<SELF, M, A>(SELF)
        where SELF : ValidateM<SELF, M, A>
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static FinT<M, A> CheckM(A repr) =>
            SELF.CheckM(repr);

    }
}
