using LanguageExt;
using VSlices.Arrows;

namespace VSlices.Work;

/// <summary>
/// Defines effectful contextual use-case admissibility that preserves the input type.
/// </summary>
public interface ValidatableM<CTX, M, IN>
    where CTX : ValidatableM<CTX, M, IN>
    where M : Monad<M>
{
    static abstract ReqK<M, IN>.Full Validation { get; }
}

public static class ValidatableM
{
    public static FinT<M, IN> Check<CTX, M, IN>(IN input)
        where CTX : ValidatableM<CTX, M, IN>
        where M : Monad<M> =>
        CTX.Validation.RunFinT(input);
}
