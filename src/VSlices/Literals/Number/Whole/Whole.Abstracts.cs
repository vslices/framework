using System.Numerics;
using VSlices.Traits;

namespace VSlices.Literals.Abstracts;

/// <summary>
/// Represents a strongly-typed constant value of zero (0), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N0<T> : Const<T>, FloatingSuffixes<N0<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number zero (0) as a value.
    /// </summary>
    public static T Value => T.Zero;
}

/// <summary>
/// Represents a strongly-typed constant value of one (1), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N1<T> : Const<T>, FloatingSuffixes<N1<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number one (1) as a value.
    /// </summary>
    public static T Value => T.One;
}

/// <summary>
/// Represents a strongly-typed constant value of two (2), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N2<T> : Const<T>, FloatingSuffixes<N2<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number two (2) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(2);
}

/// <summary>
/// Represents a strongly-typed constant value of three (3), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N3<T> : Const<T>, FloatingSuffixes<N3<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number three (3) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(3);
}

/// <summary>
/// Represents a strongly-typed constant value of four (4), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N4<T> : Const<T>, FloatingSuffixes<N4<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number four (4) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(4);
}

/// <summary>
/// Represents a strongly-typed constant value of five (5), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N5<T> : Const<T>, FloatingSuffixes<N5<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number five (5) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(5);
}

/// <summary>
/// Represents a strongly-typed constant value of six (6), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N6<T> : Const<T>, FloatingSuffixes<N6<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number six (6) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(6);
}

/// <summary>
/// Represents a strongly-typed constant value of seven (7), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N7<T> : Const<T>, FloatingSuffixes<N7<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number seven (7) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(7);
}

/// <summary>
/// Represents a strongly-typed constant value of eight (8), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N8<T> : Const<T>, FloatingSuffixes<N8<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number eight (8) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(8);
}

/// <summary>
/// Represents a strongly-typed constant value of nine (9), backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">
/// The resulting numeric type.
/// </typeparam>
public class N9<T> : Const<T>, FloatingSuffixes<N9<T>, T>
    where T : INumber<T>
{
    /// <summary>
    /// Represents the number nine (9) as a value.
    /// </summary>
    public static T Value => T.CreateChecked(9);
}

/// <summary>
/// Represents a numeric literal composed of three components: tens and units, backed by a<typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">The numeric type of the literal.</typeparam>
/// <typeparam name="Tens">The type representing the tens place of the numeric literal.</typeparam>
/// <typeparam name="Units">The type representing the units place of the numeric literal.</typeparam>
public class N<T, Tens, Units> : N<T, N0<T>, Tens, Units>
    where T : INumber<T>
    where Units : Const<T>
    where Tens : Const<T>;

/// <summary>
/// Represents a numeric literal composed of three components: tens, units and hundreds, backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">The numeric type of the literal.</typeparam>
/// <typeparam name="Tens">The type representing the tens place of the numeric literal.</typeparam>
/// <typeparam name="Units">The type representing the units place of the numeric literal.</typeparam>
/// <typeparam name="Hundreds">The type representing the hundreds place of the numeric literal.</typeparam>
public class N<T, Hundreds, Tens, Units> : N<T, N0<T>, Hundreds, Tens, Units>
    where T : INumber<T>
    where Hundreds : Const<T>
    where Tens : Const<T>
    where Units : Const<T>;

/// <summary>
/// Represents a numeric literal composed of three components: tens, units, hundreds and thousands,
/// backed by a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">The numeric type of the literal.</typeparam>
/// <typeparam name="Tens">The type representing the tens place of the numeric literal.</typeparam>
/// <typeparam name="Units">The type representing the units place of the numeric literal.</typeparam>
/// <typeparam name="Hundreds">The type representing the hundreds place of the numeric literal.</typeparam>
/// <typeparam name="Thousands">The type representing the thousands place of the numeric literal.</typeparam>
public class N<T, Thousands, Hundreds, Tens, Units> 
    : Const<T>, FloatingSuffixes<N<T, Thousands, Hundreds, Tens, Units>, T>
    where T : INumber<T>
    where Thousands : Const<T>
    where Hundreds : Const<T>
    where Tens : Const<T>
    where Units : Const<T>
{
    /// <summary>
    /// Represents the numeric literal as a value, calculated by combining the values of the components.
    /// </summary>
    public static T Value { get; } =
        Units.Value +
        Tens.Value * T.CreateChecked(10) +
        Hundreds.Value * T.CreateChecked(100) +
        Thousands.Value * T.CreateChecked(1000);
}
