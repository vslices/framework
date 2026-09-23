# Feature Model

Features are the main executable WorkFlow abstraction in VSlices.

The current validated relationship is:

```text
Feature == WorkFlow == Free<ALG, Response>
```

A Feature owns:

- one nominal request type;
- one nominal response type;
- one algebra describing the WorkParts available to that WorkFlow;
- the Free program that composes those WorkParts.

A Feature is not a controller, handler, service class, manager, presentation adapter, runtime carrier, Entity, or Aggregate Root.

## Core shape

The current contract is:

```csharp
public interface Feature<ALG, REQ, RES>
    where ALG : Functor<ALG>
{
    static abstract Free<ALG, RES> Describe(REQ request);
}
```

Conceptually:

```text
ALG
    Work vocabulary available to the Feature

REQ
    request for this execution

RES
    result of this WorkFlow
```

The base Feature contract does not require a self type. A specialization may add one when concrete type identity has an independent semantic role.

## Request and response ownership

A Feature should normally own its request and response nominally:

```csharp
public sealed class CreateSomething :
    Feature<
        ModuleAlgebra,
        CreateSomething.Request,
        CreateSomething.Response>
{
    public sealed record Request(...);
    public sealed record Response(...);

    // ...
}
```

Keep these meanings separate:

```text
semantic input
    data required to establish or transform a semantic value

Feature Request
    data required to execute one WorkFlow

Feature Response
    value explicitly returned by that WorkFlow
```

A Feature may reuse semantic types directly inside its request or response when that preserves the intended meaning.

## Algebra ownership

An algebra is the instruction language available to Work.

The default for a coherent module is one module-owned algebra containing the valid operations of that module. Features describe different programs in that shared language.

A Feature-specific algebra remains valid when a real case benefits from a smaller or distinct vocabulary; it is a specialization, not the default ownership rule.

For example:

```text
TodoAlgebra
    NextTodoId
    ReadTodo
    WriteTodo
    RemoveTodo

GetTodo
    describes a program using ReadTodo

UpdateTodo
    describes a program using ReadTodo + WriteTodo

DeleteTodo
    describes a program using ReadTodo + RemoveTodo
```

The algebra describes available WorkParts. It does not select a concrete execution mechanism.

A pure semantic transformation need not become a WorkPart merely because it occurs during a WorkFlow.

## Free WorkFlow

`Feature.Describe(request)` returns an inert description of Work:

```csharp
Free<ALG, RES>
```

Constructing the Feature program performs no external effect.

Grounding later interprets the program through:

```csharp
AlgebraIO<ALG>
```

This keeps:

```text
what the WorkFlow means
!=
how the external world realizes its WorkParts
```

## Feature composition

A Feature that reuses other Features is still a normal Feature.

Composition is represented by a richer algebra rather than by another executable abstraction.

For example:

```csharp
using Algebra = AlgebraSum<
    AddFile.Algebra,
    TodoAlgebra>;

public sealed class AttachFileToTodo :
    Feature<
        Algebra,
        AttachFileToTodo.Request,
        AttachFileToTodo.Response>
{
    // ...
}
```

Child Feature programs can be embedded through the positional algebra helpers:

```csharp
Algebra.FromA(AddFile.Describe(...))
Algebra.FromB(AddAttachmentReference.Describe(...))
```

The child Feature does not know which later Feature, product, or BFF may reuse it.

## Grounding

Grounding owns concrete realization.

A Feature-specific interpreter implements:

```csharp
AlgebraIO<Feature.Algebra>
```

and may delegate its WorkParts to smaller reusable Grounding contracts.

For example:

```text
ReadTodoPart
    -> PointReaderIO<Todo, TodoId>

WriteTodoPart
    -> PointWriterIO<Todo>

ObserveNowPart
    -> ClockIO

DelayPart
    -> DelayIO
```

A concrete mechanism such as Entity Framework Core or `TimeProvider` belongs below this boundary.

## Service specialization

A service-owned Feature is modeled with:

```csharp
ServiceFeature<F, ALG, REQ, RES>
```

