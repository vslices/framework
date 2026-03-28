namespace VSlices;

public static partial class VSlicesDomainPrelude
{
    public static Fin<T> safe<T>(bool repr)
        where T : DomainType<T, bool> =>
        T.From(repr);

    public static Fin<T> safe<T>(char repr)
        where T : DomainType<T, char> =>
        T.From(repr);

    public static Fin<T> safe<T>(string repr)
        where T : DomainType<T, string> =>
        T.From(repr);

    public static Fin<T> safe<T>(byte repr)
        where T : DomainType<T, byte> =>
        T.From(repr);

    public static Fin<T> safe<T>(short repr)
        where T : DomainType<T, short> =>
        T.From(repr);

    public static Fin<T> safe<T>(ushort repr)
        where T : DomainType<T, ushort> =>
        T.From(repr);

    public static Fin<T> safe<T>(int repr)
        where T : DomainType<T, int> =>
        T.From(repr);

    public static Fin<T> safe<T>(uint repr)
        where T : DomainType<T, uint> =>
        T.From(repr);

    public static Fin<T> safe<T>(long repr)
        where T : DomainType<T, long> =>
        T.From(repr);

    public static Fin<T> safe<T>(ulong repr)
        where T : DomainType<T, ulong> =>
        T.From(repr);

    public static Fin<T> safe<T>(float repr)
        where T : DomainType<T, float> =>
        T.From(repr);

    public static Fin<T> safe<T>(double repr)
        where T : DomainType<T, double> =>
        T.From(repr);

    public static Fin<T> safe<T>(decimal repr)
        where T : DomainType<T, decimal> =>
        T.From(repr);

    public static Fin<T> safe<T>(DateOnly repr)
        where T : DomainType<T, DateOnly> =>
        T.From(repr);

    public static Fin<T> safe<T>(TimeOnly repr)
        where T : DomainType<T, TimeOnly> =>
        T.From(repr);

    public static Fin<T> safe<T>(DateTime repr)
        where T : DomainType<T, DateTime> =>
        T.From(repr);

    public static Fin<T> safe<T>(DateTimeOffset repr)
        where T : DomainType<T, DateTimeOffset> =>
        T.From(repr);
}
