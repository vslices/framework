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

    public static Fin<EmailAddress> Transform(Input input) => ...;
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

## Executable modeling Laws (current consumer contract)

The current NuGet preview does **not** distribute the experimental analyzer.
Consumers can explicitly check closed adapter pairs in tests using
`TransformAdapterModelingLaws.Inspect(typeof(string), typeof(EmailAddress))`.
For the `vTextInput<T>` convention, use
`TransformAdapterModelingLaws.InspectTextInput(typeof(EmailAddress))`,
which checks adaptation from `string`. These reflection checks share the
runtime's source/target convention; they do not discover closed usages
automatically or provide build-time diagnostics.

### TA000 — Closed contracts

**Error:** Source or target contains unbound generic type parameters.

**Why:** Reflection cannot evaluate a not-yet-closed adapter pair.

**Preferred realization:** Inspect concrete source and target types.

**Alternative and limit:** Open generic consumers require a closed instantiation
before these Laws can establish the convention.

### TA001 — Nested Input

**Error:** When no direct target-owned transform exists, the target has no
single non-generic nested `Input`.

**Why:** The target owns the semantic construction contract.

**Preferred realization:** Declare `Target.Input`; a direct
`Transformable<Source, Target>` skips this adapter step.

**Alternative and limit:** An external DTO is not this adapter convention.

### TA002 — Input constructor

**Error:** Nested Input has no single public non-empty constructor accepting
exactly one argument of the source type.

**Why:** Adaptation must be mechanical rather than infer missing information.

**Preferred realization:** Define `public record struct Input(Source Value)`.

**Alternative and limit:** Multi-field Inputs require explicit construction by
presentation; the Law cannot infer defaults or field mapping.

### TA003 — Canonical transform

**Error:** The target does not implement `Transformable<Target.Input, Target>`.

**Why:** The semantic target must remain the authority on its own admissibility.

**Preferred realization:** Implement the canonical target-owned transformation.

**Alternative and limit:** Reflection proves the type relation, not correct
validation behavior or the meaning of the input.

## Experimental compiler analyzer (not distributed)

The analyzer remains in `VSlices.Spaces.Analyzers` as source and may be run
in a controlled repository experiment, but it is not a guarantee of the
published package. The experimental analyzer's identifiers are `VST001`,
`VST002`, `VST003`; these are not the Laws' `TA001`–`TA003`.

The experimental diagnostics are:

```text
VST001
    target requires canonical nested Input when no direct transform exists

VST002
    Input constructor is incompatible with the adapter source

VST003
    target does not own Transformable<TO.Input, TO>
```

When explicitly enabled in the source experiment, the analyzer checks both `TransformAdapter<FROM, TO>` usages and closed `vTextInput<TO>` usages.

Generated code analysis is enabled because Razor may materialize closed generic component types in generated C#.

## Runtime boundary

Reflection is a .NET realization mechanism, not the semantic definition of TransformAdapter.

Reflection is performed once per closed `TransformAdapter<FROM, TO>` pair. The result is cached as a strongly typed delegate.

Runtime convention validation remains present because an open generic component such as `vTextInput<T>` cannot always be proven valid when its assembly is compiled. The executable Laws are the consumer-facing convention check for now; the source-only analyzer is retained for future evaluation.

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

## NativeAOT status

`TransformAdapter` currently uses reflection and expression compilation to
construct typed adapters. NativeAOT compatibility is **not established**.
The optional experimental NativeAOT probe can fail without failing its
historical workflow; that result is not a compatibility guarantee.
