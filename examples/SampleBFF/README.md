# SampleBFF — cross-service Feature composition experiment

## Purpose

This sample pressures executable-algebra Eff composition across two independently owned Work modules.

The scenario is:

```text
SampleBFF.AttachFileToTodo
    -> SampleFileRepo.AddFile
    -> SampleWorkflow.AddAttachmentReference
```

`AttachFileToTodo` remains an ordinary Feature. Its runtime is the structural composition of the two module algebras:

```csharp
AlgebraMix<FileAlgebra, TodoAlgebra>
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
    owns explicit child Requests and algebra projection for child Effects

Grounding
    owns realization of each algebraic capability atom
```

The Todo surface does not depend on `SampleFileRepo`. It stores only an opaque `ResourceReference`.

## Runtime composition

The BFF Feature is:

```text
Feature<
    AttachFileToTodo,
    AlgebraMix<FileAlgebra, TodoAlgebra>,
    Request,
    Response>
```

Each child Feature receives its nominal public Request and returns an `Eff` that requires its own module algebra. The BFF projects the corresponding runtime explicitly:

```csharp
AddFile.Get(new AddFile.Request(name, content))
    .RunIO(mix.A);

AddAttachmentReference.Get(new AddAttachmentReference.Request(todoId, resource))
    .RunIO(mix.B);
```

The parent owns composition, and neither child needs to know about the BFF. This does not require `MapRuntime`, `MapRequest` or `ContraMap`.

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
new AlgebraMix<FileAlgebra, TodoAlgebra>(
    files.Algebra,
    todo.Algebra)
```

No child Grounding needs to know that the BFF exists.

## AlgebraMix arities

VSlices currently offers positional structural composition through seven children:

```text
AlgebraMix<A, B>
AlgebraMix<A, B, C>
AlgebraMix<A, B, C, D>
AlgebraMix<A, B, C, D, E>
AlgebraMix<A, B, C, D, E, F>
AlgebraMix<A, B, C, D, E, F, G>
```

The A..G positions are mechanism only. They do not encode semantic priority or authority.

## Executable evidence

The tests demonstrate that:

1. each child Feature runs against its independently owned module algebra;
2. the BFF can compose those algebras structurally;
3. the parent projects the child runtime explicitly at `RunIO`;
4. the parent constructs each child Request explicitly;
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

AlgebraMix
!= transaction
!= rollback
!= compensation
```

This is evidence for a future guarantee model, not a reason to hide stronger semantics inside algebra composition.
