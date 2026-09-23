# Handoff — Free WorkFlows and composed WorkProcess algebras

> Date: 2026-09-23
>
> Branch: `experiment/simple-workflow-point-crud`
>
> Baseline at handoff: `6c951a2bcca5fef28c1a6a246ae9df6328df981d`
>
> Status: design direction discovered; implementation of the latest Free WorkFlow / WorkProcess composition change is still pending.

## Purpose

This handoff preserves the current reasoning and the concrete continuation path for the `SampleWorkflow` experiment in `vslices/framework`.

The immediate question is no longer whether Free Monads can represent point-oriented capabilities. The stronger hypothesis now is:

```text
Feature == WorkFlow == Free<WorkFlowAlgebra, Response>
```

where the WorkFlow algebra contains the WorkParts that the Feature may execute.

The next level is:

```text
WorkProcess
    composes existing WorkFlows
    by composing their algebras
    and hoisting each WorkFlow into the composed algebra
```

This should be tested before any wider Framework migration or documentation is treated as stable.

## Current validated baseline

Before this latest design turn, the example had already demonstrated a working CRUD API with:

```text
SampleWorkflow.Spaces
SampleWorkflow.Work
SampleWorkflow.Grounding
SampleWorkflow.Api
```

The current semantic model in `SampleWorkflow.Spaces` is intentional:

### TodoId

`TodoId` is a semantic discrete space established from a `Guid`.

```text
Guid
    -> TodoId.Transformation
    -> TodoId
```

Identity generation is a separate realization concern from the semantic transformation that establishes a `TodoId`.

### TodoDetail

`TodoDetail` covers only the textual value.

```text
string
    -> TodoDetail.Transformation
    -> TodoDetail
```

`Completed` remains a plain `bool`; there is no current evidence requiring a separate semantic space for it.

### Todo

`Todo` is a semantic point established from:

```text
TodoId
TodoDetail
bool Completed
```

and implements:

```text
Evolvable<Todo, Todo.State>
```

Its identity is creation-fixed while `Detail` and `Completed` can evolve.

The PUT path is intentionally modeled as:

```text
read current Todo
    -> Todo.Update(...)
    -> accepted evolved Todo
    -> write point
```

rather than constructing an unrelated replacement aggregate from HTTP input.

The CRUD smoke workflow was green at the baseline represented by this handoff.

## The design problem that triggered this handoff

The previous point-algebra experiment introduced a shared `TodoPrograms` layer.

That produced this shape:

```text
Feature
    -> wrapper around TodoPrograms

TodoPrograms
    -> actual application behavior
```

This is the wrong ownership boundary for the current Work model.

The new interpretation is that the Feature itself is the WorkFlow, and therefore the Feature itself should be the Free program that composes its WorkParts.

There should not be a parallel `TodoPrograms.Update` that owns the real WorkFlow while `UpdateTodo` merely forwards to it.

## Work hierarchy

The current conceptual hierarchy is:

```text
1 WorkLine    : N WorkProcess
1 WorkProcess : M WorkFlow
1 WorkFlow    : Q WorkPart
```

For the current experiment:

```text
WorkPart
    specific instruction

WorkFlow
    Feature
    Free Monad over its WorkPart algebra

WorkProcess
    composition of multiple existing WorkFlows

WorkLine
    still outside the current implementation scope
```

Do not generalize WorkLine yet.

## Feature target shape

The current Framework Feature is still:

```csharp
Feature<F, RT, REQ, RES>
    static Flow<RT, REQ, RES> Get()
```

The next experiment should pressure a shape closer to:

```csharp
Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
```

The important semantic change is:

```text
Feature does not merely execute a WorkFlow.
Feature is the WorkFlow.
```

Runtime and concrete realization should not be required to define that semantic program.

### Unresolved Flow question

Do not delete or redesign `Flow` merely to make this experiment work.

The relationship between:

```text
Feature / Free WorkFlow
Flow<RT,REQ,RES>
presentation invocation
runtime interpretation
```

is still open.

First prove that Feature-as-Free and WorkProcess hoisting work. Then decide whether Flow remains an execution carrier, becomes presentation/runtime syntax, changes role, or is superseded in this path.

## WorkFlow algebras

Each Feature / WorkFlow should own the algebra required to express its WorkParts.

The Todo CRUD should pressure separate algebras rather than one shared `TodoAlgebra`.

Example target:

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

These can still reuse generic vocabulary such as:

```text
PointReader
PointWriter
PointRemover
```

but each Feature owns the concrete WorkPart signature that it actually needs.

The exact generated/handwritten split remains open. ADT cases and `Functor.Map` are mechanical and remain strong Tooling-generation candidates.

## Grounding

A single service implementation may satisfy several WorkFlow algebras.

For the in-memory Todo sample, the intended shape is conceptually:

```csharp
InMemoryTodoWork :
    AlgebraIO<CreateTodo.Algebra>,
    AlgebraIO<GetTodo.Algebra>,
    AlgebraIO<UpdateTodo.Algebra>,
    AlgebraIO<DeleteTodo.Algebra>
```

This does not mean that the algebras are the same.

It means the same concrete service realization knows how to interpret the WorkParts required by several WorkFlows.

For normal service execution, DI only needs to expose the appropriate default interface:

```text
AlgebraIO<CreateTodo.Algebra>
AlgebraIO<GetTodo.Algebra>
...
```

The Feature remains unaware of the concrete interpreter.

## WorkProcess composition

A WorkProcess composes already-existing WorkFlows.

The motivating real precedent is the Ticket Support BFF:

```text
AttachFile
    -> Folders.AddFile
    -> Tickets.AddAttachmentReference
```

and:

```text
RemoveAttachment
    -> Tickets.RemoveAttachmentReference
    -> Folders.RemoveFile
```

These product operations coordinate WorkFlows owned by different service boundaries.

The miniature experiment in `SampleWorkflow` should reproduce the same geometry with two Todo Features before touching Ticket Support.

Suggested process:

```text
CreateAndGetTodo
    -> CreateTodo
    -> GetTodo
```

## Composed algebra

A WorkProcess algebra is the composition of the algebras of the WorkFlows it coordinates.

For two WorkFlows:

```text
WorkflowAlgebraA ─┐
                  ├─ ProcessAlgebra
WorkflowAlgebraB ─┘
```

The simplest first implementation can use a binary algebra sum/coproduct:

```text
AlgebraSum<A, B>
```

with explicit cases for operations coming from the left or right algebra.

The composed algebra must implement `Functor`.

Do not prematurely build arbitrary-N machinery; binary composition is enough to validate the mechanism and can be nested later if pressure justifies it.

## Hoisting

Each WorkFlow must be lifted into the Process algebra without changing its behavior.

Conceptually:

```text
Free<A, X>
    -> Free<ProcessAlgebra, X>

Free<B, Y>
    -> Free<ProcessAlgebra, Y>
```

This is exactly the role of `Free.hoist`.

### LanguageExt ownership caveat

The current LanguageExt implementation has this shape:

```csharp
Free.hoist<F, G, A>(Free<F, A>)
    where F : Functor<F>, Natural<F, G>
    where G : Functor<G>
```

That requires `F` itself to know the natural transformation into `G`.

That ownership is undesirable for VSlices because a service WorkFlow algebra must not know which BFF or WorkProcess may compose it later.

Therefore the experiment should provide an external natural-transformation witness owned by the composing Process, for example conceptually:

```text
InjectLeft<A,B>  : A ~> AlgebraSum<A,B>
InjectRight<A,B> : B ~> AlgebraSum<A,B>
```

and a thin VSlices helper with the same recursive semantics as `Free.hoist`:

```csharp
FreeAlgebra.hoist<N, F, G, A>(Free<F, A>)
    where N : Natural<F, G>
```

The mathematical mechanism remains hoisting; only ownership of the natural transformation differs from the LanguageExt convenience API.

## Process interpreter

The Process should not reimplement the WorkParts of its child WorkFlows.

Its interpreter should only dispatch each branch to the interpreter that already owns it.

Conceptually:

```text
Workflow A interpreter ─┐
                        ├─ Process interpreter
Workflow B interpreter ─┘
```

For a binary sum:

```text
Left operation
    -> left interpreter

Right operation
    -> right interpreter
```

This is the miniature analogue of a Ticket Support BFF receiving the default Folders and Tickets interpreters and redirecting each composed operation to its owner.

## Important ownership rules

Preserve these while implementing:

```text
WorkFlow owns its WorkParts.
WorkProcess owns composition of WorkFlows.
Grounding owns realization.
A composing Process owns the injection into its larger algebra.
A lower WorkFlow does not know its future Process/BFF.
```

Also preserve the broader VSlices distinction:

```text
semantic meaning
!= execution mechanism
!= concrete realization
!= authorization
!= guarantee
```

## Guarantees remain out of scope

Do not use this refactor as an excuse to implement the pending guarantee model.

Still deferred:

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

The Work composition experiment should remain independently verifiable.

## WorkPart classification remains partially open

The current claim is:

```text
WorkPart = specific instruction in a WorkFlow
```

Do not infer that every pure domain calculation must become an explicit WorkPart.

For example:

```csharp
todo.Update(...)
```

may remain a pure semantic transformation inside a continuation unless evidence shows that evolution itself must be represented as an inspectable WorkPart.

The first experiment should prefer the minimum explicit instruction vocabulary required by the actual WorkFlow.

## Concrete continuation plan

Continue in small, verifiable steps.

### 1. Add the composition mechanism

Implement, experimentally:

```text
AlgebraSum<L,R>
InjectLeft<L,R>
InjectRight<L,R>
AlgebraSumIO<L,R>
external-witness Free hoist helper
```

