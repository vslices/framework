using System.Reflection;
using System.Runtime.CompilerServices;

namespace VSlices.Work.Laws;

/// <summary>
/// Executable structural checks for Feature definitions.
/// These checks cannot prove completion, authority, or behavioral correctness.
/// </summary>
public static class FeatureModelingLaws
{
    public const string Guide = "https://github.com/vslices/framework/blob/master/docs/work/feature-modeling.md";

    public static IReadOnlyList<FeatureLawViolation> InspectDefinitions(
        Assembly assembly, string definitionsNamespace)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        ArgumentException.ThrowIfNullOrWhiteSpace(definitionsNamespace);

        return assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract && !type.IsNested
                && type.Namespace is not null
                && (type.Namespace == definitionsNamespace
                    || type.Namespace.StartsWith(definitionsNamespace + ".", StringComparison.Ordinal)))
            .Where(type => type.GetInterfaces().Any(IsFeatureInterface))
            .OrderBy(type => type.FullName, StringComparer.Ordinal)
            .SelectMany(Inspect)
            .ToArray();
    }

    public static IReadOnlyList<FeatureLawViolation> Inspect(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var violations = new List<FeatureLawViolation>();

        if (!type.IsClass || type.IsAbstract)
            return violations;

        var contracts = type.GetInterfaces().Where(IsFeatureInterface).ToArray();
        if (contracts.Length != 1 || contracts[0].GenericTypeArguments[0] != type)
        {
            violations.Add(Violation(type, "FM001", "The definition must implement Feature<SELF, ALG, REQ, RES> exactly once.", "fm001-feature-identity"));
            return violations;
        }

        var parameters = contracts[0].GenericTypeArguments;
        var algebra = parameters[1];
        var request = type.GetNestedType("Request", BindingFlags.Public | BindingFlags.NonPublic);
        var response = type.GetNestedType("Response", BindingFlags.Public | BindingFlags.NonPublic);

        // Every Feature declares its own runtime view; the module runtime can
        // implement several such interfaces without exposing all capabilities.
        var ownedAlgebra = type.GetNestedType("Algebra", BindingFlags.Public | BindingFlags.NonPublic);
        if (ownedAlgebra is null || ownedAlgebra.DeclaringType != type
            || !ownedAlgebra.IsInterface || !ownedAlgebra.IsNestedPublic
            || algebra != ownedAlgebra)
            violations.Add(Violation(type, "FM011",
                "Declare a public nested interface Algebra and bind ALG to that exact Feature-owned interface.",
                "fm011-feature-owned-algebra"));

        if (request is null || request.DeclaringType != type)
            violations.Add(Violation(type, "FM002", "Request must be declared directly inside its Feature.", "fm002-request-ownership"));

        if (response is null || response.DeclaringType != type)
            violations.Add(Violation(type, "FM003", "Response must be declared directly inside its Feature.", "fm003-response-ownership"));

        if (request != parameters[2] || response != parameters[3])
            violations.Add(Violation(type, "FM004", "Feature generic Request/Response must match the nested nominal types.", "fm004-nominal-binding"));

        if (request is not null)
        {
            if (!request.IsNestedPublic || !request.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any())
                violations.Add(Violation(type, "FM005", "Request must be public and publicly constructible.", "fm005-request-construction"));
            InspectNullable(type, request, "Request", violations);
        }

        if (response is not null)
        {
            var constructors = response.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (!response.IsNestedPublic || constructors.Length == 0 || constructors.Any(ctor => !ctor.IsPrivate))
                violations.Add(Violation(type, "FM006", "Response must be public with exclusively private instance constructors.", "fm006-response-construction"));

            var factory = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.DeclaredOnly)
                .Where(m => m.Name == "NewResponse" && m.IsPrivate && m.ReturnType == response)
                .Where(m => m.GetCustomAttribute<UnsafeAccessorAttribute>()?.Kind == UnsafeAccessorKind.Constructor)
                .ToArray();
            if (factory.Length != 1 || !constructors.Any(ctor => ctor.GetParameters().Select(p => p.ParameterType)
                    .SequenceEqual(factory[0].GetParameters().Select(p => p.ParameterType))))
                violations.Add(Violation(type, "FM008", "Provide one private NewResponse UnsafeAccessor matching a Response constructor.", "fm008-response-factory"));

            const BindingFlags declared = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            if (response.GetProperties(declared).Any(p => p.SetMethod?.IsPublic == true)
                || response.GetFields(declared).Any(f => !f.IsInitOnly))
                violations.Add(Violation(type, "FM010", "Response must not expose publicly writable members.", "fm010-response-immutability"));
            InspectNullable(type, response, "Response", violations);
        }

        return violations;
    }

    private static bool IsFeatureInterface(Type type) =>
        type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Feature<,,,>);

    private static void InspectNullable(Type owner, Type contract, string name, List<FeatureLawViolation> issues)
    {
        var nullable = new NullabilityInfoContext();
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        foreach (var property in contract.GetProperties(flags))
        {
            if (property.GetIndexParameters().Length != 0) continue;
            var info = nullable.Create(property);
            if (IsNullable(property.PropertyType, info.ReadState) || IsNullable(property.PropertyType, info.WriteState))
                issues.Add(Violation(owner, "FM009", $"{name}.{property.Name} is nullable; model meaningful absence explicitly.", "fm009-explicit-optionality"));
        }
        foreach (var field in contract.GetFields(flags))
        {
            var info = nullable.Create(field);
            if (IsNullable(field.FieldType, info.ReadState))
                issues.Add(Violation(owner, "FM009", $"{name}.{field.Name} is nullable; model meaningful absence explicitly.", "fm009-explicit-optionality"));
        }
        foreach (var ctor in contract.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        foreach (var parameter in ctor.GetParameters())
        {
            if (contract.GetProperty(parameter.Name ?? "", BindingFlags.IgnoreCase | flags) is not null) continue;
            var info = nullable.Create(parameter);
            if (IsNullable(parameter.ParameterType, info.ReadState))
                issues.Add(Violation(owner, "FM009", $"{name}.{parameter.Name} is nullable; model meaningful absence explicitly.", "fm009-explicit-optionality"));
        }
    }

    private static bool IsNullable(Type type, NullabilityState state) =>
        Nullable.GetUnderlyingType(type) is not null ||
        (!type.IsValueType && state == NullabilityState.Nullable);

    private static FeatureLawViolation Violation(Type type, string id, string error, string anchor) =>
        new(id, type.FullName ?? type.Name, error, Guide + "#" + anchor);
}

public sealed record FeatureLawViolation(string RuleId, string Definition, string Error, string GuidanceUrl)
{
    public override string ToString() => $"[{RuleId}] {Definition}: {Error} See {GuidanceUrl}";
}
