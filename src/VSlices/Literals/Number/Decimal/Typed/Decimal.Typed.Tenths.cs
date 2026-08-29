using VSlices.Literals;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to tenths,
    /// backed by a <see cref="double"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    public class D<Tenths> : D<Tenths, N0>
        where Tenths : Const<int>;

    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to tenths,
    /// backed by a <see cref="float"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    public class F<Tenths> : F<Tenths, N0>
        where Tenths : Const<int>;

    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to tenths,
    /// backed by a <see cref="decimal"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    public class M<Tenths> : M<Tenths, N0>
        where Tenths : Const<int>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.1, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D1 : D<N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.2, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D2 : D<N2>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.3, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D3 : D<N3>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.4, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D4 : D<N4>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.5, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D5 : D<N5>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.6, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D6 : D<N6>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.7, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D7 : D<N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.8, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D8 : D<N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.9, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D9 : D<N9>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.1, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F1 : F<N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.2, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F2 : F<N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.3, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F3 : F<N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.4, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F4 : F<N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.5, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F5 : F<N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.6, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F6 : F<N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.7, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F7 : F<N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.8, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F8 : F<N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.9, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F9 : F<N9>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.1, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M1 : M<N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.2, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M2 : M<N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.3, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M3 : M<N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.4, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M4 : M<N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.5, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M5 : M<N5>;
    
    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.005, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M6 : M<N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.007, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M7 : M<N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.008, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M8 : M<N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.009, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M9 : M<N9>;
}
