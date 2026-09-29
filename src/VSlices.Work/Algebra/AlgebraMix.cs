namespace VSlices.Work;

/// <summary>
/// Mixes independently owned executable algebras into one larger Flow runtime.
/// Each component remains independently owned and can be projected when a child
/// Flow requires only one part of the mixed runtime.
/// The name intentionally makes no claim about categorical product/coproduct semantics.
/// </summary>
public sealed record AlgebraMix<TA, TB>(
    TA A,
    TB B);

public sealed record AlgebraMix<TA, TB, TC>(
    TA A,
    TB B,
    TC C);

public sealed record AlgebraMix<TA, TB, TC, TD>(
    TA A,
    TB B,
    TC C,
    TD D);

public sealed record AlgebraMix<TA, TB, TC, TD, TE>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E);

public sealed record AlgebraMix<TA, TB, TC, TD, TE, TF>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E,
    TF F);

public sealed record AlgebraMix<TA, TB, TC, TD, TE, TF, TG>(
    TA A,
    TB B,
    TC C,
    TD D,
    TE E,
    TF F,
    TG G);
