using System;
using System.Collections.Generic;
using System.Numerics;

namespace VSlices.Domain.Literals;

public interface NumConst<out T> : Const<T>
    where T : INumber<T>;
