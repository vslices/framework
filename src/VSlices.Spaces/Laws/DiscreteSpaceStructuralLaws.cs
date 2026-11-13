using VSlices.Spaces;

namespace VSlices.Spaces.Laws;

/// <summary>
/// Verifies nominal conformance to DiscreteSpace&lt;SELF&gt;.
/// Equality behavior is verified separately with concrete witnesses.
/// </summary>
public static class DiscreteSpaceStructuralLaws
{
    public static IReadOnlyList<SpaceLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsClass || type.IsAbstract)
            return [];

        var nominal = type.GetInterfaces().Any(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(DiscreteSpace<>) &&
            i.GenericTypeArguments[0] == type);

        return nominal
            ? []
            : [new SpaceLawViolation(
                "SM008",
                type.FullName ?? type.Name,
                "Concrete Space must implement DiscreteSpace<SELF>, directly or through a trait inheriting it.",
                SpaceModelingLaws.Guide + "#sm008-nominal-discrete-space")];
    }
}
