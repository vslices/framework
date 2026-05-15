using System.Numerics;
using VSlices.Traits;

namespace VSlices.Literals.Abstracts;

public class N0<T> : WholeConst<T>, FloatingSuffixes<N0<T>, T>
    where T : INumber<T>
{
    public static T Value => T.Zero;
}

public class N1<T> : WholeConst<T>, FloatingSuffixes<N1<T>, T>
    where T : INumber<T>
{
    public static T Value => T.One;
}

public class N2<T> : WholeConst<T>, FloatingSuffixes<N2<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(2);
}

public class N3<T> : WholeConst<T>, FloatingSuffixes<N3<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(3);
}

public class N4<T> : WholeConst<T>, FloatingSuffixes<N4<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(4);
}

public class N5<T> : WholeConst<T>, FloatingSuffixes<N5<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(5);
}

public class N6<T> : WholeConst<T>, FloatingSuffixes<N6<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(6);
}

public class N7<T> : WholeConst<T>, FloatingSuffixes<N7<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(7);
}

public class N8<T> : WholeConst<T>, FloatingSuffixes<N8<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(8);
}

public class N9<T> : WholeConst<T>, FloatingSuffixes<N9<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(9);
}

public class N<T, Tens, Units> : N<T, N0<T>, Tens, Units>
    where T : INumber<T>
    where Units : WholeConst<T>
    where Tens : WholeConst<T>;

public class N<T, Hundreds, Tens, Units> : N<T, N0<T>, Hundreds, Tens, Units>
    where T : INumber<T>
    where Hundreds : WholeConst<T>
    where Tens : WholeConst<T>
    where Units : WholeConst<T>;

public class N<T, Thousands, Hundreds, Tens, Units> 
    : WholeConst<T>, FloatingSuffixes<N<T, Thousands, Hundreds, Tens, Units>, T>
    where T : INumber<T>
    where Thousands : WholeConst<T>
    where Hundreds : WholeConst<T>
    where Tens : WholeConst<T>
    where Units : WholeConst<T>
{
    public static T Value { get; } =
        Units.Value +
        Tens.Value * T.CreateChecked(10) +
        Hundreds.Value * T.CreateChecked(100) +
        Thousands.Value * T.CreateChecked(1000);
}
