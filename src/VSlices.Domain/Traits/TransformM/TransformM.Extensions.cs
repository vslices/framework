using LanguageExt;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class TransformMExt
{
    extension<SELF, M, A>(SELF)
        where SELF : TransformM<SELF, M, A>
        where M : Monad<M>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static FinT<M, SELF> CreateM(A repr) =>
            SELF.Create(repr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static K<M, SELF> NewM(A repr) =>
            SELF.New(repr);

        /// <summary>
        ///
        /// </summary>
        /// <param name="repr"></param>
        /// <returns></returns>
        public static K<M, Seq<SELF>> NewM(Seq<A> repr) =>
            SELF.New(repr);
    }
}
