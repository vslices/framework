namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    public static Fin<T> safe<T>(string repr)
        where T : DomainType<T, string> =>
        T.From(repr);

    public static T @unsafe<T>(string repr)
        where T : DomainType<T, string> =>
        T.FromUnsafe(repr);

    public static T @unsafe<T>(long repr)
        where T : DomainType<T, long> =>
        T.FromUnsafe(repr);

}
