using System.Numerics;
using VSlices.Traits;

namespace VSlices.Literals.Abstracts;

public class N0<T> : Const<T>, FloatingSuffixes<N0<T>, T>
    where T : INumber<T>
{
    public static T Value => T.Zero;
}

public class N1<T> : Const<T>, FloatingSuffixes<N1<T>, T>
    where T : INumber<T>
{
    public static T Value => T.One;
}

public class N2<T> : Const<T>, FloatingSuffixes<N2<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(2);
}

public class N3<T> : Const<T>, FloatingSuffixes<N3<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(3);
}

public class N4<T> : Const<T>, FloatingSuffixes<N4<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(4);
}

public class N5<T> : Const<T>, FloatingSuffixes<N5<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(5);
}

public class N6<T> : Const<T>, FloatingSuffixes<N6<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(6);
}

public class N7<T> : Const<T>, FloatingSuffixes<N7<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(7);
}

public class N8<T> : Const<T>, FloatingSuffixes<N8<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(8);
}

public class N9<T> : Const<T>, FloatingSuffixes<N9<T>, T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(9);
}

public class N<T, Tens, Units> : N<T, N0<T>, Tens, Units>
    where T : INumber<T>
    where Units : Const<T>
    where Tens : Const<T>;

public class N<T, Hundreds, Tens, Units> : N<T, N0<T>, Hundreds, Tens, Units>
    where T : INumber<T>
    where Hundreds : Const<T>
    where Tens : Const<T>
    where Units : Const<T>;

public class N<T, Thousands, Hundreds, Tens, Units> 
    : Const<T>, FloatingSuffixes<N<T, Thousands, Hundreds, Tens, Units>, T>
    where T : INumber<T>
    where Thousands : Const<T>
    where Hundreds : Const<T>
    where Tens : Const<T>
    where Units : Const<T>
{
    public static T Value { get; } =
        Units.Value +
        Tens.Value * T.CreateChecked(10) +
        Hundreds.Value * T.CreateChecked(100) +
        Thousands.Value * T.CreateChecked(1000);
}
