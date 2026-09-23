# Handoff — Free WorkFlows and composed Feature algebras

> Date: 2026-09-23
>
> Origin branch: `experiment/cross-service-workprocess`
>
> Promoted to `master`: `83283716e2f4a4d98600bdc4ae0d805d7506838f`
>
> Original Free WorkFlow miniature baseline: `6c951a2bcca5fef28c1a6a246ae9df6328df981d`
>
> First successful cross-service composition: `ea9abe076fb4af42d47d1f759e61feb3c5b8e300`
>
> Status: Feature-as-Free and cross-service algebra composition are executable. The temporary `WorkProcess` type has been removed because composition is already represented by `Feature<..., ALG, ...>` when `ALG` is a composed algebra.

## Current conclusion

The current executable model is:

```text
Feature == WorkFlow == Free<ALG, Response>
```

where `ALG` is the language of WorkParts available to that Feature.

A Feature may own a direct algebra:

```text
CreateTodo
    ALG = CreateTodo.Algebra
```

or a composed algebra:

```text
CreateAndGetTodo
    ALG = AlgebraSum<
        CreateTodo.Algebra,
        GetTodo.Algebra>

AttachFileToTodo
    ALG = AlgebraSum<
        AddFile.Algebra,
        AddAttachmentReference.Algebra>
```

No second executable abstraction is required.

## Why WorkProcess was removed

The experiment originally introduced:

```csharp
WorkProcess<P, ALG, REQ, RES>
```

to represent a Work unit composed from several WorkFlows.

That interface had the same executable shape as Feature:

```text
REQ -> Free<ALG, RES>
```

The first Todo miniature did not prove whether the distinction was meaningful because both child WorkFlows used one Grounding.

The later cross-service sample provided stronger pressure:

```text
AttachFileToTodo
    -> SampleFileRepo.AddFile
       interpreted by InMemoryFileWork

    -> SampleWorkflow.AddAttachmentReference
       interpreted by InMemoryTodoWork
```

The existing Feature contract still represented the composed behavior without loss.

The observed trajectory is therefore:

```text
Feature
    -> discover child WorkFlow composition
    -> introduce WorkProcess hypothesis
    -> validate same-service composition
    -> validate cross-service composition
    -> observe identical executable contract
    -> locate the real distinction in ALG
    -> remove WorkProcess type
```

`WorkProcess` may remain useful as informal language for a process made from several WorkFlows, but it is not currently justified as a separate Framework interface.

## Feature contract

The current Framework surface is:

```csharp
public interface Feature<F, ALG, REQ, RES>
    where ALG : Functor<ALG>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Free<ALG, RES> Get(REQ request);
}
```

The Functor requirement belongs to `ALG`, not to the self type `F`.

Conceptually:

```text
F
    Feature identity / self type

ALG
    instruction language available to the Feature

REQ
    explicit request

RES
    explicit result
```

## WorkParts and Feature-owned algebras

A WorkPart remains:

```text
a specific instruction in a WorkFlow
```

A direct Feature can own only the WorkParts it needs:

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

AddAttachmentReference.Algebra
    ReadTodo
    WriteTodo
```

Pure semantic transformations such as `todo.Update(...)` do not become WorkParts merely because they occur inside a WorkFlow.

## Composed Feature algebras

When a Feature reuses child Feature programs, its `ALG` can be an `AlgebraSum`.

The supported positional family follows the A..G convention:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
AlgebraSum<A, B, C, D>
AlgebraSum<A, B, C, D, E>
AlgebraSum<A, B, C, D, E, F>
AlgebraSum<A, B, C, D, E, F, G>
```

This is intentionally finite for now. Do not extend beyond seven without pressure.

The positions are mechanism, not semantic priority.

## Hoisting ergonomics

Each `AlgebraSum` exposes positional helpers:

```text
FromA
FromB
FromC
FromD
FromE
FromF
FromG
```

as applicable.

For example:

```csharp
using Algebra = AlgebraSum<
    AddFile.Algebra,
    AddAttachmentReference.Algebra>;

var stored =
    Algebra.FromA(
        AddFile.Get(...));

var associated =
    Algebra.FromB(
        AddAttachmentReference.Get(...));
```

These helpers perform the same mathematical operation as:

```text
external natural transformation
    +
Free hoist
```

without making every Feature call site spell the full witness type.

The explicit witnesses remain available:

```text
InjectA
InjectB
InjectC
InjectD
InjectE
InjectF
InjectG
```

No `InjectLeft` / `InjectRight` compatibility aliases are retained. `InjectA` through `InjectG` are the only supported injection witnesses.

## Grounding

Grounding still owns concrete realization.

A direct interpreter is:

```text
AlgebraIO<Feature.Algebra>
```

