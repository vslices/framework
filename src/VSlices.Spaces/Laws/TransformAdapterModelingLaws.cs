using System.Reflection;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Laws;

/// <summary>
/// Reflection-based, consumer-invoked checks for closed TransformAdapter contracts.
/// This does not run as a compiler analyzer or inspect generic usages in source code.
/// </summary>
public static class TransformAdapterModelingLaws
{
    public const string Guide =
        "https://github.com/vslices/framework/blob/master/docs/transform-adapter.md";

    public static IReadOnlyList<TransformAdapterLawViolation> InspectTextInput(Type target) =>
        Inspect(typeof(string), target);

    public static IReadOnlyList<TransformAdapterLawViolation> Inspect(Type source, Type target)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(target);
        var issues = new List<TransformAdapterLawViolation>();

        // Open generic types cannot establish a closed source/target contract.
        if (source.ContainsGenericParameters || target.ContainsGenericParameters)
        {
            issues.Add(Violation(source, target, "TA000",
                "Only closed source and target types can be inspected.", "ta000-closed-contracts"));
            return issues;
        }

        if (OwnsTransform(target, source))
            return issues;

        var inputTypes = target.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic)
            .Where(type => type.Name == "Input" && !type.IsGenericType)
            .ToArray();
        if (inputTypes.Length != 1)
        {
            issues.Add(Violation(source, target, "TA001",
                "The target must declare one non-generic nested Input.", "ta001-nested-input"));
            return issues;
        }

        var input = inputTypes[0];
        var constructors = input.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Where(ctor => ctor.GetParameters().Length > 0)
            .ToArray();
        if (constructors.Length != 1 ||
            constructors[0].GetParameters().Length != 1 ||
            constructors[0].GetParameters()[0].ParameterType != source)
        {
            issues.Add(Violation(source, target, "TA002",
                "Input requires exactly one non-empty public constructor taking the source type.",
                "ta002-input-constructor"));
            return issues;
        }

        if (!OwnsTransform(target, input))
            issues.Add(Violation(source, target, "TA003",
                "Target must implement Transformable<Target.Input, Target>.",
                "ta003-canonical-transform"));

        return issues;
    }

    private static bool OwnsTransform(Type target, Type input) =>
        target.GetInterfaces().Any(i =>
            i.IsGenericType &&
            i.GetGenericTypeDefinition() == typeof(Transformable<,>) &&
            i.GenericTypeArguments[0] == input &&
            i.GenericTypeArguments[1] == target);

    private static TransformAdapterLawViolation Violation(
        Type source, Type target, string id, string message, string anchor) =>
        new(id, source.FullName ?? source.Name, target.FullName ?? target.Name,
            message, Guide + "#" + anchor);
}

public sealed record TransformAdapterLawViolation(
    string RuleId, string Source, string Target, string Error, string GuidanceUrl)
{
    public override string ToString() =>
        $"[{RuleId}] {Source} -> {Target}: {Error} See {GuidanceUrl}";
}
