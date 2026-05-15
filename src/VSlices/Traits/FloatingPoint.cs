using System.Numerics;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
    where WSelf : WholeConst<WType>
    where WType : INumber<WType>;
