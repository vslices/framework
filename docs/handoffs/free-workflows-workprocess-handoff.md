# Handoff — Free WorkFlows and composed WorkProcess algebras

> Date: 2026-09-23
>
> Branch: `experiment/simple-workflow-point-crud`
>
> Validated miniature baseline: `6c951a2bcca5fef28c1a6a246ae9df6328df981d`
>
> Documentation alignment: `2812b57877214e6acb2894327bc278e9c78b9237`
>
> Status: the miniature Free WorkFlow / WorkProcess composition is implemented and green. The next step is real-world pressure against Ticket Support BFF composition.

## Purpose

This handoff preserves the validated result of the `SampleWorkflow` experiment and the next concrete continuation path.

The experiment now has executable evidence for:

```text
Feature == WorkFlow == Free<WorkFlowAlgebra, Response>
```

where each WorkFlow algebra contains only the WorkParts required by that Feature.

It also has executable evidence for:

```text
WorkProcess
    composes existing WorkFlows
    by composing their algebras
    and hoisting each WorkFlow into the composed algebra
```

The miniature has done its job. Do not repeat it before moving to a real composition boundary.

## Validated baseline

The current sample contains:

```text
SampleWorkflow.Spaces
SampleWorkflow.Work
SampleWorkflow.Grounding
SampleWorkflow.Process
SampleWorkflow.Process.Tests
SampleWorkflow.Api
```

The branch CI verifies:

```text
VSlices.Work build
VSlices.Work.Services build
VSlices.Work.Products build
Work algebra tests
SampleWorkflow API build
WorkProcess hoist/composition tests
CRUD smoke test
```

The current branch run at the documentation handoff is green.

## Semantic model retained by the sample

### TodoId

`TodoId` is a semantic discrete space established from a `Guid`:

```text
Guid
    -> TodoId.Transformation
    -> TodoId
```

Identity generation is a realization concern distinct from the semantic transformation that establishes a valid `TodoId`.

### TodoDetail

`TodoDetail` covers only the textual value:

```text
string
    -> TodoDetail.Transformation
    -> TodoDetail
```

`Completed` remains a plain `bool`; the current experiment has not produced evidence requiring a separate semantic space.

### Todo

`Todo` is an evolvable semantic point established from:

```text
TodoId
TodoDetail
bool Completed
```

and implements:

```text
Evolvable<Todo, Todo.State>
```

The PUT path remains:

```text
read current Todo
    -> Todo.Update(...)
    -> accepted evolved Todo
    -> write point
```

rather than constructing an unrelated replacement from HTTP input.

## Validated Work hierarchy

The current conceptual hierarchy remains:

```text
1 WorkLine    : N WorkProcess
1 WorkProcess : M WorkFlow
1 WorkFlow    : Q WorkPart
```

For the implemented experiment:

```text
WorkPart
    specific instruction in a WorkFlow

WorkFlow
    Feature
    Free Monad over its WorkPart algebra

WorkProcess
    composition of multiple existing WorkFlows

WorkLine
    still outside current implementation scope
```

Do not generalize WorkLine yet.

## Feature is the WorkFlow

The current Framework surface is already:

```csharp
Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
```

The accepted experimental interpretation is:

```text
Feature does not merely execute a WorkFlow.
Feature is the WorkFlow.
```

Runtime and concrete realization are not required to define the semantic program.

The previous parallel `TodoPrograms` ownership has been removed.

## WorkFlow algebras

Each Feature / WorkFlow owns the algebra required to express its WorkParts.

The sample currently demonstrates:

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

Generic vocabulary such as:

```text
PointReader
PointWriter
PointRemover
```

can still be reused without moving WorkFlow ownership into a shared service algebra.

The exact generated/handwritten split remains open. ADT cases and `Functor.Map` remain plausible Tooling-generation candidates, but that is not part of this experiment.

## Grounding

A single service implementation may satisfy several WorkFlow algebras.

The sample currently has:

```csharp
InMemoryTodoWork :
    AlgebraIO<CreateTodo.Algebra>,
    AlgebraIO<GetTodo.Algebra>,
    AlgebraIO<UpdateTodo.Algebra>,
    AlgebraIO<DeleteTodo.Algebra>
```

This does not make the algebras identical.

It means one concrete realization knows how to interpret the WorkParts required by several independently owned WorkFlows.

Normal service execution can receive:

```text
AlgebraIO<CreateTodo.Algebra>
AlgebraIO<GetTodo.Algebra>
...
```

without exposing the concrete interpreter to the Feature.

## WorkProcess composition

The miniature now implements:

```text
CreateAndGetTodo
    -> CreateTodo
    -> GetTodo
```

using:

```text
AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>
```

A WorkProcess algebra is the composition of the algebras of the WorkFlows it coordinates.

For two WorkFlows:

```text
WorkflowAlgebraA --\
                  +--> ProcessAlgebra
WorkflowAlgebraB --/
```

Binary composition is sufficient for the current evidence and can be nested later if real pressure requires more.

Do not introduce arbitrary-N machinery speculatively.

## Hoisting and ownership

Each WorkFlow is lifted into the Process algebra without changing its behavior:

```text
Free<A, X>
    -> Free<ProcessAlgebra, X>

Free<B, Y>
    -> Free<ProcessAlgebra, Y>
```

VSlices currently uses external natural-transformation witnesses:

```text
InjectLeft<A,B>  : A ~> AlgebraSum<A,B>
InjectRight<A,B> : B ~> AlgebraSum<A,B>
```

with:

```csharp
FreeAlgebra.hoist<N, F, G, A>(Free<F, A>)
    where N : Natural<F, G>
```

The mathematical mechanism is hoisting.

The important ownership decision is that the composing Process owns the injection into the larger algebra. A lower service WorkFlow does not know which future Process or BFF may compose it.

## Process interpreter

The Process does not reimplement child WorkParts.

For the binary sum:

```text
Left operation
    -> left interpreter

Right operation
    -> right interpreter
```

`AlgebraSumIO<L,R>` delegates to the existing interpreters.

This preserves:

```text
WorkFlow owns its WorkParts.
WorkProcess owns composition of WorkFlows.
Grounding owns realization.
A composing Process owns injection into its larger algebra.
A lower WorkFlow does not know its future Process/BFF.
```

## Flow remains unresolved

Do not delete or redesign `Flow` merely because Feature is now a Free WorkFlow.

The relationship between:

```text
Feature / Free WorkFlow
Flow<RT,REQ,RES>
presentation invocation
runtime interpretation
```

remains open.

The miniature proves Feature-as-Free and WorkProcess hoisting. It does not prove the final role of `Flow`.

## Guarantees remain out of scope

The Work composition experiment still does not settle:

```text
Tracking
Atomicity
Isolation
Durability
staged vs autonomous persistence
transaction boundaries
law registration
analyzers
proof vocabulary
guarantee-oriented VSIR
```

Do not smuggle stronger guarantees into WorkPart names, point capabilities, interpreters, or Process composition.

## WorkPart classification remains partially open

The current claim remains:

```text
WorkPart = specific instruction in a WorkFlow
```

Do not infer that every pure semantic calculation must become an explicit WorkPart.

For example:

```csharp
todo.Update(...)
```

currently remains a pure semantic transformation inside a continuation.

Real cases should decide whether some transformations need to become separately inspectable WorkParts.

## Miniature success status

The original success criteria are now materially satisfied:

```text
1. A Feature directly is a Free WorkFlow.                         [validated]
2. Its algebra contains only the WorkParts it requires.          [validated in sample]
3. A default service interpreter can execute that Feature.       [validated]
4. Two Features with different algebras compose into a Process.  [validated]
5. Composition uses natural injection + hoist.                   [validated]
6. The Process interpreter delegates to existing interpreters.   [validated]
7. Existing CRUD behavior remains green.                         [validated]
8. The model is clearer than the previous TodoPrograms model.    [accepted provisionally; keep testing against real cases]
```

Point 8 remains the reason to continue pressure against real software instead of declaring the taxonomy complete.

## Current continuation plan

Continue using the normal evidence-driven loop:

```text
real artifact
    -> first observable unsupported boundary
    -> identify who owns that boundary
    -> smallest coherent change
    -> rerun
    -> observe the next boundary
```

### 1. Use Ticket Support as the next pressure case

Do not create another artificial miniature first.

Inspect the real Ticket Support BFF composition, especially:

```text
AttachFile
    -> Folders.AddFile
    -> Tickets.AddAttachmentReference

RemoveAttachment
    -> Tickets.RemoveAttachmentReference
    -> Folders.RemoveFile
```

Establish the existing behavior, failure semantics, service ownership, and current implementation before adapting anything.

### 2. Map existing operations to WorkFlow ownership

For each child operation, determine:

- which service/product owns it;
- whether it already corresponds to a Feature/WorkFlow;
- what WorkParts its current behavior requires;
- what errors/failures are semantically meaningful;
- which effects are realization details;
- which guarantees are currently implicit.

Do not rename or refactor until ownership is reconstructed from repository evidence.

### 3. Attempt the smallest faithful WorkProcess representation

If both operations can be represented as independent WorkFlows, attempt Process composition using the existing mechanism:

```text
child algebra composition
    + external natural injection
    + Free hoist
    + interpreter delegation
```

Do not change `AlgebraSum` unless the real case exposes an actual limitation.

### 4. Stop at the first unsupported boundary

Likely pressure points may include, but are not assumed to include:

- cross-service error propagation;
- compensation/rollback behavior;
- attachment identity flow;
- ordering dependencies;
- partial success semantics;
- target/presentation mapping;
- transaction or guarantee expectations;
- more than two child WorkFlows;
- service-specific grounding differences.

Classify the first real mismatch before changing Framework.

### 5. Change only the owner of the discovered limitation

If the gap is:

- semantic WorkFlow vocabulary -> change the owning WorkFlow;
- composition mechanism -> change `VSlices.Work`;
- concrete realization -> change Grounding;
- product orchestration -> change the WorkProcess/product surface;
- guarantee semantics -> record separately; do not fold them into point capabilities;
- documentation-only mismatch -> update documentation without inventing code.

### 6. Re-run both miniature and real evidence

Any Framework change discovered from Ticket Support must preserve the miniature unless the real evidence proves the miniature model itself invalid.

## Explicit non-goals

The next continuation is not automatically trying to:

- migrate all of Ticket Support;
- introduce arbitrary-N algebra composition;
- generate algebras from VSIR;
- settle the final role of `Flow`;
- implement WorkLine;
- redesign guarantees;
- remove historical Repository / DatabaseIO APIs globally;
- claim that all WorkParts are persistence operations;
- impose the sample naming or project layout onto Ticket Support.

## Documentation rule

Do not formalize a broader Framework rule merely because the Todo miniature supports it.

Promote a rule only after the real pressure case preserves the same ownership and composition semantics without distortion.

If Ticket Support contradicts the miniature, preserve the contradiction as evidence and revise the model rather than forcing Ticket Support into it.