A composed algebra can combine independently owned interpreters:

```csharp
new AlgebraSumIO<
    AddFile.Algebra,
    AddAttachmentReference.Algebra>(
        fileWork,
        todoWork);
```

The cross-service sample proves that the child Groundings do not need to be the same object or service boundary.

The same A..G arity family is available for `AlgebraSumIO`.

## Ownership rules

Preserve:

```text
Feature / WorkFlow owns its WorkParts.

A composing Feature owns:
    its composed ALG
    ordering of child WorkFlows
    integration mappings between child semantics
    injection of child programs into its ALG

Grounding owns realization.

A child Feature does not know:
    which later Feature reuses it
    which BFF reuses it
    which larger algebra it may be hoisted into
```

For `SampleBFF.AttachFileToTodo`:

```text
SampleFileRepo
    owns SampleFileId + file behavior

SampleWorkflow
    owns Todo + ResourceReference association

SampleBFF
    owns SampleFileId -> ResourceReference
    owns AddFile -> AddAttachmentReference ordering
```

Todo does not depend on SampleFileRepo.

## AlgebraSum is mechanism

Do not elevate the concrete coproduct representation into universal semantics.

The useful semantic statement is:

```text
this Feature can express the WorkParts required by these child WorkFlows
```

The current .NET mechanism is:

```text
AlgebraSum<A,...,G>
    +
InjectA...G
    +
Free hoist
    +
AlgebraSumIO<A,...,G>
```

If later evidence justifies a different representation, child Features should not need to change merely because the composition mechanism changes.

## Cross-service executable evidence

The sample reproduces the Ticket Support attachment geometry:

```text
SampleBFF.AttachFileToTodo
    -> SampleFileRepo.AddFile
    -> SampleWorkflow.AddAttachmentReference
```

The first green cross-service run proved:

```text
Branch: experiment/cross-service-workprocess
Head:   ea9abe076fb4af42d47d1f759e61feb3c5b8e300
Run:    35857218289
Result: success
```

Later commits remove `WorkProcess`, introduce the A..G API, and add direct arity-seven evidence. After promotion, `master` CI is authoritative for the maintained state.

## Partial failure is the next semantic pressure

The sample intentionally preserves this behavior:

```text
AddFile succeeds
AddAttachmentReference cannot associate
    -> no Todo association
    -> file remains stored
```

Therefore:

```text
Feature composition
!= atomicity
!= rollback
!= compensation
!= reconciliation
```

This does not currently justify changing `AlgebraSum`.

The next substantive design question is where stronger cross-WorkFlow guarantees belong when a real product requires them.

Possible future concepts include compensation, retry, reconciliation, atomicity or another guarantee vocabulary, but none should be smuggled into the composition mechanism by default.

## Flow remains unresolved

The relationship between:

```text
Feature / Free WorkFlow
Flow<RT, REQ, RES>
presentation invocation
runtime interpretation
```

remains open.

Do not restore `Flow` as the Feature boundary from historical documentation, and do not delete it merely because the current experiment uses Free directly.

## Ticket Support remains a real-world pressure case

The real product geometry remains:

```text
AttachFile
    -> Folders.AddFile
    -> Tickets.AddAttachmentReference

RemoveAttachment
    -> Tickets.RemoveAttachmentReference
    -> Folders.RemoveFile
```

Its child Features still live on the historical `VSlices.Application` surface.

The isolated samples deliberately avoid turning the composition experiment into a whole-repository Framework migration.

When Ticket Support is revisited, migrate or recreate only enough of the real WorkFlows to pressure:

- cross-service failures;
- compensation/retry/reconciliation;
- authorization;
- attachment identity mapping;
- ordering;
- stronger guarantees if actually required.

Do not create a generic `Flow -> Free` adapter merely to make old Features participate.

## Current evidence requirements

Keep green evidence for:

```text
VSlices.Work build
VSlices.Work.Services build
VSlices.Work.Products build
Work algebra tests
AlgebraSum A..G test
SampleWorkflow API build
composed Feature hoist test
SampleFileRepo build
cross-service SampleBFF composition tests
CRUD smoke test
```

## Explicit non-goals

This experiment does not currently try to:

- create a separate ComposedFeature type;
- restore WorkProcess as a separate interface;
- extend AlgebraSum past seven children;
- settle compensation or transaction semantics;
- settle the final role of Flow;
- implement WorkLine;
- generate composition from VSIR;
- migrate all of Ticket Support;
- claim that all WorkParts are persistence operations.

## Documentation rule

The current conclusion is evidence-backed but still revisable:

```text
Feature is the executable WorkFlow abstraction.
Composition is represented in ALG.
```

If a later real case requires a distinct executable category, preserve the contradiction and reopen the model rather than forcing it into the current shape.
