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
            SELF.Create(repr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static SELF New(A repr) =>
            SELF.New(repr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static Seq<SELF> New(Seq<A> repr) =>
            SELF.New(repr);
    }
}
