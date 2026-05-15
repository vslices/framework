using VSlices.Literals;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    public class D<Tenths, Hundredths> : D<Tenths, Hundredths, N0>
        where Tenths : WholeConst<int>
        where Hundredths : WholeConst<int>;

    public class F<Tenths, Hundredths> : F<Tenths, Hundredths, N0>
        where Tenths : WholeConst<int>
        where Hundredths : WholeConst<int>;

    public class M<Tenths, Hundredths> : M<Tenths, Hundredths, N0>
        where Tenths : WholeConst<int>
        where Hundredths : WholeConst<int>;

    public sealed class D01 : D<N0, N1>;
    public sealed class D02 : D<N0, N2>;
    public sealed class D03 : D<N0, N3>;
    public sealed class D04 : D<N0, N4>;
    public sealed class D05 : D<N0, N5>;
    public sealed class D06 : D<N0, N6>;
    public sealed class D07 : D<N0, N7>;
    public sealed class D08 : D<N0, N8>;
    public sealed class D09 : D<N0, N9>;

    public sealed class F01 : F<N0, N1>;
    public sealed class F02 : F<N0, N2>;
    public sealed class F03 : F<N0, N3>;
    public sealed class F04 : F<N0, N4>;
    public sealed class F05 : F<N0, N5>;
    public sealed class F06 : F<N0, N6>;
    public sealed class F07 : F<N0, N7>;
    public sealed class F08 : F<N0, N8>;
    public sealed class F09 : F<N0, N9>;

    public sealed class M01 : M<N0, N1>;
    public sealed class M02 : M<N0, N2>;
    public sealed class M03 : M<N0, N3>;
    public sealed class M04 : M<N0, N4>;
    public sealed class M05 : M<N0, N5>;
    public sealed class M06 : M<N0, N6>;
    public sealed class M07 : M<N0, N7>;
    public sealed class M08 : M<N0, N8>;
    public sealed class M09 : M<N0, N9>;
}
