# AGENTS.md

## Purpose

This repository contains the current .NET realization of VSlices Framework.

The Framework is being defined and validated through real implementation. Do not treat Domain-Driven Design, functional programming, LanguageExt, Vertical Slice Architecture, or the current .NET API as the universal ontology of VSlices Framework. They may be useful antecedents, techniques, or realization mechanisms.

The current direction is semantic-first and is actively validating distinctions such as:

- `Space`: semantic values, structure, admissibility, and pure transformations;
- `Work`: executable behavior, coordination, Feature composition, effects, and expected errors;
- `Grounding`: acquisition or realization of facts and effects that are not derivable from pure semantics alone.

These names and boundaries remain provisional. Prefer current repository evidence over remembered architecture.

The main engineering goals remain:

- simplicity;
- explicit semantics and ownership;
- composability;
- meaningful compile-time guarantees where the target can provide them;
- low accidental complexity;
- and continuity between semantic intent, executable realization, and evidence.

---

## Core Philosophy

Preserve these distinctions:

```text
semantic meaning
!= execution mechanism
!= concrete realization
!= authorization
!= guarantee
```

General principles:

- no null values in semantic APIs when an explicit alternative is available;
- no exceptions for expected control flow;
- prefer composition over inheritance;
- prefer explicit dependencies and effects over hidden dependencies;
- prefer pure transformations where the required evidence is already available;
- make effectful acquisition and realization visible;
- introduce stronger structure only when evidence justifies it;
- do not infer semantics from convenient implementation mechanisms;
- keep abstractions honest and minimal.

A recurring factorization is:

```text
effectful acquisition -> explicit evidence -> pure transformation
```

Do not force this factorization when current evidence contradicts it, but do not hide acquisition inside a supposedly pure semantic transformation.

---

## Current Work Model

The current Feature contract uses `Eff<ALG, RES> Get(REQ request)` over executable Work algebras. `Flow` remains available as an independent core abstraction, but it is not the current Feature execution contract.

```text
Feature
    -> Eff<ALG, Response> Get(Request)

ALG
    -> executable algebra owned by a Work module
    -> composed from minimal executable capability atoms

Grounding
    -> implements those atoms
    -> assembles and may expose the completed algebra
```

There is no Free-monad/interpreter layer in the current Work model.

### Feature

A Feature owns public nested Request/Response contracts and exposes execution through Eff; Request is publicly constructible, while Response is privately constructed through the owning Feature's `NewResponse` accessor:

```csharp
Feature<F, ALG, REQ, RES>
{
    static abstract Eff<ALG, RES> Get(REQ request);
}
```

For capability-backed Work, the runtime parameter is the module algebra itself.

### Executable algebra

A `[concept].Work` module should expose a small algebra composed from the executable atoms supported by current evidence.

Example:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

Do not create a broad dependency bag. The algebra represents the executable vocabulary owned by that Work module.

### Grounding

Grounding implements capability atoms and assembles the algebra.

A concrete Grounding may expose the assembled algebra through an ordinary property such as:

```csharp
public TodoAlgebra Algebra { get; }
```

Do not introduce a generic "algebra provider" trait merely because several Groundings expose such a property. The previous `AlgebraIO<ALG>` contract added no executable capability or semantic guarantee and has been retired. If later generic composition or provisioning requires a contract, rediscover the smallest truthful abstraction from that pressure.

### Feature composition

Features sharing the same module algebra compose directly.

Independent module algebras compose structurally with:

```text
AlgebraMix<A, B>
...
AlgebraMix<A, B, C, D, E, F, G>
```

`AlgebraMix` is intentionally modest vocabulary: it means that independently owned executable algebras are available together to a composing Feature's effect. It does not claim categorical product/coproduct semantics, ordering, priority, transactionality, or shared ownership.

A parent constructs explicit child requests and projects the required child algebra at execution:

```csharp
Child.Get(new Child.Request(...)).RunIO(mix.A);
```

Do not restore `MapRuntime`, `MapRequest` or `ContraMap` solely for Feature composition. `Flow` remains an independent Framework abstraction.

Ownership rules:

```text
child Work module owns its algebra
child Feature owns its Eff factory and nominal Request/Response
parent Feature owns composition/projection
Grounding owns realization
```

### Point capabilities

The currently validated point atoms are:

