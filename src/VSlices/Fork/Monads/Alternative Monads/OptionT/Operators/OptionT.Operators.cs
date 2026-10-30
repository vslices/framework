using ForkExt.Traits;

namespace ForkExt;

public static partial class OptionTExtensions
{
    extension<M, A>(K<OptionT<M>, A> _)
        where M : Monad<M>
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static OptionT<M, A> operator +(K<OptionT<M>, A> ma) =>
            (OptionT<M, A>)ma;
        
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static OptionT<M, A> operator >> (K<OptionT<M>, A> ma, Lower lower) =>
            +ma;
    }
}
