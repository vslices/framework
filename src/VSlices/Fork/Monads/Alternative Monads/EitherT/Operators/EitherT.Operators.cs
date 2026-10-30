using ForkExt.Traits;

namespace ForkExt;

public static partial class EitherTExtensions
{
    extension<L, M, A>(K<EitherT<L, M>, A> _)
        where M : Monad<M>
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static EitherT<L, M, A> operator +(K<EitherT<L, M>, A> ma) =>
            (EitherT<L, M, A>)ma;
        
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static EitherT<L, M, A> operator >> (K<EitherT<L, M>, A> ma, Lower lower) =>
            +ma;
    }
}
