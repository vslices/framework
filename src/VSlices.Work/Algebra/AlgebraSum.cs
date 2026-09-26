namespace VSlices.Work;

/// <summary>
/// Composes independent executable algebras into a larger Flow runtime.
/// Each component remains independently owned and can be projected when a child
/// Flow requires only one part of the composed runtime.
/// </summary>
public sealed record AlgebraSum<A, B>(
    A A,
    B B);

public sealed record AlgebraSum<A, B, C>(
    A A,
    B B,
    C C);

public sealed record AlgebraSum<A, B, C, D>(
    A A,
    B B,
    C C,
    D D);

public sealed record AlgebraSum<A, B, C, D, E>(
    A A,
    B B,
    C C,
    D D,
    E E);

public sealed record AlgebraSum<A, B, C, D, E, F>(
    A A,
    B B,
    C C,
    D D,
    E E,
    F F);

public sealed record AlgebraSum<A, B, C, D, E, F, G>(
    A A,
    B B,
    C C,
    D D,
    E E,
    F F,
    G G);
