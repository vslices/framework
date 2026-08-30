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

    public static virtual FinT<M, IN> CheckM(IN input) =>
        SELF.Invariants.RunFinT(input);
    
}
