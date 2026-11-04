using ForkExt.Traits;

namespace ForkExt;

public static partial class HashSetExtensions
{
    extension<A>(K<HashSet, A> _)
    {
        /// <summary>
        /// Downcast operator
        /// </summary>
        public static HashSet<A> operator +(K<HashSet, A> ma) =>
            (HashSet<A>)ma;
    }
}
