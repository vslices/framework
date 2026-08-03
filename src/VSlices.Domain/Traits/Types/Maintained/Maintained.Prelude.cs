
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
    public static Option<A> findM<A>(Func<A, bool> fa)
        where A : Maintained<A> =>
        get<A>().Find(fa);

    /// <summary>
    ///
    /// </summary>
    public static A find<A>(Func<A, bool> fa)
        where A : Maintained<A> =>
        findM(fa).Case switch
        {
            A a => a,
            _ => throw new InvalidOperationException("Option was None")
        };
}
