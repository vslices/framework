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

The current `SampleWorkflow` and `SampleBFF` experiments have executable evidence for the following model:

```text
Feature == WorkFlow == Free<ALG, Response>

1 Feature / WorkFlow : Q WorkPart

A Feature may reuse another Feature's WorkFlow by hoisting the child algebra
into a larger composed ALG. Composition changes ALG; it does not require a
second executable abstraction.
```

`WorkLine` remains outside the current implemented scope.

### Feature / WorkFlow

A Feature directly owns the semantic program for one WorkFlow:

```csharp
Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
```

The important ownership rule is:

```text
Feature does not merely execute a WorkFlow.
Feature is the WorkFlow.
```

A Feature must not delegate its real program to a parallel shared `*Programs` layer merely to keep the Feature thin.

### WorkPart

A WorkPart is currently understood as a specific instruction in a WorkFlow.

Do not infer that every pure semantic calculation must become a WorkPart. Pure transformations such as accepted state evolution may remain inside Free continuations unless real pressure shows that they need independent inspectability or execution semantics.

### Feature-owned algebra

Each WorkFlow owns only the instruction vocabulary it actually requires.

For example:

```text
CreateTodo.Algebra
    NextId
    ReadTodo
    WriteTodo

GetTodo.Algebra
    ReadTodo

UpdateTodo.Algebra
    ReadTodo
    WriteTodo

DeleteTodo.Algebra
    ReadTodo
    RemoveTodo
```

Generic vocabulary such as `PointReader`, `PointWriter`, and `PointRemover` may be reused to construct those algebras.

Do not replace the per-Feature algebra with one broad shared service algebra merely because the same realization can interpret all operations.

---

## Grounding and Interpretation

`Grounding` owns concrete realization of WorkParts.

The current interpreter boundary is:

```csharp
AlgebraIO<ALG>
```

A single concrete service may implement several Feature algebras:

```text
InMemoryTodoWork
    AlgebraIO<CreateTodo.Algebra>
    AlgebraIO<GetTodo.Algebra>
    AlgebraIO<UpdateTodo.Algebra>
    AlgebraIO<DeleteTodo.Algebra>
```

This does not make those algebras the same. It means one realization can interpret several independently owned WorkFlow vocabularies.

`HasAlgebra<ALG, RT>` and runtime carriers may remain useful execution mechanisms in contexts that need them, but they are not the current semantic boundary of `Feature`.

Do not reintroduce `RT` into `Feature<F, ALG, REQ, RES>` merely because historical APIs or runtime helpers still exist.

---

## Feature Composition

A Feature can reuse already-existing WorkFlows by composing their algebras.

The same `Feature<F, ALG, REQ, RES>` contract is used whether `ALG` is a Feature-owned algebra or a sum of several child algebras.

For example:

```text
CreateAndGetTodo
    ALG = AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>

AttachFileToTodo
    ALG = AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>
```

`AlgebraSum` is an execution-language composition mechanism, not a separate category of Feature.

The public family currently follows LanguageExt-style positional arities:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
AlgebraSum<A, B, C, D>
AlgebraSum<A, B, C, D, E>
AlgebraSum<A, B, C, D, E, F>
AlgebraSum<A, B, C, D, E, F, G>
```

Each sum exposes `FromA` through `FromG` as applicable. These helpers hoist an existing child Feature program into the larger algebra without exposing recursive `Left/Right` structure at the call site.

The underlying natural transformations remain explicit as `InjectA` through `InjectG`. Positional A..G vocabulary is the only supported injection surface.

Ownership rules:

```text
Feature / WorkFlow owns its WorkParts.
The composing Feature owns its composed ALG and child-program injection.
Grounding owns realization.
A child WorkFlow does not know which future Feature or BFF may reuse it.
```

`AlgebraSumIO<A,...,G>` composes existing interpreters and delegates each operation to the interpreter that owns that child algebra. It must not reimplement child WorkParts.

Do not introduce arities beyond seven or a different composition mechanism until real pressure requires it.

---

## Flow Status

`Flow<RT, REQ, RES>` still exists in the repository, but its final relationship to the current Free WorkFlow model is unresolved.

Do not:

- restore `Flow` as the Feature boundary merely because older documentation says so;
- delete or redesign `Flow` just to simplify the current experiment;
- claim that its final role is settled.

First preserve the validated Feature-as-Free and composed-algebra model. Let real cases determine whether `Flow` remains an execution carrier, presentation/runtime syntax, another abstraction, or is superseded on this path.

---

## Point Capabilities

When Work needs operations over points of semantic spaces:

- prefer small structural capabilities over importing historical infrastructure abstractions;
- use `PointReader<ALG, POINT, ID>` for point reading where it fits;
- use `PointWriter<ALG, POINT>` for point writing where it fits;
- use `PointRemover<ALG, POINT, ID>` for point removal where it fits;
- compose only the capabilities required by the owning Feature algebra;
- build the WorkFlow as `Free<ALG, A>`;
- let Grounding provide `AlgebraIO<ALG>`;
- when a reusable realization is useful, depend on the minimum grounding contract such as `PointReaderIO<POINT, ID>`, `PointWriterIO<POINT>`, or `PointRemoverIO<POINT, ID>`;
- treat `EntityFrameworkPointIO` as an EF Core realization of those point capabilities, not as a semantic persistence boundary;
- do not reintroduce `Repository`, `DatabaseIO`, Store, Unit of Work, tracking, transactions, atomicity, or durability from read/write/remove capability alone.

`Repository` and `DatabaseIO` were removed after executable EF Core evidence showed that point capabilities were sufficient for the validated cases.

Read/write/remove vocabulary describes instructions. Stronger guarantees are separate semantics.

---

## Guarantees

The current Free WorkFlow / composed-Feature experiment does not settle the guarantee model.

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
- keep concrete realization behind explicit interpreter boundaries;
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
- restore historical `RT` constraints at the Feature boundary without new evidence;
- force every pure semantic transformation into a WorkPart;
- extend `AlgebraSum` beyond the supported A..G arities without pressure;
- use current implementation convenience as proof of universal Framework semantics.

---

## Testing

Testing should preserve the same architectural model.

Current evidence should include, where relevant:

- direct tests of generic algebra/hoist mechanisms;
- Feature WorkFlow interpretation through `AlgebraIO<Feature.Algebra>`;
- composed Feature algebra hoisting and interpreter delegation;
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
