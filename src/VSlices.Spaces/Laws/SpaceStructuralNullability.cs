using System.Reflection;

namespace VSlices.Spaces.Laws;

internal static class SpaceStructuralNullability
{
    public static void Inspect(Type owner, Type contract, string name, List<SpaceLawViolation> issues)
    {
        var context = new NullabilityInfoContext();
        const BindingFlags declared = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

        foreach (var property in contract.GetProperties(declared))
        {
            if (property.GetIndexParameters().Length != 0) continue;
            var info = context.Create(property);
            if (IsNullable(property.PropertyType, info.ReadState) ||
                IsNullable(property.PropertyType, info.WriteState))
                issues.Add(Violation(owner, name, property.Name));
        }
        foreach (var field in contract.GetFields(declared))
        {
            var info = context.Create(field);
            if (IsNullable(field.FieldType, info.ReadState) ||
                IsNullable(field.FieldType, info.WriteState))
                issues.Add(Violation(owner, name, field.Name));
        }
        foreach (var ctor in contract.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
        foreach (var parameter in ctor.GetParameters())
        {
            var member = parameter.Name ?? "unnamed";
            if (contract.GetProperty(member, BindingFlags.IgnoreCase | declared) is not null)
                continue;
            var info = context.Create(parameter);
            if (IsNullable(parameter.ParameterType, info.ReadState))
                issues.Add(Violation(owner, name, member));
        }
    }

    private static bool IsNullable(Type type, NullabilityState state) =>
        Nullable.GetUnderlyingType(type) is not null ||
        (!type.IsValueType && state == NullabilityState.Nullable);

    private static SpaceLawViolation Violation(Type type, string contract, string member) =>
        new("SM007", type.FullName ?? type.Name,
            $"{contract}.{member} is nullable; model absence with ForkExt Option<A>, not C# nullable types.",
            SpaceModelingLaws.Guide + "#sm007-explicit-optionality");
}