A concrete `ServiceFeature` declares:

```csharp
static abstract string UniqueName { get; }
static abstract string Description { get; }
```

and receives a default `ServiceClaim` derived from that metadata.

`UniqueName` is the stable machine-facing capability identity. `Description` is human/agent-facing metadata and does not define equality.

## Product specialization

A product behavior is modeled with:

```csharp
ProductFeature<F, ALG, REQ, RES>
```

and exposes its product authorization through:

```csharp
static abstract ProductRole ExecutableBy { get; }
```

Service claims and product roles intentionally have different ownership semantics.

## Presentation adapters

Presentation adapters should only:

1. receive external input;
2. refine or translate it into `Feature.Request`;
3. obtain the Feature Free program;
4. interpret it with the selected Grounding;
5. translate `Feature.Response` into the presentation response.

Features must not contain HTTP, UI, worker, or transport-specific concerns merely because one presentation invokes them.

The current SampleWorkflow API performs direct interpretation explicitly:

```text
HTTP
    -> Feature.Describe(request)
    -> Free<ALG, RES>
    -> AlgebraIO<ALG>
    -> response mapping
```

Future invocation bindings may make this wiring reusable without changing Feature semantics.

## Side effects and failures

External effects occur only when a Grounding interprets WorkParts.

Expected domain or application failures should remain explicit values in the Feature response or semantic transformations.

Do not use exceptions for expected control flow.

Interpreter or host failures that are not modeled as expected outcomes may still surface through the effect runtime.

## Retired execution model: Flow

The historical `Flow<RT, REQ, RES>` abstraction was retained temporarily while Feature-as-Free was being validated.

Subsequent evidence showed that:

- Feature already owns the request;
- `ALG` already expresses the Work vocabulary;
- `Free<ALG, RES>` already expresses WorkFlow composition;
- Grounding already owns concrete interpretation;
- current Features, samples, persistence Grounding, and temporal Grounding require no `Flow` consumer.

The executable trajectory is therefore:

```text
Feature + Flow<RT, REQ, RES>
    -> Free point-algebra experiment
    -> Feature owns Free<ALG, RES>
    -> same-service composition
    -> cross-service composition
    -> point and temporal Groundings
    -> no remaining Flow responsibility
    -> Flow removed
```

Do not reintroduce `Flow` merely as a request/runtime wrapper. A future mechanism with genuinely new semantics should be named and justified from that pressure.

## Retired semantic categories: Entity and Aggregate Root

`Entity` and `AggregateRoot` are not current VSlices Framework semantic primitives.

The current model uses smaller independently justified concepts such as:

- semantic spaces;
- explicit identity values where identity is meaningful;
- `Evolvable` when accepted state evolution is meaningful;
- Feature-owned Work vocabulary;
- guarantees or consistency semantics only when independently established.

Do not infer:

```text
has identity
    => Entity

coordinates related changes
    => Aggregate Root
```

Those historical DDD labels bundled additional assumptions that are not justified by identity or state evolution alone.

If future evidence requires a consistency boundary, authority boundary, lifecycle concept, or identity-continuity rule, model that concept directly rather than restoring `Entity` or `AggregateRoot`.

## Testing

Test a Feature by:

1. constructing its `Free<ALG, RES>` program;
2. verifying that construction is inert where relevant;
3. interpreting it through a suitable `AlgebraIO<ALG>`;
4. asserting semantic results and meaningful observable effects.

Use alternate Groundings when useful to verify that Work semantics do not depend on one realization.

## Rules

A Feature must:

- own or explicitly bind its request and response contracts;
- use an algebra containing only the WorkParts it actually requires;
- return `Free<ALG, RES>`;
- remain presentation-independent;
- keep semantic transformation and external acquisition/realization distinguishable;
- make composition explicit through its algebra.

A Feature must not require an ambient runtime carrier merely to access dependencies.

A ServiceFeature must additionally:

- expose a globally intended `UniqueName`;
- expose a useful `Description`;
- use the default derived `Claim` unless an exceptional override is semantically required.
