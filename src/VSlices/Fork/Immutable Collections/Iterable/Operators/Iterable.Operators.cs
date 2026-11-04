using ForkExt.Traits;

namespace ForkExt;

public static partial class IterableExtensions
{
    extension<A>(K<Iterable, A> _)
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static Iterable<A> operator +(K<Iterable, A> ma) =>
            (Iterable<A>)ma;
        
        public static Iterable<A> operator >> (K<Iterable, A> ma, Lower lower) =>
            (Iterable<A>)ma;
    }
}