```csharp
PointReader<POINT, ID>
PointWriter<POINT>
PointRemover<POINT, ID>
```

They execute in `IO` and can be implemented directly by Grounding.

Do not infer Repository, Store, Unit of Work, tracking, transaction, atomicity or durability semantics from these atoms alone.

`EntityFrameworkPointIO` is a Grounding realization of the point atoms.

### Temporal capabilities

`ClockIO` and `DelayIO` are executable temporal atoms and may be composed into a temporal Work algebra.

Observation, delay, scheduling and invocation remain distinct concerns. Do not infer scheduler or recurrence semantics from clock/delay capabilities.

---

## Guarantees

The current executable-algebra / Eff-based Feature model does not settle the guarantee model.

Still treat the following as separate, unresolved work unless current repository evidence says otherwise:

- tracking;
- atomicity;
- isolation;
- durability;
- staged vs autonomous persistence;
- transaction boundaries;
- law registration;
- proof vocabulary;
- analyzers;
- guarantee-oriented VSIR.

Do not smuggle those guarantees into capability names, Feature names, Grounding implementations, or Process composition. Historical WorkPart/interpreter designs do not establish guarantees for the current model.

---

## Error Handling

Failures expected by the modeled Work must remain explicit.

- do not throw exceptions for expected flows;
- use `Error.New` for expected effect failures where useful, or explicit typed alternatives for modeled absence/rejection; preserve the established required-presence contract of `PointReader.Read` instead of adding catchable error types without consumer pressure;
- preserve error information through composed Feature effects (use `Error.New` for expected failures; reserve .NET exceptions for exceptional conditions);
- distinguish semantic rejection from effect execution or Grounding failure;
- do not collapse missing authority or unsupported semantics into a plausible default.

---

## Semantic Modeling

Prefer explicit semantic spaces and transformations where they remove ambiguity or invalid states.

Current examples include:

- `TodoId` as a semantic discrete space established from `Guid`;
- `TodoDetail` as the textual semantic value;
- `Todo` as an evolvable semantic point;
- `Todo.Update(...)` as accepted state evolution rather than unrelated replacement construction.

Do not create a semantic type merely because a primitive exists. `Completed` remains a plain `bool` in the current experiment because no evidence yet requires a separate space.

### Presentation transform adaptation

Presentation convenience must not redefine semantic transformation ownership.

For `TransformAdapter<FROM, TO>`:

- if `TO` already owns `Transformable<FROM, TO>`, use that transformation directly;
- otherwise the adapter may mechanically establish `TO.Input` from `FROM` only when the validated single-input convention holds;
- `TO.Input -> TO` remains owned by `TO.Transformation`;
- do not infer missing values for multi-field Inputs;
- do not make a semantic target implement a presentation-specific direct transform merely to satisfy a UI control;
- for published consumers, explicitly invoke `TransformAdapterModelingLaws` on closed source/target pairs in tests; the experimental analyzer remains source-only and is not distributed with NuGet. Do not claim compilation diagnostics or automatic discovery for consumers.

Reflection is a .NET realization detail and is cached per closed adapter pair. It is not semantic authority.

---

## Architecture

Architecture should emerge from semantic ownership and real coordination boundaries.

Rules:

- keep Features understandable in isolation;
- keep each Feature's executable vocabulary and nominal Request/Response contracts under their proper Work ownership;
- avoid shared service/program layers that steal Feature ownership;
- keep concrete realization behind explicit Grounding boundaries;
- keep presentation adapters thin;
- let a composing Feature reuse existing child Features without rewriting their behavior;
- do not assign semantic policy to persistence or transport components merely because they can execute an operation.

Vertical slices, DDD patterns, functional programming, and other established approaches may be used where they fit; do not treat them as mandatory top-level taxonomy.

---

## Modification Guidelines

When making changes:

- inspect current branch HEAD and nearby evidence first;
- keep changes small and verifiable;
- preserve the current ownership model unless evidence contradicts it;
- prefer extending an existing mechanism over inventing a parallel one;
- do not generalize from one unsupported case;
- do not introduce abstractions only to prepare for hypothetical futures;
- preserve human-editable and documented semantics when refactoring;
- if implementation contradicts documentation, update the nearest authoritative documentation or record the discrepancy.

For new Work cases, prefer:

