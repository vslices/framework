# Feature Model

Features are the main executable unit of Work in VSlices.

A Feature owns:

- one nominal request type;
- one nominal response type;
- one executable runtime algebra;
- one `Flow` that relates those three surfaces.

## Core shape

```csharp
public sealed class SomeFeature :
    Feature<
        SomeFeature,
        SomeAlgebra,
        SomeFeature.Request,
        SomeFeature.Response>
{
    public sealed record Request(...);
    public sealed record Response(...);

    public static Flow<SomeAlgebra, Request, Response> Get() => ...;
}
```

Conceptually:

```text
Feature
├── Request
├── Response
└── Flow<Algebra, Request, Response>
```

The runtime channel is not an arbitrary dependency carrier. For capability-backed Work it is the executable algebra exposed by the owning Work module.

## Runtime and request are distinct channels

```text
ALG = executable operations available to Work
REQ = input for this execution
RES = result of this execution
```

They compose independently.

A child Feature can be reused under a larger runtime:

```csharp
child.MapRuntime(
    (AlgebraSum<A, B> sum) => sum.A)
```

and under a different parent request:

```csharp
child.MapRequest(
    (Parent.Request request) =>
        new Child.Request(...))
```

`ContraMap` adapts both channels together when that is clearer.

Result transformation remains ordinary `Map`.

## Module algebra

A `[concept].Work` module should define the algebra used by its Features.

Prefer a small record composed from executable capability atoms:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

Do not create one algebra per Feature merely because Features consume different subsets. The module algebra represents the executable vocabulary owned by that Work module; individual Features simply use the operations they need.

Do not inflate the algebra with hypothetical capabilities.

## Grounding

A `[concept].Grounding` realization implements the algebraic atoms and assembles the module algebra.

```text
Work owns:
    TodoAlgebra
    PointReader/Writer/Remover requirements

Grounding owns:
    how those requirements contact the world
```

`AlgebraIO<ALG>` is the optional Grounding-facing contract for exporting the completed executable algebra.

## Feature composition

### Same module

Features sharing the same algebra compose directly as Flows. Their requests can be adapted with `MapRequest`.

### Independent modules

A Feature that coordinates independently owned Work can use:

```text
AlgebraSum<A, B, ...>
```

and project each child algebra with `MapRuntime`.

This preserves child ownership:

```text
child Feature
    knows only its own algebra

parent Feature
    owns the composition and projection

Grounding
    realizes the child atoms
```

## Presentation

Presentation adapters should:

1. receive external input;
2. transform/refine it into the Feature request;
3. obtain the required executable algebra from Grounding;
4. run the Feature Flow;
5. translate the Feature response outward.

HTTP, UI, workers and transports do not define Feature semantics.

## Failures

Expected failures remain explicit values or effect failures.

Transport-specific status codes do not belong to the base Feature error model. Presentation decides how semantic/application outcomes map to HTTP or another transport.

## Current rules

A Feature should:

- own nominal Request and Response types;
- expose `Flow<ALG, Request, Response>`;
- use an executable Work algebra as its capability-backed runtime;
- remain presentation-independent;
- compose child Features rather than reimplementing them;
- adapt runtime, request and result through explicit mappings;
- avoid Free-monad/interpreter layers unless future evidence independently justifies a different mechanism.

The implementation remains evidence. These rules are current and revisable.
