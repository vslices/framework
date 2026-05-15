using VSlices.Literals.Abstracts;

namespace VSlices.Literals;

/// <summary>
/// Represents the number one thousand (1000) as a strongly-typed constant.
/// </summary>
public sealed class N1000 : N<int, N1, N0, N0, N0>
{
    /// <summary>
    /// Represents the number one thousand (1000) as a strongly-typed constant of type <see cref="uint"/>.
    /// </summary>
    public sealed class U : N<uint, N1.U, N0.U, N0.U, N0.U>;

    /// <summary>
    /// Represents the number one thousand (1000) as a strongly-typed constant of type <see cref="long"/>.
    /// </summary>
    public sealed class L : N<long, N1.L, N0.L, N0.L, N0.L>;

    /// <summary>
    /// Represents the number one thousand (1000) as a strongly-typed constant of type <see cref="ulong"/>.
    /// </summary>
    public sealed class UL : N<ulong, N1.UL, N0.UL, N0.UL, N0.UL>;
}

/// <summary>
/// Represents the number one thousand twenty-three (1023) as a strongly-typed constant.
/// </summary>
public sealed class N1023 : N<int, N1, N0, N2, N3>
{
    /// <summary>
    /// Represents the number one thousand twenty-three (1023) as a strongly-typed constant of type <see cref="uint"/>.
    /// </summary>
    public sealed class U : N<uint, N1.U, N0.U, N2.U, N3.U>;

    /// <summary>
    /// Represents the number one thousand twenty-three (1023) as a strongly-typed constant of type <see cref="long"/>.
    /// </summary>
    public sealed class L : N<long, N1.L, N0.L, N2.L, N3.L>;

    /// <summary>
    /// Represents the number one thousand twenty-three (1023) as a strongly-typed constant of type <see cref="ulong"/>.
    /// </summary>
    public sealed class UL : N<ulong, N1.UL, N0.UL, N2.UL, N3.UL>;
}

/// <summary>
/// Represents the number one thousand twenty-four (1024) as a strongly-typed constant.
/// </summary>
public sealed class N1024 : N<int, N1, N0, N2, N4>
{
    /// <summary>
    /// Represents the number one thousand twenty-four (1024) as a strongly-typed constant of type <see cref="uint"/>.
    /// </summary>
    public sealed class U : N<uint, N1.U, N0.U, N2.U, N4.U>;

    /// <summary>
    /// Represents the number one thousand twenty-four (1024) as a strongly-typed constant of type <see cref="long"/>.
    /// </summary>
    public sealed class L : N<long, N1.L, N0.L, N2.L, N4.L>;

    /// <summary>
    /// Represents the number one thousand twenty-four (1024) as a strongly-typed constant of type <see cref="ulong"/>.
    /// </summary>
    public sealed class UL : N<ulong, N1.UL, N0.UL, N2.UL, N4.UL>;
}
