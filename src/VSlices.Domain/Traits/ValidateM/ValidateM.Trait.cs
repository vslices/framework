using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
/// Defines effectful validation that preserves the input type.
/// </summary>
public interface ValidateM<SELF, M, IN> : DomainType<SELF>
    where SELF : ValidateM<SELF, M, IN>
    where M : Monad<M>
{
    public static abstract ReqK<M, IN>.Full Invariants { get; }

    public static virtual FinT<M, IN> Check(IN input) =>
        SELF.Invariants.RunFinT(input);

    public static virtual K<M, IN> CheckUnsafe(IN input) =>
        SELF.Check(input)
            .Run()
            .Map(f => f.ThrowIfFail());
}
