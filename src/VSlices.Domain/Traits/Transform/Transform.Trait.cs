using VSlices.Monads;
using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
/// Defines a transformation from <typeparamref name="IN"/> to <typeparamref name="OUT"/>.
/// </summary>
public interface Transform<SELF, OUT, IN> : DomainType<SELF>
    where SELF : Transform<SELF, OUT, IN>
{
    public static abstract Req<IN, OUT>.Full Invariants { get; }

    public static virtual Fin<OUT> Create(IN repr) =>
        SELF.Invariants.RunFin(repr);

    public static virtual OUT New(IN repr) =>
        SELF.Create(repr).ThrowIfFail();

    public static virtual Seq<OUT> New(Seq<IN> repr) =>
        repr.Map(SELF.New);
}

/// <summary>
/// Defines a transformation from <typeparamref name="IN"/> to <typeparamref name="SELF"/>.
/// </summary>
public interface Transform<SELF, IN> : Transform<SELF, SELF, IN>
    where SELF : Transform<SELF, IN>;
