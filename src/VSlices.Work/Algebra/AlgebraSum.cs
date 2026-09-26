namespace VSlices.Work;

/// <summary>
/// Composes independent executable algebras into a larger Flow runtime.
/// Each component remains independently owned and can be projected when a child
/// Flow requires only one part of the composed runtime.
/// </summary>
public sealed record AlgebraSum<TA, TB>(
    TA A,
    TB B);

public sealed record AlgebraSum<TA, TB, TC>(
    TA A,
    TB B,
    TC C);

public sealed record AlgebraSum<TA, TB, TC, TD>(
    TA A,
    TB B,
    TC C,
    TD D);

public sealed record AlgebraSum<TA, TB, TC, TD, TE>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E);

public sealed record AlgebraSum<TA, TB, TC, TD, TE, TF>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E,
    TF F);

public sealed record AlgebraSum<TA, TB, TC, TD, TE, TF, TG>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E,
    TF F,
    TG G);
