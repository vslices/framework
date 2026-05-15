using VSlices.Literals;
using VSlices.Literals.Abstracts;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    public class D<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, double>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    public class F<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, float>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    public class M<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, decimal>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    public sealed class D001 : D<N0, N0, N1>;
    public sealed class D002 : D<N0, N0, N2>;
    public sealed class D003 : D<N0, N0, N3>;
    public sealed class D004 : D<N0, N0, N4>;
    public sealed class D005 : D<N0, N0, N5>;
    public sealed class D006 : D<N0, N0, N6>;
    public sealed class D007 : D<N0, N0, N7>;
    public sealed class D008 : D<N0, N0, N8>;
    public sealed class D009 : D<N0, N0, N9>;

    public sealed class F001 : F<N0, N0, N1>;
    public sealed class F002 : F<N0, N0, N2>;
    public sealed class F003 : F<N0, N0, N3>;
    public sealed class F004 : F<N0, N0, N4>;
    public sealed class F005 : F<N0, N0, N5>;
    public sealed class F006 : F<N0, N0, N6>;
    public sealed class F007 : F<N0, N0, N7>;
    public sealed class F008 : F<N0, N0, N8>;
    public sealed class F009 : F<N0, N0, N9>;

    public sealed class M001 : M<N0, N0, N1>;
    public sealed class M002 : M<N0, N0, N2>;
    public sealed class M003 : M<N0, N0, N3>;
    public sealed class M004 : M<N0, N0, N4>;
    public sealed class M005 : M<N0, N0, N5>;
    public sealed class M006 : M<N0, N0, N6>;
    public sealed class M007 : M<N0, N0, N7>;
    public sealed class M008 : M<N0, N0, N8>;
    public sealed class M009 : M<N0, N0, N9>;
}