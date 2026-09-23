using LanguageExt;
using VSlices.Arrows;

namespace VSlices.Space.Traits;

/// <summary>
/// Defines an effectful semantic transformation from <typeparamref name="FROM"/> to
/// <typeparamref name="TO"/> whose rules are owned by <typeparamref name="CTX"/>.
/// </summary>
public interface TransformableM<CTX, M, FROM, TO>
    where CTX : TransformableM<CTX, M, FROM, TO>
    where M : Monad<M>
{
    /// <summary>
    /// Gets the effectful rules that establish a target value from a source value.
    /// </summary>
    static abstract ReqK<M, FROM, TO>.Full Transformation { get; }
}

/// <summary>
/// Defines a target-owned effectful semantic transformation from <typeparamref name="FROM"/>
/// to <typeparamref name="TO"/>.
/// </summary>
public interface TransformableM<M, FROM, TO> : TransformableM<TO, M, FROM, TO>
    where TO : TransformableM<M, FROM, TO>
    where M : Monad<M>;

/// <summary>
/// Operations for executing effectful semantic transformations.
/// </summary>
public static class TransformableM
{
    public static FinT<M, TO> Transform<CTX, M, FROM, TO>(FROM source)
        where CTX : TransformableM<CTX, M, FROM, TO>
        where M : Monad<M> =>
        CTX.Transformation.RunFinT(source);

    public static FinT<M, TO> Transform<M, FROM, TO>(FROM source)
        where TO : TransformableM<M, FROM, TO>
        where M : Monad<M> =>
        TO.Transformation.RunFinT(source);
}
