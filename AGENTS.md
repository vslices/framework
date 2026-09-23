# AGENTS.md

## Purpose

This repository contains the current .NET realization of VSlices Framework.

The Framework is being defined and validated through real implementation. Do not treat Domain-Driven Design, functional programming, LanguageExt, Vertical Slice Architecture, or the current .NET API as the universal ontology of VSlices Framework. They may be useful antecedents, techniques, or realization mechanisms.

The current direction is semantic-first and is actively validating distinctions such as:

- `Space`: semantic values, structure, admissibility, and pure transformations;
- `Work`: executable behavior, coordination, WorkFlows, WorkProcesses, effects, and expected errors;
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
Feature == WorkFlow == Free<WorkFlowAlgebra, Response>

1 WorkProcess : M WorkFlow
1 WorkFlow    : Q WorkPart
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

## WorkProcess Composition

A WorkProcess composes already-existing WorkFlows.

The current validated miniature uses a binary algebra sum:

```text
WorkFlow A algebra --\
                     +--> AlgebraSum<A,B>
WorkFlow B algebra --/
```

Each child WorkFlow is hoisted into the Process algebra through an external natural-transformation witness:

```text
InjectLeft<A,B>  : A ~> AlgebraSum<A,B>
InjectRight<A,B> : B ~> AlgebraSum<A,B>
```

and:

```csharp
FreeAlgebra.hoist<N, F, G, A>(Free<F, A>)
```

Ownership rules:

```text
WorkFlow owns its WorkParts.
WorkProcess owns composition of WorkFlows.
Grounding owns realization.
A composing Process owns injection into its larger algebra.
A lower WorkFlow does not know its future Process or BFF.
```

The Process interpreter must delegate each branch to the interpreter that already owns it. It must not reimplement child WorkParts.

Do not introduce arbitrary-N algebra machinery until real cases require it. Binary composition can be nested while the model is still under pressure.

---

## Flow Status

`Flow<RT, REQ, RES>` still exists in the repository, but its final relationship to the current Free WorkFlow model is unresolved.

Do not:

- restore `Flow` as the Feature boundary merely because older documentation says so;
- delete or redesign `Flow` just to simplify the current experiment;
- claim that its final role is settled.

First preserve the validated Feature-as-Free and WorkProcess composition model. Let real cases determine whether `Flow` remains an execution carrier, presentation/runtime syntax, another abstraction, or is superseded on this path.

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
- do not infer Repository, Store, Unit of Work, tracking, transactions, atomicity, or durability from read/write/remove capability alone.

Read/write vocabulary describes instructions. Stronger guarantees are separate semantics.

---

## Guarantees

The current Free WorkFlow / WorkProcess experiment does not settle the guarantee model.

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
- preserve error information through WorkFlow and WorkProcess composition;
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
- let a WorkProcess coordinate existing WorkFlows without rewriting them;
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
- make a lower WorkFlow know the Process/BFF that may compose it later;
- treat one concrete interpreter as the semantic owner of several WorkFlows;
- infer transactions or stronger persistence guarantees from point read/write/remove;
- restore historical `RT` constraints at the Feature boundary without new evidence;
- force every pure semantic transformation into a WorkPart;
- generalize `AlgebraSum` to arbitrary-N machinery without pressure;
- use current implementation convenience as proof of universal Framework semantics.

---

## Testing

Testing should preserve the same architectural model.

Current evidence should include, where relevant:

- direct tests of generic algebra/hoist mechanisms;
- Feature WorkFlow interpretation through `AlgebraIO<Feature.Algebra>`;
- WorkProcess composition and interpreter delegation;
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
WorkProcess hoist/composition tests
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
