using LanguageExt;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class ValidateExt
{
    extension<SELF, A>(SELF)
        where SELF : Validate<SELF, A>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static Fin<A> Check(A repr) =>
            SELF.Check(repr);

    }
}
