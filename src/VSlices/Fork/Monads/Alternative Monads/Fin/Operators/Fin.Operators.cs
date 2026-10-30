using ForkExt.Traits;

namespace ForkExt;

public static partial class FinExtensions
{
    extension<A>(K<Fin, A> _)
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static Fin<A> operator +(K<Fin, A> ma) =>
            (Fin<A>)ma;
        
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static Fin<A> operator >> (K<Fin, A> ma, Lower lower) =>
            +ma;
    }
}
