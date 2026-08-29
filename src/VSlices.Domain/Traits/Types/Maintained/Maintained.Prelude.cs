
using LanguageExt;

namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    /// <summary>
    ///
    /// </summary>
    public static Seq<A> get<A>()
        where A : Maintained<A> =>
        A.All;

    /// <summary>
    ///
    /// </summary>
    public static Option<A> find<A>(Func<A, bool> fa)
        where A : Maintained<A> =>
        A.Find(fa);

    /// <summary>
    ///
    /// </summary>
    public static A first<A>(Func<A, bool> fa)
        where A : Maintained<A> =>
        A.First(fa);

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="A"></typeparam>
    /// <param name="fa"></param>
    /// <returns></returns>
    public static bool exists<A>(Func<A, bool> fa)
        where A : Maintained<A> =>
        A.Exists(fa);
}
