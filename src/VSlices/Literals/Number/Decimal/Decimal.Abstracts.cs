using System.Numerics;
using VSlices.Traits;

namespace VSlices.Literals.Abstracts;

public class P<WholeType, Whole, Tenths, Type> : P<WholeType, Whole, Tenths, N0, Type>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where Type : IFloatingPoint<Type>;

public class P<WholeType, Whole, Tenths, Hundredths, Type> : P<WholeType, Whole, Tenths, Hundredths, N0, Type>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where Hundredths : Const<int>
    where Type : IFloatingPoint<Type>;

public class P<WholeType, Whole, Tenths, Hundredths, Thousandths, Type> : Const<Type>
    where WholeType : INumber<WholeType>
    where Whole : Const<WholeType>
    where Tenths : Const<int>
    where Hundredths : Const<int>
    where Thousandths : Const<int>
    where Type : IFloatingPoint<Type>
{
    public static Type Value { get; } =
        Type.CreateChecked(Whole.Value) +
        Type.CreateChecked(Tenths.Value) / Type.CreateChecked(N10.Value) +
        Type.CreateChecked(Hundredths.Value) / Type.CreateChecked(N100.Value) +
        Type.CreateChecked(Thousandths.Value) / Type.CreateChecked(N1000.Value);
}