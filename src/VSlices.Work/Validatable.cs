using LanguageExt;
using VSlices.Arrows;

namespace VSlices.Work;

/// <summary>
/// Defines contextual use-case admissibility that preserves the input type.
/// </summary>
public interface Validatable<CTX, IN>
    where CTX : Validatable<CTX, IN>
{
    static abstract Req<IN>.Full Validation { get; }
}

public static class Validatable
{
    public static Fin<IN> Check<CTX, IN>(IN input)
        where CTX : Validatable<CTX, IN> =>
        CTX.Validation.RunFin(input);
}
