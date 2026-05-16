using VSlices.Literals;
using VSlices.Literals.Abstracts;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
{
    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to thousandths,
    /// backed by a <see cref="double"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place of the number.</typeparam>
    /// <typeparam name="Thousandths">The type representing the thousandths place of the number.</typeparam>
    public class D<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, double>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    /// <summary>
    /// Represents a generic class that defines a floating-point number with precision up to thousandths,
    /// backed by a <see cref="float"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place of the number.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place of the number.</typeparam>
    /// <typeparam name="Thousandths">The type representing the thousandths place of the number.</typeparam>
    public class F<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, float>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    /// <summary>
    /// Represents a class that defines a floating-point suffix with a precision up to the thousandths, 
    /// backed by a <see cref="decimal"/> value and using <typeparamref name="WSelf"/> as the whole value part.
    /// </summary>
    /// <typeparam name="Tenths">The type representing the tenths place value.</typeparam>
    /// <typeparam name="Hundredths">The type representing the hundredths place value.</typeparam>
    /// <typeparam name="Thousandths">The type representing the thousandths place value.</typeparam>
    public class M<Tenths, Hundredths, Thousandths> : P<WType, WSelf, Tenths, Hundredths, Thousandths, decimal>
        where Tenths : Const<int>
        where Hundredths : Const<int>
        where Thousandths : Const<int>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.001, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D001 : D<N0, N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.002, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D002 : D<N0, N0, N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.003, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D003 : D<N0, N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.004, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D004 : D<N0, N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.005, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D005 : D<N0, N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.006, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D006 : D<N0, N0, N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.007, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D007 : D<N0, N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.008, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D008 : D<N0, N0, N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.009, backed by a <see cref="double"/> value.
    /// </summary>
    public sealed class D009 : D<N0, N0, N9>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.001, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F001 : F<N0, N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.002, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F002 : F<N0, N0, N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.003, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F003 : F<N0, N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.004, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F004 : F<N0, N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.005, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F005 : F<N0, N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.006, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F006 : F<N0, N0, N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.007, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F007 : F<N0, N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.008, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F008 : F<N0, N0, N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.009, backed by a <see cref="float"/> value.
    /// </summary>
    public sealed class F009 : F<N0, N0, N9>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.001, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M001 : M<N0, N0, N1>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.002, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M002 : M<N0, N0, N2>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.003, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M003 : M<N0, N0, N3>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.004, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M004 : M<N0, N0, N4>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.005, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M005 : M<N0, N0, N5>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.006, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M006 : M<N0, N0, N6>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.007, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M007 : M<N0, N0, N7>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.008, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M008 : M<N0, N0, N8>;

    /// <summary>
    /// Represents <typeparamref name="WSelf"/>.009, backed by a <see cref="decimal"/> value.
    /// </summary>
    public sealed class M009 : M<N0, N0, N9>;
}