using System.Reflection;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Laws;

/// <summary>
/// Executable, structural integrity checks for explicitly scoped .NET Space definitions.
/// They do not prove semantic admissibility or transition correctness.
/// </summary>
public static class SpaceModelingLaws
{
    public const string Guide = "https://github.com/vslices/framework/blob/master/docs/spaces/modeling.md";

    /// <summary>
    /// Inspect concrete top-level classes in a Definitions namespace (including descendants).
    /// The consumer owns the choice of namespace; this API cannot inspect source folders.
    /// </summary>
    public static IReadOnlyList<SpaceLawViolation> InspectDefinitions(
        Assembly assembly, string definitionsNamespace)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionsNamespace);

        return assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !type.IsNested
                && type.Namespace is not null
                && (type.Namespace == definitionsNamespace
                    || type.Namespace.StartsWith(definitionsNamespace + ".", StringComparison.Ordinal)))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .SelectMany(Inspect)
            .ToArray();
    }

    /// <summary>Inspect one semantic type without requiring a namespace convention.</summary>
    public static IReadOnlyList<SpaceLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var issues = new List<SpaceLawViolation>();

        if (!type.IsClass || type.IsAbstract)
            return issues;

        var interfaces = type.GetInterfaces();
        bool ownsTransform = interfaces.Any(i =>
            i.IsGenericType
            && ((i.GetGenericTypeDefinition() == typeof(Transformable<,>)
                    && i.GenericTypeArguments[1] == type)
                || (i.GetGenericTypeDefinition() == typeof(Transformable<,,>)
                    && i.GenericTypeArguments[0] == type)));

        if (!ownsTransform)
            issues.Add(Violation(type, "SM001", "The definition does not own a Transformable rule.",
                "sm001-semantic-construction"));


        // A definition owns its own construction input contract.
        var input = type.GetNestedType("Input", BindingFlags.Public | BindingFlags.NonPublic);
        var usesOwnInput = input is not null && interfaces.Any(i =>
            i.IsGenericType
            && ((i.GetGenericTypeDefinition() == typeof(Transformable<,>)
                    && i.GenericTypeArguments[0] == input
                    && i.GenericTypeArguments[1] == type)
                || (i.GetGenericTypeDefinition() == typeof(Transformable<,,>)
                    && i.GenericTypeArguments[0] == type
                    && i.GenericTypeArguments[1] == input
                    && i.GenericTypeArguments[2] == type)));
        if (!usesOwnInput)
            issues.Add(Violation(type, "SM006",
                "Define nested Input and use it as the source of Transformable<Input, SELF>.",
                "sm006-input-ownership"));

        // A public constructor can bypass Transform and materialize an unchecked point.
        if (type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length != 0)
            issues.Add(Violation(type, "SM002", "A public instance constructor bypasses semantic construction.",
                "sm002-constructor-encapsulation"));

        var evolvable = interfaces.FirstOrDefault(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(Evolvable<,>)
            && i.GenericTypeArguments[0] == type);

        if (evolvable is not null)
        {
            var stateType = evolvable.GenericTypeArguments[1];
            if (!stateType.IsNested || stateType.DeclaringType != type)
                issues.Add(Violation(type, "SM003", "Evolvable State must be nested in its semantic owner.",
                    "sm003-evolution-state-ownership"));
            else if (stateType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                         .Any(ctor => !ctor.IsPrivate))
                issues.Add(Violation(type, "SM004", "State has an externally accessible constructor.",
                    "sm004-state-constructor-encapsulation"));

            var currentState = type.GetProperty("CurrentState",
                BindingFlags.Instance | BindingFlags.Public);
            if (currentState?.PropertyType != stateType || currentState.SetMethod is not null)
                issues.Add(Violation(type, "SM005", "CurrentState must be a publicly readable, non-settable State.",
                    "sm005-accepted-state-exposure"));
        }

        return issues;
    }

    private static SpaceLawViolation Violation(Type type, string id, string message, string anchor) =>
        new(id, type.FullName ?? type.Name, message, Guide + "#" + anchor);
}

/// <summary>Actionable result with a stable rule identifier and an exact documentation anchor.</summary>
public sealed record SpaceLawViolation(string RuleId, string Definition, string Error, string GuidanceUrl)
{
    public override string ToString() =>
        $"[{RuleId}] {Definition}: {Error} See {GuidanceUrl}";
}
