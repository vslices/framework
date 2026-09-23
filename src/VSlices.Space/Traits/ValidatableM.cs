using LanguageExt;
using VSlices.Arrows;

namespace VSlices.Space.Traits;

/// <summary>
/// Defines effectful contextual admissibility for values from <typeparamref name="SPACE"/>.
/// </summary>
/// <typeparam name="CTX">The context that owns the admissibility rule.</typeparam>
/// <typeparam name="M">The effect in which admissibility is established.</typeparam>
/// <typeparam name="SPACE">The space whose use is being validated.</typeparam>
public interface ValidatableM<CTX, M, SPACE>
    where CTX : ValidatableM<CTX, M, SPACE>
    where M : Monad<M>
{
    /// <summary>
    /// Gets the effectful rules that determine whether a value from the space may be used in this context.
    /// </summary>
    static abstract ReqK<M, SPACE>.Full Validation { get; }
}

/// <summary>
/// Operations for evaluating effectful contextual admissibility of spaces.
/// </summary>
public static class ValidatableM
{
    public static FinT<M, SPACE> Check<CTX, M, SPACE>(SPACE value)
        where CTX : ValidatableM<CTX, M, SPACE>
        where M : Monad<M> =>
        CTX.Validation.RunFinT(value);
}
