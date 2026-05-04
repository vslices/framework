using System;
using System.Collections.Generic;
using System.Text;

namespace VSlices.Domain.Literals;

public interface NumericConst : Const<long>, Const<int>, Const<decimal>;

public sealed class N0 : NumericConst
{
    static long Const<long>.Value => 0L;

    static int Const<int>.Value => 0;

    static decimal Const<decimal>.Value => 0m;
}

public sealed class N1 : NumericConst
{
    static long Const<long>.Value => 1L;

    static int Const<int>.Value => 1;

    static decimal Const<decimal>.Value => 1m;
}

public sealed class N2 : NumericConst
{
    static long Const<long>.Value => 2L;

    static int Const<int>.Value => 2;

    static decimal Const<decimal>.Value => 2m;
}

public sealed class N4 : NumericConst
{
    static long Const<long>.Value => 4L;

    static int Const<int>.Value => 4;

    static decimal Const<decimal>.Value => 4m;
}

public sealed class N8 : NumericConst
{
    static long Const<long>.Value => 8L;

    static int Const<int>.Value => 8;

    static decimal Const<decimal>.Value => 8m;
}

public sealed class N10 : NumericConst
{
    static long Const<long>.Value => 10L;

    static int Const<int>.Value => 10;

    static decimal Const<decimal>.Value => 10m;
}

public sealed class N16 : NumericConst
{
    static long Const<long>.Value => 16L;

    static int Const<int>.Value => 16;

    static decimal Const<decimal>.Value => 16m;
}

public sealed class N32 : NumericConst
{
    static long Const<long>.Value => 32L;

    static int Const<int>.Value => 32;

    static decimal Const<decimal>.Value => 32m;
}

public sealed class N64 : NumericConst
{
    static long Const<long>.Value => 64L;

    static int Const<int>.Value => 64;

    static decimal Const<decimal>.Value => 64m;
}

public sealed class N100 : NumericConst
{
    static long Const<long>.Value => 100L;

    static int Const<int>.Value => 100;

    static decimal Const<decimal>.Value => 100m;
}

public sealed class N128 : NumericConst
{
    static long Const<long>.Value => 128L;

    static int Const<int>.Value => 128;

    static decimal Const<decimal>.Value => 128m;
}

public sealed class N256 : NumericConst
{
    static long Const<long>.Value => 256L;

    static int Const<int>.Value => 256;

    static decimal Const<decimal>.Value => 256m;
}

public sealed class N512 : NumericConst
{
    static long Const<long>.Value => 512L;

    static int Const<int>.Value => 512;

    static decimal Const<decimal>.Value => 512m;
}

public sealed class N1000 : NumericConst
{
    static long Const<long>.Value => 1000L;

    static int Const<int>.Value => 1000;

    static decimal Const<decimal>.Value => 1000m;
}

public sealed class N1024 : NumericConst
{
    static long Const<long>.Value => 1024L;

    static int Const<int>.Value => 1024;

    static decimal Const<decimal>.Value => 1024m;
}

public sealed class N2048 : NumericConst
{
    static long Const<long>.Value => 2048L;

    static int Const<int>.Value => 2048;

    static decimal Const<decimal>.Value => 2048m;
}

public sealed class N4096 : NumericConst
{
    static long Const<long>.Value => 4096L;

    static int Const<int>.Value => 4096;

    static decimal Const<decimal>.Value => 4096m;
}

public sealed class N8192 : NumericConst
{
    static long Const<long>.Value => 8192L;

    static int Const<int>.Value => 8192;

    static decimal Const<decimal>.Value => 8192m;
}

public sealed class N10000 : NumericConst
{
    static long Const<long>.Value => 10000L;

    static int Const<int>.Value => 10000;

    static decimal Const<decimal>.Value => 10000m;
}

public sealed class N16384 : NumericConst
{
    static long Const<long>.Value => 16384L;

    static int Const<int>.Value => 16384;

    static decimal Const<decimal>.Value => 16384m;
}

public sealed class N32768 : NumericConst
{
    static long Const<long>.Value => 32768L;

    static int Const<int>.Value => 32768;

    static decimal Const<decimal>.Value => 32768m;
}

public sealed class N65536 : NumericConst
{
    static long Const<long>.Value => 65536L;

    static int Const<int>.Value => 65536;

    static decimal Const<decimal>.Value => 65536m;
}

public sealed class N100000 : NumericConst
{
    static long Const<long>.Value => 100000L;

    static int Const<int>.Value => 100000;

    static decimal Const<decimal>.Value => 100000m;
}
