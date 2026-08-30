using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
/// Defines validation that preserves the input type.
/// </summary>
public interface Validate<SELF, IN> : DomainType<SELF>
    where SELF : Validate<SELF, IN>
{
    public static abstract Req<IN>.Full Invariants { get; }

    public static virtual Fin<IN> Check(IN input) =>
        SELF.Invariants.RunFin(input);

}
