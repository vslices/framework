# SampleBFF — cross-service Feature composition experiment

## Purpose

This sample pressures executable-algebra Flow composition across two independently owned Work modules.

The scenario is:

```text
SampleBFF.AttachFileToTodo
    -> SampleFileRepo.AddFile
    -> SampleWorkflow.AddAttachmentReference
```

`AttachFileToTodo` remains an ordinary Feature. Its runtime is the structural composition of the two module algebras:

```csharp
AlgebraSum<FileAlgebra, TodoAlgebra>
```

There is no Free-monad program, hoisting layer, or composed interpreter.

## Ownership

```text
SampleFileRepo
    owns FileAlgebra
    owns file Features
    owns SampleFile semantics

SampleWorkflow
    owns TodoAlgebra
    owns Todo Features
    owns Todo -> ResourceReference association semantics

SampleBFF
    owns cross-service coordination
    owns FileAlgebra + TodoAlgebra composition
    owns request/runtime adaptation between child Flows

Grounding
    owns realization of each algebraic capability atom
```

The Todo surface does not depend on `SampleFileRepo`. It stores only an opaque `ResourceReference`.

## Runtime composition

The BFF Feature is:

```text
Feature<
    AttachFileToTodo,
    AlgebraSum<FileAlgebra, TodoAlgebra>,
    Request,
    Response>
```

Each child Flow keeps its smaller runtime:

```text
AddFile
    Flow<FileAlgebra, AddFile.Request, AddFile.Response>

AddAttachmentReference
    Flow<TodoAlgebra, AddAttachmentReference.Request, ...>
```

The parent adapts each child to the larger runtime by projection:

```csharp
AddFile.Get()
    .MapRuntime(
        (AlgebraSum<FileAlgebra, TodoAlgebra> sum) => sum.A)

AddAttachmentReference.Get()
    .MapRuntime(
        (AlgebraSum<FileAlgebra, TodoAlgebra> sum) => sum.B)
```

Requests are adapted independently with `MapRequest`.

This gives the composition geometry:

```text
parent runtime
    AlgebraSum<FileAlgebra, TodoAlgebra>
        |                       |
        v                       v
    FileAlgebra             TodoAlgebra
        |                       |
        v                       v
    AddFile Flow     AddAttachmentReference Flow
```

The runtime mapping is contravariant: the parent provides enough structure to satisfy the smaller runtime required by the child.

## Grounding

The test uses two independent realizations:

```text
InMemoryFileWork
    -> FileAlgebra

InMemoryTodoWork
    -> TodoAlgebra
```

The BFF composes the exported algebra values:

```csharp
new AlgebraSum<FileAlgebra, TodoAlgebra>(
    files.Algebra,
    todo.Algebra)
```

No child Grounding needs to know that the BFF exists.

## AlgebraSum arities

VSlices currently offers positional structural composition through seven children:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
AlgebraSum<A, B, C, D>
AlgebraSum<A, B, C, D, E>
AlgebraSum<A, B, C, D, E, F>
AlgebraSum<A, B, C, D, E, F, G>
```

The A..G positions are mechanism only. They do not encode semantic priority or authority.

## Executable evidence

The tests demonstrate that:

1. each child Feature runs against its independently owned module algebra;
2. the BFF can compose those algebras structurally;
3. `MapRuntime` projects the larger runtime into each child runtime;
4. `MapRequest` adapts the parent request independently;
5. both Groundings remain independently readable after the composed Feature completes;
6. no Free/interpreter layer is required for this composition.

## First guarantee pressure

The sample intentionally preserves the partial-failure case:

```text
AddFile succeeds
AddAttachmentReference cannot find the Todo

=> file remains stored
=> no association is created
```

Therefore:

```text
Feature composition
!= atomic composition

AlgebraSum
!= transaction
!= rollback
!= compensation
```

This is evidence for a future guarantee model, not a reason to hide stronger semantics inside algebra composition.
