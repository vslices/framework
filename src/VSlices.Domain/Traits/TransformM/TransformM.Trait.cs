using LanguageExt;

namespace VSlices.Domain.Traits;

/// <summary>
/// Defines an effectful transformation from <typeparamref name="IN"/> to <typeparamref name="OUT"/>.
/// </summary>
public interface TransformM<SELF, M, OUT, IN> : DomainType<SELF>
    where SELF : TransformM<SELF, M, OUT, IN>
    where M : Monad<M>
{
    public static abstract ReqK<M, IN, OUT>.Full Invariants { get; }

    public static virtual FinT<M, OUT> Create(IN repr) =>
        SELF.Invariants.RunFinT(repr);

    public static virtual K<M, OUT> New(IN repr) =>
        SELF.Create(repr)
            .Run()
            .Map(f => f.ThrowIfFail());
}

/// <summary>
/// Defines an effectful transformation from <typeparamref name="IN"/> to <typeparamref name="SELF"/>.
/// </summary>
public interface TransformM<SELF, M, IN> : TransformM<SELF, M, SELF, IN>
    where SELF : TransformM<SELF, M, IN>
    where M : Monad<M>;
