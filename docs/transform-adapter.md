# TransformAdapter

## Purpose

`TransformAdapter<FROM, TO>` lets presentation code materialize a semantic target from a simpler representation without forcing the target to own every presentation-specific transformation directly.

The current UI pressure case is:

```text
string
    -> EmailAddress.Input
    -> EmailAddress
```

rather than requiring:

```text
EmailAddress : Transformable<string, EmailAddress>
```

only because a text input happens to produce a string.

## Semantic ownership

The adapter preserves two different responsibilities:

```text
FROM -> TO.Input
    mechanical adaptation owned by TransformAdapter

TO.Input -> TO
    canonical semantic transformation owned by TO
```

The adapter does not replace, reproduce, or reinterpret `TO.Transformation`.

Its explicit transformation context is:

```csharp
Transformable<
    TransformAdapter<FROM, TO>,
    FROM,
    TO>
```

This is intentionally different from claiming that `TO` owns `Transformable<FROM, TO>`.

## Direct transformations are preserved

If `TO` already owns:

```csharp
Transformable<FROM, TO>
```

the adapter delegates directly to that transformation.

For example, a simple semantic text value may legitimately own:

```text
string -> LocationName
```

and does not need an artificial nested `Input`.

## Nested Input convention

When no direct target-owned transformation exists, the adapter requires:

1. `TO` declares exactly one non-generic nested type named `Input`;
2. `TO.Input` exposes exactly one non-empty public instance constructor;
3. that constructor has exactly one parameter and its type is `FROM`;
4. `TO` implements `Transformable<TO.Input, TO>`.

The parameterless constructor structurally available on value types is ignored.

Example:

```csharp
public sealed record EmailAddress :
    Transformable<EmailAddress.Input, EmailAddress>
{
    public readonly record struct Input(string Value);

    public static Req<Input, EmailAddress>.Full Transformation => ...;
}
```

can be used as:

```csharp
TransformAdapter<string, EmailAddress>
    .Transform(value);
```

## UI

`vTextInput<T>` uses:

```csharp
TransformAdapter<string, T>
```

instead of requiring:

```csharp
where T : Transformable<string, T>
```

This lets UI consume richer semantic targets while keeping the canonical creation contract in `T.Input`.

A target whose `Input` requires several independent values is deliberately not adaptable from one text value. Presentation must construct the richer input explicitly rather than asking reflection to invent missing semantics.

## Analyzer

The `VSlices.Space` package ships the analyzer as a compiler asset.

Current diagnostics are:

```text
VST001
    target requires canonical nested Input when no direct transform exists

VST002
    Input constructor is incompatible with the adapter source

VST003
    target does not own Transformable<TO.Input, TO>
```

The analyzer checks both explicit `TransformAdapter<FROM, TO>` usages and closed `vTextInput<TO>` usages.

Generated code analysis is enabled because Razor may materialize closed generic component types in generated C#.

## Runtime boundary

Reflection is a .NET realization mechanism, not the semantic definition of TransformAdapter.

Reflection is performed once per closed `TransformAdapter<FROM, TO>` pair. The result is cached as a strongly typed delegate.

Runtime convention validation remains present because an open generic component such as `vTextInput<T>` cannot always be proven valid when its assembly is compiled. The analyzer remains the preferred early feedback mechanism when the closed target is visible to Roslyn.

## Current boundary

TransformAdapter currently solves only:

```text
one representation value
    -> one canonical Input constructor
    -> canonical target transformation
```

It does not infer:

- multi-field Inputs;
- defaults;
- semantic values for missing fields;
- field names or mapping policy;
- alternate constructors;
- authorization;
- effects.

Those require explicit presentation or Work logic rather than a broader reflective adapter.
