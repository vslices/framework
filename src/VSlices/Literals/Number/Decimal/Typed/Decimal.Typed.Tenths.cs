using VSlices.Literals;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    public class D<Tenths> : D<Tenths, N0>
        where Tenths : Const<int>;

    public class F<Tenths> : F<Tenths, N0>
        where Tenths : Const<int>;

    public class M<Tenths> : M<Tenths, N0>
        where Tenths : Const<int>;

    public sealed class D1 : D<N1>;
    public sealed class D2 : D<N2>;
    public sealed class D3 : D<N3>;
    public sealed class D4 : D<N4>;
    public sealed class D5 : D<N5>;
    public sealed class D6 : D<N6>;
    public sealed class D7 : D<N7>;
    public sealed class D8 : D<N8>;
    public sealed class D9 : D<N9>;

    public sealed class F1 : F<N1>;
    public sealed class F2 : F<N2>;
    public sealed class F3 : F<N3>;
    public sealed class F4 : F<N4>;
    public sealed class F5 : F<N5>;
    public sealed class F6 : F<N6>;
    public sealed class F7 : F<N7>;
    public sealed class F8 : F<N8>;
    public sealed class F9 : F<N9>;

    public sealed class M1 : M<N1>;
    public sealed class M2 : M<N2>;
    public sealed class M3 : M<N3>;
    public sealed class M4 : M<N4>;
    public sealed class M5 : M<N5>;
    public sealed class M6 : M<N6>;
    public sealed class M7 : M<N7>;
    public sealed class M8 : M<N8>;
    public sealed class M9 : M<N9>;
}
