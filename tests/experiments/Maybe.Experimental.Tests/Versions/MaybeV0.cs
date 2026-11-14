namespace Maybe.Experimental.MaybeV0;

/// <summary>
/// V0: a discriminated union expressed with ordinary C# inheritance.
/// No LanguageExt, higher-kinded types, traits or monadic operations.
/// </summary>
public abstract record MaybeV0<A>;

public sealed record SomeV0<A>(A Value) : MaybeV0<A>;

public sealed record NoneV0<A> : MaybeV0<A>;
