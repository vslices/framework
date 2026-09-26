namespace VSlices.Work;

/// <summary>
/// Exposes a concrete executable algebra from Grounding.
/// </summary>
/// <typeparam name="ALG">The Work algebra being realized.</typeparam>
public interface AlgebraIO<ALG>
{
    ALG Algebra { get; }
}
