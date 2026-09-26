using System.Linq.Expressions;
using System.Reflection;
using LanguageExt;
using LanguageExt.Common;
using VSlices.Arrows;

namespace VSlices.Space.Traits;

/// <summary>
/// Adapts a simple representation into the canonical nested Input of a semantic
/// target and then delegates to the target-owned transformation.
/// </summary>
/// <typeparam name="FROM">The representation accepted by the adapter.</typeparam>
/// <typeparam name="TO">The semantic target.</typeparam>
/// <remarks>
/// If <typeparamref name="TO"/> already owns a direct
/// <c>Transformable&lt;FROM, TO&gt;</c>, that canonical transformation is used as-is.
/// Otherwise the adapter convention requires <typeparamref name="TO"/> to declare a
/// nested type named <c>Input</c>, that Input to expose exactly one non-empty public
/// constructor with exactly one parameter of type <typeparamref name="FROM"/>, and
/// the target to implement <c>Transformable&lt;TO.Input, TO&gt;</c>.
///
/// These conditions are verified statically by the VSlices analyzer when the
/// closed adapter is visible to Roslyn. Runtime validation remains as a safety
/// boundary for open generic consumers such as UI components.
/// </remarks>
public sealed class TransformAdapter<FROM, TO> :
    Transformable<TransformAdapter<FROM, TO>, FROM, TO>
{
    private static readonly Func<FROM, Fin<TO>> Adapt = CreateAdapter();

    public static Req<FROM, TO>.Full Transformation =>
        new Req<FROM, TO, FROM, TO>(
            (_, previous) =>
                previous.Bind(state =>
                {
                    if (!state.IsValid)
                    {
                        return Either.Left<Error, ReqState<TO>>(
                            state.Error);
                    }

                    return Adapt(state.Value).Match(
                        Succ: value =>
                            Either.Right<Error, ReqState<TO>>(
                                ReqState.New(value)),
                        Fail: error =>
                            Either.Left<Error, ReqState<TO>>(
                                error));
                }));

    public static Fin<TO> Transform(FROM source) =>
        Transformation.RunFin(source);

    private static Func<FROM, Fin<TO>> CreateAdapter()
    {
        var targetType = typeof(TO);

        if (ImplementsTargetOwnedTransform(
                targetType,
                typeof(FROM)))
        {
            var directBridgeType = typeof(DirectTransformAdapterBridge<,>)
                .MakeGenericType(
                    typeof(FROM),
                    targetType);

            var createDirect = directBridgeType.GetMethod(
                "Create",
                BindingFlags.Public | BindingFlags.Static)
                ?? throw InvalidConvention(
                    "The direct TransformAdapter bridge could not be created.");

            return (Func<FROM, Fin<TO>>)
                (createDirect.Invoke(null, null)
                 ?? throw InvalidConvention(
                     "The direct TransformAdapter bridge returned no delegate."));
        }

        var inputType = targetType.GetNestedType(
            "Input",
            BindingFlags.Public | BindingFlags.NonPublic);

        if (inputType is null)
        {
            throw InvalidConvention(
                $"'{targetType}' must declare a nested type named 'Input'.");
        }

        var constructors = inputType
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Where(static constructor =>
                constructor.GetParameters().Length > 0)
            .ToArray();

        if (constructors.Length != 1)
        {
            throw InvalidConvention(
                $"'{inputType}' must expose exactly one non-empty public instance constructor.");
        }

        var constructor = constructors[0];
        var parameters = constructor.GetParameters();

        if (parameters.Length != 1 ||
            parameters[0].ParameterType != typeof(FROM))
        {
            throw InvalidConvention(
                $"'{inputType}' must expose one public constructor whose only parameter is '{typeof(FROM)}'.");
        }

        if (!ImplementsTargetOwnedTransform(
                targetType,
                inputType))
        {
            throw InvalidConvention(
                $"'{targetType}' must implement Transformable<{inputType.Name}, {targetType.Name}>.");
        }

        var bridgeType = typeof(TransformAdapterBridge<,,>)
            .MakeGenericType(
                typeof(FROM),
                inputType,
                targetType);

        var create = bridgeType.GetMethod(
            nameof(TransformAdapterBridge<object, object, Probe>.Create),
            BindingFlags.Public | BindingFlags.Static)
            ?? throw InvalidConvention(
                "The typed TransformAdapter bridge could not be created.");

        return (Func<FROM, Fin<TO>>)
            (create.Invoke(null, [constructor])
             ?? throw InvalidConvention(
                 "The typed TransformAdapter bridge returned no delegate."));
    }

    private static bool ImplementsTargetOwnedTransform(
        Type targetType,
        Type sourceType) =>
        targetType
            .GetInterfaces()
            .Any(@interface =>
                @interface.IsGenericType &&
                @interface.GetGenericTypeDefinition() ==
                    typeof(Transformable<,>) &&
                @interface.GenericTypeArguments[0] == sourceType &&
                @interface.GenericTypeArguments[1] == targetType);

    private static InvalidOperationException InvalidConvention(
        string message) =>
        new(
            $"TransformAdapter<{typeof(FROM).Name}, {typeof(TO).Name}> cannot be created. {message}");

    private sealed class Probe :
        Transformable<object, Probe>
    {
        public static Req<object, Probe>.Full Transformation =>
            Req<object, Probe>.Transform<object, Probe>(
                static _ => new Probe());
    }
}

internal static class TransformAdapterBridge<TFrom, TInput, TTo>
    where TTo : Transformable<TInput, TTo>
{
    public static Func<TFrom, Fin<TTo>> Create(
        ConstructorInfo constructor)
    {
        var source = Expression.Parameter(
            typeof(TFrom),
            "source");

        var constructInput = Expression.Lambda<Func<TFrom, TInput>>(
            Expression.New(constructor, source),
            source)
            .Compile();

        return value =>
            TTo.Transformation.RunFin(
                constructInput(value));
    }
}


internal static class DirectTransformAdapterBridge<TFrom, TTo>
    where TTo : Transformable<TFrom, TTo>
{
    public static Func<TFrom, Fin<TTo>> Create() =>
        static value =>
            TTo.Transformation.RunFin(value);
}
