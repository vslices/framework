using System.Numerics;

namespace VSlices.Literals;

public class N0<TYPE> : Num<TYPE>
    where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.Zero;
}

public class N1<TYPE> : Num<TYPE>
    where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.One;
}

public class N2<TYPE> : Num<TYPE>
    where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(2);
}

public class N3<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(3);
}

public class N4<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(4);
}

public class N5<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(5);
}

public class N6<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(6);
}

public class N7<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(7);
}

public class N8<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(8);
}

public class N9<TYPE> : Num<TYPE> where TYPE : INumber<TYPE>
{
    public static TYPE Value => TYPE.CreateChecked(9);
}

public class N<TYPE, UNITS, TENS> : Num<TYPE>
    where TYPE : INumber<TYPE>
    where UNITS : Num<TYPE>
    where TENS : Num<TYPE>
{
    public static TYPE Value { get; } =
        UNITS.Value +
        TENS.Value * (TYPE.CreateChecked(10));
}

public class N<TYPE, UNITS, TENS, CENTS> : Num<TYPE>
    where TYPE : INumber<TYPE>
    where UNITS : Num<TYPE>
    where TENS : Num<TYPE>
    where CENTS : Num<TYPE>
{
    public static TYPE Value { get; } =
        UNITS.Value +
        TENS.Value * (TYPE.CreateChecked(10)) +
        CENTS.Value * (TYPE.CreateChecked(100));
}
