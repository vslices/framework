using System.Reflection;

namespace VSlices.Spaces.Laws;

/// <summary>
/// Facade coordinating structural checks for concrete semantic Space definitions.
/// Semantic equality laws require caller-supplied witnesses and are not run by reflection.
/// </summary>
public static class SpaceModelingLaws
{
    public const string Guide = "https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md";

    /// <summary>Inspect concrete top-level classes in a selected Definitions namespace.</summary>
    public static IReadOnlyList<SpaceLawViolation> InspectDefinitions(
        Assembly assembly, string definitionsNamespace)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionsNamespace);

        return assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !type.IsNested &&
                type.Namespace is not null &&
                (type.Namespace == definitionsNamespace ||
                 type.Namespace.StartsWith(definitionsNamespace + ".", StringComparison.Ordinal)))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .SelectMany(Inspect)
            .ToArray();
    }

    /// <summary>
    /// Inspect structural contracts only. Behavioral equality checks use
    /// DiscreteSpaceSemanticLaws.Verify with concrete witnesses.
    /// </summary>
    public static IReadOnlyList<SpaceLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsClass || type.IsAbstract) return [];

        return TransformableStructuralLaws.Inspect(type)
            .Concat(EvolvableStructuralLaws.Inspect(type))
            .Concat(DiscreteSpaceStructuralLaws.Inspect(type))
            .ToArray();
    }
}

/// <summary>Actionable result with a stable rule identifier and guidance link.</summary>
public sealed record SpaceLawViolation(string RuleId, string Definition, string Error, string GuidanceUrl)
{
    public override string ToString() =>
        $"[{RuleId}] {Definition}: {Error} See {GuidanceUrl}";
}
