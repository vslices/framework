using System.Numerics;

namespace VSlices.Literals;

public interface Num<out TYPE> : Const<TYPE>
    where TYPE : INumber<TYPE>;

