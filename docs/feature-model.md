# Feature Model

Features are the main executable unit of Work in VSlices.

The current model is:

```text
Feature == WorkFlow == Free<ALG, Response>
```

A Feature owns:

- one nominal request type;
- one nominal response type;
- one algebra describing the WorkParts available to that WorkFlow;
- the program that composes those WorkParts.

A Feature is not a controller, handler, service class, manager, presentation adapter, runtime carrier, or dependency-injection boundary.

## Core shape

The canonical shape is:

```csharp
public sealed class SomeFeature :
    Feature<
        SomeFeature,
        SomeFeature.Algebra,
        SomeFeature.Request,
        SomeFeature.Response>
{
    public sealed record Request(...);
    public sealed record Response(...);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed class Algebra :
        Functor<Algebra>
    {
        // Feature-owned WorkPart constructors + Functor.Map
    }

    public static Free<Algebra, Response> Get(Request request) =>
        ...;
}
```

Conceptually:

```text
Feature
├── Request
├── Response
├── Algebra
│   └── available WorkPart vocabulary
└── Get(request)
    └── Free<Algebra, Response>
```

Constructing the Feature WorkFlow performs no external effect.

External effects occur only when a Grounding interprets the resulting `Free` program.

## Request and algebra are different concerns

The request describes the input for one execution.

The algebra describes the operations the WorkFlow is allowed to express.

```text
REQ
    execution input

ALG
    Work instruction vocabulary
```

Do not encode request data as ambient runtime state.

Do not encode infrastructure mechanisms into the Feature algebra unless the mechanism itself is genuinely part of the Work semantics.

## Feature-owned WorkParts

A Feature should own only the WorkParts it actually requires.

For example:

```text
GetTodo.Algebra
    ReadTodo

UpdateTodo.Algebra
    ReadTodo
    WriteTodo

DeleteTodo.Algebra
    ReadTodo
    RemoveTodo
```

Generic capability vocabulary can help define those WorkParts:

```text
PointReader
PointWriter
PointRemover
Clock
Delay
```

The Feature-owned algebra remains the WorkFlow vocabulary even when several Features reuse the same generic capability concepts.

## Grounding and execution

A Feature does not carry a runtime type parameter.

Execution requires an interpreter:

```csharp
AlgebraIO<Feature.Algebra>
```

A caller can interpret the WorkFlow through:

```csharp
var program = SomeFeature.Get(request);

var response = await FreeAlgebra
    .interpret(program, interpreter)
    .RunAsync();
```

This keeps three concerns separate:

```text
Feature
    owns Work semantics

AlgebraIO
    maps Feature WorkParts into concrete effects

Grounding
    owns concrete realization
```

A presentation or host may select and compose interpreters, but that selection does not become part of the Feature type.

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

and receives a default claim through:

```csharp
static virtual ServiceClaim Claim
```

The claim metadata belongs to service authorization semantics.

It does not alter the WorkFlow execution model.

## ServiceClaim

`ServiceClaim` is a sealed value rather than a subtype hierarchy.

Its identity is:

```text
UniqueName
```

Its descriptive metadata is:

```text
Description
```

`Description` does not participate in equality or hashing.

Claims are created through:

```csharp
ServiceClaim.New<OWNER>(uniqueName, description)
```

The runtime registry remains a local collision-safety mechanism. Global uniqueness across independently running services is a tooling/build concern rather than a reason to introduce a Feature runtime carrier.

## Product specialization

A product behavior is modeled with:

```csharp
ProductFeature<F, ALG, REQ, RES>
```

and exposes product authorization through:

```csharp
static abstract ProductRole ExecutableBy { get; }
```

Service claims and product roles intentionally have different ownership semantics.

## Feature composition

A Feature may reuse already-existing Feature WorkFlows.

The child programs remain independently defined:

```text
Feature A
    -> Free<A.Algebra, X>

Feature B
    -> Free<B.Algebra, Y>
```

The composing Feature owns the larger vocabulary:

```text
AlgebraSum<A.Algebra, B.Algebra>
```

and hoists child programs into that vocabulary.

The composing Feature remains an ordinary Feature.

No `WorkProcess`, `ComposedFeature`, runtime service bag, or alternate execution contract is required.

## Pure transformations inside Work

Not every operation inside a Feature is a WorkPart.

If the required evidence is already available, a pure semantic transformation may remain directly inside a `Free` continuation.

For example:

```text
read Todo
-> pure Todo.Update(...)
-> write accepted Todo
```

Do not turn pure transformations into WorkParts solely to make every line look effectful.

## Presentation adapters

Presentation adapters should normally:

1. receive external input;
2. transform or refine it into `Feature.Request`;
3. select the appropriate Grounding/interpreter;
4. interpret the Feature WorkFlow;
5. translate `Feature.Response` into the presentation response.

Features should not contain HTTP, UI, worker, transport, or host-lifecycle concerns merely because those mechanisms invoke the Feature.

## Effects and failures

Building a Feature produces an inert `Free` program.

Grounding interpretation produces effects.

Expected semantic or application failures should remain explicit values in the WorkFlow result rather than exceptions used for normal control flow.

Interpreter or infrastructure failure remains a distinct concern from an expected Feature outcome.

## Testing

Test Features by constructing the same `Free<ALG, RES>` program used in production and interpreting it with a suitable `AlgebraIO<ALG>`.

Useful test Groundings include:

- pure/fake interpreters;
- in-memory interpreters;
- real infrastructure Groundings when the claim depends on them.

The choice of Grounding should follow the evidence required by the claim rather than a universal preference for mocks or full topology.

## Rules

A Feature must:

- own nominal `Request` and `Response` types;
- bind those types through `Feature<F, ALG, REQ, RES>`;
- own or explicitly compose its `ALG`;
- return `Free<ALG, RES>` from `Get(request)`;
- keep WorkFlow ownership local and readable;
- remain presentation-independent;
- avoid ambient runtime capability lookup.

A ServiceFeature must additionally:

- expose a stable `UniqueName`;
- expose useful `Description` metadata;
- use the default derived Claim unless an exceptional override is semantically required.

## Historical trajectory

The previous execution model used:

```text
Feature<F, RT, REQ, RES>
    -> Flow<RT, REQ, RES>
```

That model helped expose request/runtime separation and explicit effect requirements, but later implementation pressure produced a smaller representation:

```text
Feature<F, ALG, REQ, RES>
    -> Free<ALG, RES>
```

The Feature algebra now carries the explicit Work vocabulary, while Grounding/interpreter selection occurs outside the Feature contract.

`Flow`, `HasAlgebra<ALG, RT>`, and `AlgebraEnv<ALG, RT>` are therefore no longer part of the current Feature model.