```text
real case
    -> first observable unsupported boundary
    -> identify who owns that boundary
    -> smallest coherent change
    -> rerun
    -> observe the next boundary
```

`SampleBFF.AttachFileToTodo` validates composition across two independently owned Work modules and two distinct Groundings. The next observable pressure is partial-failure semantics across those effects: compensation, retry, reconciliation, or stronger guarantees when a product actually requires them. Ticket Support BFF `AttachFile` / `RemoveAttachment` remains the real-world pressure case after the isolated sample.

---

## What To Avoid

Do NOT:

- introduce null where an explicit alternative belongs;
- throw exceptions for expected control flow;
- inject concrete service implementations into Feature definitions;
- hide executable capability atoms or Feature ownership behind vague helper/manager/service abstractions;
- recreate a shared `TodoPrograms`-style layer that takes ownership from the Feature;
- make a child Feature know the parent Feature/BFF that may compose it later;
- treat one concrete Grounding adapter as the semantic owner of several Features;
- infer transactions or stronger persistence guarantees from point read/write/remove;
- replace the explicit module algebra runtime with an arbitrary dependency carrier without new evidence;
- force every pure semantic transformation into an effectful Work capability;
- extend `AlgebraMix` beyond the supported A..G arities without pressure;
- use current implementation convenience as proof of universal Framework semantics.

---

## Testing

Testing should preserve the same architectural model.

Current evidence should include, where relevant:

- direct tests of executable capability atoms;
- Feature execution through `Eff<ALG, Response> Get(Request)`;
- composed Feature runtime projection through `AlgebraMix` and `RunIO`;
- compile/build evidence for dependent Framework surfaces;
- presentation/API smoke behavior;
- preservation of semantic state transitions.

Prefer fake or in-memory Grounding implementations of capability atoms over mocking vague service layers.

For the current miniature, the branch should remain green for:

```text
VSlices.Work build
Work algebra tests
SampleWorkflow API build
composed Feature execution/composition tests
AlgebraMix A..G tests
SampleFileRepo build
cross-service SampleBFF composition tests
CRUD smoke test
```

Do not treat green execution alone as proof that the semantic decomposition is correct. Readability and ownership remain part of the experiment.

---

## Style Expectations

When generating or modifying code:

- prefer small focused files;
- prefer intention-revealing names;
- prefer expressions over deeply nested statements;
- avoid hidden control flow;
- avoid ambient mutation;
- keep semantic ownership visible;
- keep execution paths readable;
- prefer the least-powerful sufficient mechanism.

If a simpler honest solution exists, prefer it.

---

## Decision Heuristics

When in doubt, prefer:

- semantic ownership over namespace habit;
- explicit over implicit;
- typed over ad-hoc;
- composable over centralized;
- local reasoning over framework magic;
- evidence over remembered architecture;
- meaningful guarantees over decorative types;
- minimal viable abstraction over speculative architecture.

The implementation is evidence about the design. If a real case contradicts this file, preserve the contradiction and revise the model rather than forcing the code to satisfy stale instructions.


## Structural modeling Laws

When defining or evolving semantic Spaces, consult [Space modeling rules](docs/spaces/modeling.md). The stable SM001–SM007 identifiers explain errors, rationale, and rejected/limited alternatives. Consumers can call `SpaceModelingLaws.InspectDefinitions` from their test suite; the check is structural, not a semantic proof or source-layout analyzer.

For a closed `TransformAdapter<FROM, TO>` pair, run `TransformAdapterModelingLaws.Inspect(typeof(FROM), typeof(TO))` in a consumer test, or `InspectTextInput(typeof(TO))` for `vTextInput<TO>`. The analyzer stays source-only and is not distributed in NuGet. See [TransformAdapter modeling](docs/transform-adapter.md). NativeAOT compatibility is not yet established.

When defining or evolving Work Features, consult [Feature modeling rules](docs/work/feature-modeling.md) and [the current Feature contract](docs/feature-model.md). `FeatureModelingLaws.InspectDefinitions` checks the compiled, selected Feature definitions. The current active IDs are FM001–FM006 and FM008–FM010; FM007 is intentionally retired because the C# interface already enforces `Get(Request) -> Eff<ALG, Response>` at compile time. The reflection Laws do not prove correct outcomes, completed effects, authorization, or cancellation.
