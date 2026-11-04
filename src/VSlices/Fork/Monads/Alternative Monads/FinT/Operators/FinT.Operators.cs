using ForkExt.Traits;

namespace ForkExt;

public static partial class FinTExtensions
{
    extension<M, A>(K<FinT<M>, A> _)
        where M : Monad<M>
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static FinT<M, A> operator +(K<FinT<M>, A> ma) =>
            (FinT<M, A>)ma;
        
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static FinT<M, A> operator >> (K<FinT<M>, A> ma, Lower lower) =>
            +ma;
    }
}