Add focused tests before changing the CRUD.

### 2. Refactor Feature

Pressure:

```text
Feature<F,ALG,REQ,RES>
    -> Free<ALG,RES>
```

Do not make a larger architecture decision about Flow yet.

### 3. Refactor one WorkFlow first

Start with `GetTodo` because it has one WorkPart.

Validate:

```text
GetTodo.Get(request)
    -> Free<GetTodo.Algebra, Response>
```

and direct interpretation through its default service interpreter.

### 4. Refactor UpdateTodo

Move the actual WorkFlow into the Feature and remove the parallel `TodoPrograms.Update` ownership.

Preserve:

```text
read current Todo
    -> Todo.Update(...)
    -> write accepted evolution
```

### 5. Complete Create/Delete

Create should include identity generation in its own WorkPart vocabulary or another equally explicit capability representation.

Delete should pressure the current `PointRemover` vocabulary.

### 6. Remove the obsolete shared program layer

Once all CRUD Features own their programs:

```text
delete TodoPrograms
```

Only remove the shared `TodoAlgebra` when no valid use remains.

### 7. Compose a miniature WorkProcess

Add a small `CreateAndGetTodo` process:

```text
CreateTodo
    -> hoist into ProcessAlgebra
    -> GetTodo
    -> hoist into ProcessAlgebra
```

The result should be one:

```text
Free<ProcessAlgebra, ProcessResponse>
```

### 8. Test interpreter delegation

Use the same in-memory service implementation as the default interpreter for the individual WorkFlow algebras.

Then build a composed Process interpreter that only delegates left/right operations.

Verify that no WorkPart realization is duplicated in the Process.

### 9. Adapt HTTP execution

Keep the existing CRUD API behavior intact:

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

DI should provide the default interpreter for the relevant WorkFlow algebra.

### 10. Run evidence

Require at least:

```text
VSlices.Work build
SampleWorkflow API build
CRUD smoke test
WorkProcess hoist/composition test
```

before treating the new shape as successful.

### 11. Documentation only after executable evidence

If the experiment is green, update the Framework docs to reflect:

```text
Feature == WorkFlow
WorkFlow == Free over WorkParts
WorkProcess composes WorkFlows through algebra composition + hoist
```

If the experiment exposes a contradiction, preserve that evidence instead of forcing the model.

## Files likely to change

Framework:

```text
src/VSlices.Work/Feature.cs
src/VSlices.Work/Algebra/AlgebraIO.cs
src/VSlices.Work/Algebra/*
src/VSlices.Work.Services/Traits/ServiceFeature.cs
src/VSlices.Work.Products/Traits/ProductFeature.cs
```

Example:

```text
examples/SampleWorkflow/SampleWorkflow.Work/*
examples/SampleWorkflow/SampleWorkflow.Grounding/*
examples/SampleWorkflow/SampleWorkflow.Api/*
examples/SampleWorkflow/SampleWorkflow.Process/*        [new]
examples/SampleWorkflow/SampleWorkflow.Process.Tests/*  [new]
```

Potential cleanup after successful migration:

```text
TodoPrograms.cs
TodoAlgebra.cs
TodoIdGeneration.cs / GuidTodoIdGeneration.cs
```

Do not delete them before their responsibilities have been demonstrably recovered in the new model.

## Explicit non-goals

This continuation is not yet trying to:

- migrate Ticket Support itself;
- introduce arbitrary-N algebra composition;
- generate algebras from VSIR;
- settle the final role of `Flow`;
- implement WorkLine;
- redesign guarantees;
- remove historical Repository / DatabaseIO APIs;
- claim that all WorkParts are persistence operations.

## Success criterion

The experiment is successful if all of the following are simultaneously true:

```text
1. A Feature directly is a Free WorkFlow.
2. Its algebra contains only the WorkParts it requires.
3. A default service interpreter can execute that Feature.
4. Two Features with different algebras can be composed into one WorkProcess.
5. Composition uses natural injection + hoist rather than making lower WorkFlows know the Process.
6. The Process interpreter delegates to existing WorkFlow interpreters.
7. Existing CRUD behavior remains green.
8. The code is easier to read as WorkLine / WorkProcess / WorkFlow / WorkPart semantics than the previous TodoPrograms model.
```

If point 8 fails even while the code works technically, do not formalize the model yet.

## Real-world pressure after the miniature

If the miniature succeeds, the next valuable pressure is not another artificial sample.

Use Ticket Support BFF composition, especially:

```text
AttachFile
    -> Folders.AddFile
    -> Tickets.AddAttachmentReference

RemoveAttachment
    -> Tickets.RemoveAttachmentReference
    -> Folders.RemoveFile
```

Those cases can reveal whether the miniature composition preserves the ownership, boundaries and failure semantics that the real product currently needs.
