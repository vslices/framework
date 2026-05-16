using VSlices.Literals;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to hundredths,
    /// backed by a <see cref="double"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place of the number.</typeparam>
    public class D<Tenths, Hundredths> : D<Tenths, Hundredths, N0>
        where Tenths : Const<int>
        where Hundredths : Const<int>;

    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to hundredths,
    /// backed by a <see cref="float"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place of the number.</typeparam>
    public class F<Tenths, Hundredths> : F<Tenths, Hundredths, N0>
        where Tenths : Const<int>
        where Hundredths : Const<int>;

    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to hundredths,
    /// backed by a <see cref="decimal"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place of the number.</typeparam>
    public class M<Tenths, Hundredths> : M<Tenths, Hundredths, N0>
        where Tenths : Const<int>
        where Hundredths : Const<int>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.01, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D01 : D<N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.02, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D02 : D<N0, N2>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.03, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D03 : D<N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.04, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D04 : D<N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.05, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D05 : D<N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.06, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D06 : D<N0, N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.07, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D07 : D<N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.08, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D08 : D<N0, N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.09, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D09 : D<N0, N9>;


    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.001, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F01 : F<N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.002, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F02 : F<N0, N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.003, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F03 : F<N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.004, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F04 : F<N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.005, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F05 : F<N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.006, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F06 : F<N0, N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.007, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F07 : F<N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.008, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F08 : F<N0, N8>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.009, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F09 : F<N0, N9>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.01, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M01 : M<N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.02, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M02 : M<N0, N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.03, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M03 : M<N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.04, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M04 : M<N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.05, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M05 : M<N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.06, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M06 : M<N0, N6>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.07, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M07 : M<N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.08, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M08 : M<N0, N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.09, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M09 : M<N0, N9>;
}
