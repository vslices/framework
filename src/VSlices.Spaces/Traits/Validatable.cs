using LanguageExt;

namespace VSlices.Spaces.Traits;

/// <summary>
/// Defines whether values from <typeparamref name="SPACE"/> are admissible for use
/// in the semantic context represented by <typeparamref name="CTX"/>.
/// </summary>
/// <typeparam name="CTX">The context that owns the admissibility rule.</typeparam>
/// <typeparam name="SPACE">The space whose use is being validated.</typeparam>
public interface Validatable<CTX, SPACE>
    where CTX : Validatable<CTX, SPACE>
{
    /// <summary>
    /// Gets the rules that determine whether a value from the space may be used in this context.
    /// </summary>
    static abstract Fin<SPACE> Validate(SPACE value);
}

/// <summary>
/// Operations for evaluating contextual admissibility of spaces.
/// </summary>
public static class Validatable
{
    public static Fin<SPACE> Check<CTX, SPACE>(SPACE value)
        where CTX : Validatable<CTX, SPACE> =>
        CTX.Validate(value);
}
