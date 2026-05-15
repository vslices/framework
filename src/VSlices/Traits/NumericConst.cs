using System.Numerics;

namespace VSlices.Traits;

public interface WholeConst<out T> : Const<T>
    where T : INumber<T>;

public interface DecimalConst<T> : Const<T>
    where T : IFloatingPoint<T>;
