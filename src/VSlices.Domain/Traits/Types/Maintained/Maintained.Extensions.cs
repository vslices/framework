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
        public static Option<A> Find(Func<A, bool> fa) =>
            find(fa);

        /// <summary>
        ///
        /// </summary>
        public static A First(Func<A, bool> fa) =>
            first(fa);

        /// <summary>
        ///
        /// </summary>
        /// <param name="fa"></param>
        /// <returns></returns>
        public static bool Exists(Func<A, bool> fa) =>
            find(fa).IsSome;
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
