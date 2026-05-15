using System.Numerics;

namespace VSlices.Traits;

public partial interface FloatingSuffixes<WSelf, WType>
    where WSelf : Const<WType>
    where WType : INumber<WType>;
