# AGENTS.md

## Purpose

This repository contains the current .NET realization of VSlices Framework.

The Framework is being defined and validated through real implementation. Do not treat Domain-Driven Design, functional programming, LanguageExt, Vertical Slice Architecture, or the current .NET API as the universal ontology of VSlices Framework. They may be useful antecedents, techniques, or realization mechanisms.

The current direction is semantic-first and is actively validating distinctions such as:

- `Space`: semantic values, structure, admissibility, and pure transformations;
- `Work`: executable behavior, coordination, WorkFlows, Feature composition, effects, and expected errors;
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

The current validated direction uses `Flow` directly over executable Work algebras.

```text
Feature
    -> Flow<ALG, Request, Response>

ALG
    -> executable algebra owned by a Work module
    -> composed from minimal executable capability atoms

Grounding
    -> implements those atoms
    -> may expose the completed algebra through AlgebraIO<ALG>
```

There is no Free-monad/interpreter layer in the current Work model.

### Feature / WorkFlow

A Feature owns its request/response contract and exposes execution through Flow:

```csharp
Feature<F, ALG, REQ, RES>
{
    static abstract Flow<ALG, REQ, RES> Get();
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

```csharp
public interface AlgebraIO<ALG>
{
    ALG Algebra { get; }
}
```

`AlgebraIO` is an export contract for an executable algebra, not an operation interpreter.

### Feature composition

Features sharing the same module algebra compose directly.

Independent module algebras compose structurally with:

```text
AlgebraSum<A, B>
...
AlgebraSum<A, B, C, D, E, F, G>
```

A child Flow is adapted to the parent runtime by projection:

```csharp
child.MapRuntime(
    (AlgebraSum<FileAlgebra, TodoAlgebra> sum) => sum.A)
```

Request adaptation is independent:

```csharp
child.MapRequest(
    (Parent.Request request) => new Child.Request(...))
```

Use `ContraMap` when both runtime and request should be adapted together.

Ownership rules:

```text
child Work module owns its algebra
child Feature owns its Flow
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

The current executable-algebra / Flow model does not settle the guarantee model.

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

Do not smuggle those guarantees into capability names, WorkPart names, interpreter classes, or Process composition.

---

## Error Handling

Failures expected by the modeled Work must remain explicit.

- do not throw exceptions for expected flows;
- use explicit error values or typed alternatives;
- preserve error information through composed Feature WorkFlows;
- distinguish semantic rejection from interpreter/runtime failure;
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

---

## Architecture

Architecture should emerge from semantic ownership and real coordination boundaries.

Rules:

- keep Features understandable in isolation;
- keep WorkFlow instruction vocabulary local to its owner;
- avoid shared service/program layers that steal WorkFlow ownership;
- keep concrete realization behind explicit Grounding boundaries;
- keep presentation adapters thin;
- let a composing Feature reuse existing WorkFlows without rewriting them;
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

`SampleBFF.AttachFileToTodo` now validates composition across two independently owned WorkFlows and two distinct Groundings. The next observable pressure is partial-failure semantics across those WorkFlows: compensation, retry, reconciliation, or stronger guarantees when a product actually requires them. Ticket Support BFF `AttachFile` / `RemoveAttachment` remains the real-world pressure case after the isolated sample.

---

## What To Avoid

Do NOT:

- introduce null where an explicit alternative belongs;
- throw exceptions for expected control flow;
- inject concrete service implementations into Feature definitions;
- hide WorkParts behind vague helper/manager/service abstractions;
- recreate a shared `TodoPrograms`-style layer that owns the real WorkFlow;
- make a lower WorkFlow know the Feature/BFF that may compose it later;
- treat one concrete interpreter as the semantic owner of several WorkFlows;
- infer transactions or stronger persistence guarantees from point read/write/remove;
- replace the explicit module algebra runtime with an arbitrary dependency carrier without new evidence;
- force every pure semantic transformation into a WorkPart;
- extend `AlgebraSum` beyond the supported A..G arities without pressure;
- use current implementation convenience as proof of universal Framework semantics.

---

## Testing

Testing should preserve the same architectural model.

Current evidence should include, where relevant:

- direct tests of executable capability atoms;
- Feature execution through `Flow<ALG, Request, Response>`;
- composed Feature runtime projection through `AlgebraSum` and `MapRuntime`;
- compile/build evidence for dependent Framework surfaces;
- presentation/API smoke behavior;
- preservation of semantic state transitions.

Prefer fake or in-memory interpreters over mocking vague service layers.

For the current miniature, the branch should remain green for:

```text
VSlices.Work build
VSlices.Work.Services build
VSlices.Work.Products build
Work algebra tests
SampleWorkflow API build
composed Feature hoist/composition tests
AlgebraSum A..G tests
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
