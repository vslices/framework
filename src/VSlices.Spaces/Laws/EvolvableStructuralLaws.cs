using System.Reflection;

namespace VSlices.Spaces.Laws;

/// <summary>Checks structural ownership and exposure of evolvable State.</summary>
public static class EvolvableStructuralLaws
{
    public static IReadOnlyList<SpaceLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsClass || type.IsAbstract) return [];

        var issues = new List<SpaceLawViolation>();
        var evolvable = type.GetInterfaces().FirstOrDefault(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(Evolvable<,>) &&
            i.GenericTypeArguments[0] == type);
        if (evolvable is null) return issues;

        var state = evolvable.GenericTypeArguments[1];
        if (state.IsNested && state.DeclaringType == type)
            SpaceStructuralNullability.Inspect(type, state, "State", issues);

        if (!state.IsNested || state.DeclaringType != type)
            issues.Add(Violation(type, "SM003", "Evolvable State must be nested in its semantic owner.",
                "sm003-evolution-state-ownership"));
        else if (state.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                      .Any(ctor => !ctor.IsPrivate))
            issues.Add(Violation(type, "SM004", "State has an externally accessible constructor.",
                "sm004-state-constructor-encapsulation"));

        var current = type.GetProperty("CurrentState", BindingFlags.Instance | BindingFlags.Public);
        if (current?.PropertyType != state || current.SetMethod is not null)
            issues.Add(Violation(type, "SM005", "CurrentState must be a publicly readable, non-settable State.",
                "sm005-accepted-state-exposure"));

        return issues;
    }

    private static SpaceLawViolation Violation(Type type, string id, string message, string anchor) =>
        new(id, type.FullName ?? type.Name, message, SpaceModelingLaws.Guide + "#" + anchor);
}
