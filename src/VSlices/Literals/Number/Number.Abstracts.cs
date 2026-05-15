using System.Numerics;

namespace VSlices.Literals.Abstracts;

public class N0<T> : Num<T>
    where T : INumber<T>
{
    public static T Value => T.Zero;
}

public class N1<T> : Num<T>
    where T : INumber<T>
{
    public static T Value => T.One;
}

public class N2<T> : Num<T>
    where T : INumber<T>
{
    public static T Value => T.CreateChecked(2);
}

public class N3<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(3);
}

public class N4<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(4);
}

public class N5<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(5);
}

public class N6<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(6);
}

public class N7<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(7);
}

public class N8<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(8);
}

public class N9<T> : Num<T> where T : INumber<T>
{
    public static T Value => T.CreateChecked(9);
}

public class N<T, Tens, Units> : Num<T>
    where T : INumber<T>
    where Units : Num<T>
    where Tens : Num<T>
{
    public static T Value { get; } =
        Units.Value +
        Tens.Value * (T.CreateChecked(10));
}

public class N<T, Hunds, Tens, Units> : Num<T>
    where T : INumber<T>
    where Hunds : Num<T>
    where Tens : Num<T>
    where Units : Num<T>
{
    public static T Value { get; } =
        Units.Value +
        Tens.Value * (T.CreateChecked(10)) +
        Hunds.Value * (T.CreateChecked(100));
}

public class N<T, Thous, Hunds, Tens, Units> : Num<T>
    where T : INumber<T>
    where Thous : Num<T>
    where Hunds : Num<T>
    where Tens : Num<T>
    where Units : Num<T>
{
    public static T Value { get; } =
        Units.Value +
        Tens.Value * (T.CreateChecked(10)) +
        Hunds.Value * (T.CreateChecked(100)) +
        Thous.Value * (T.CreateChecked(1000));
}
