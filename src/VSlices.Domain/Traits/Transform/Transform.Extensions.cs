namespace VSlices;

/// <summary>
///
/// </summary>
public static class TransformExt
{
    extension<SELF, A>(SELF)
        where SELF : Transform<SELF, A>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static Fin<SELF> Create(A repr) =>
            SELF.Invariants.Onto(repr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static SELF New(A repr) =>
            SELF.Create(repr).ThrowIfFail();

    }
}
