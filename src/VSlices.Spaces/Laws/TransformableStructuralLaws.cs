using System.Reflection;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Laws;

/// <summary>Checks ownership of pure semantic construction and nested input.</summary>
public static class TransformableStructuralLaws
{
    public static IReadOnlyList<SpaceLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        if (!type.IsClass || type.IsAbstract) return [];

        var issues = new List<SpaceLawViolation>();
        var interfaces = type.GetInterfaces();
        var ownsTransform = interfaces.Any(i => i.IsGenericType &&
            ((i.GetGenericTypeDefinition() == typeof(Transformable<,>) && i.GenericTypeArguments[1] == type) ||
             (i.GetGenericTypeDefinition() == typeof(Transformable<,,>) && i.GenericTypeArguments[0] == type)));
        if (!ownsTransform)
            issues.Add(Violation(type, "SM001", "The definition does not own a Transformable rule.", "sm001-semantic-construction"));

        var input = type.GetNestedType("Input", BindingFlags.Public | BindingFlags.NonPublic);
        var usesOwnInput = input is not null && interfaces.Any(i => i.IsGenericType &&
            ((i.GetGenericTypeDefinition() == typeof(Transformable<,>) &&
              i.GenericTypeArguments[0] == input && i.GenericTypeArguments[1] == type) ||
             (i.GetGenericTypeDefinition() == typeof(Transformable<,,>) &&
              i.GenericTypeArguments[0] == type && i.GenericTypeArguments[1] == input &&
              i.GenericTypeArguments[2] == type)));
        if (!usesOwnInput)
            issues.Add(Violation(type, "SM006",
                "Define nested Input and use it as the source of Transformable<Input, SELF>.",
                "sm006-input-ownership"));

        if (input is not null)
            SpaceStructuralNullability.Inspect(type, input, "Input", issues);

        if (type.GetConstructors(BindingFlags.Instance | BindingFlags.Public).Length != 0)
            issues.Add(Violation(type, "SM002",
                "A public instance constructor bypasses semantic construction.",
                "sm002-constructor-encapsulation"));

        return issues;
    }

    private static SpaceLawViolation Violation(Type type, string id, string message, string anchor) =>
        new(id, type.FullName ?? type.Name, message, SpaceModelingLaws.Guide + "#" + anchor);
}
