using System;
using VSlices.Domain.Traits;

namespace VSlices;

/// <summary>
///
/// </summary>
public static class MaintainerExtensions
{
    extension<A>(A)
        where A : Maintained<A>
    {
        /// <summary>
        ///
        /// </summary>
        public static Option<A> FindM(Func<A, bool> fa) =>
            findM(fa);

        /// <summary>
        ///
        /// </summary>
        public static A Find(Func<A, bool> fa) =>
            find(fa);
    }

    extension<A>(A a)
        where A : Maintained<A>
    {
        /// <summary>
        ///
        /// </summary>
        public bool Is(A b) => a.Equals(b);

        /// <summary>
        ///
        /// </summary>
        public bool IsNot(A b) => !a.Equals(b);
    }
}
