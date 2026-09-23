# Simple Workflow

This example is a small CRUD API used to pressure the current VSlices Work model.

The current experiment treats:

```text
Feature == WorkFlow == Free<ALG, Response>

ALG
    contains the WorkParts that the Feature can express
    and may itself be a composition of child Feature algebras
```

There is no separate executable `WorkProcess` abstraction. A Feature that reuses other Features remains a normal Feature whose `ALG` is composed.

## Structure

```text
SampleWorkflow.Spaces
    TodoId
    TodoDetail
    Todo : Evolvable<Todo, Todo.State>

SampleWorkflow.Work
    CreateTodo Feature / WorkFlow
        CreateTodo.Algebra
        NextId / Read / Write WorkParts

    GetTodo Feature / WorkFlow
        GetTodo.Algebra
        Read WorkPart

    UpdateTodo Feature / WorkFlow
        UpdateTodo.Algebra
        Read / Write WorkParts

    DeleteTodo Feature / WorkFlow
        DeleteTodo.Algebra
        Read / Remove WorkParts

    AddAttachmentReference Feature / WorkFlow
        AddAttachmentReference.Algebra
        Read / Write WorkParts

SampleWorkflow.Grounding
    InMemoryTodoWork
        interprets the Todo-owned WorkFlow algebras

SampleWorkflow.Process
    CreateAndGetTodo Feature
        ALG = AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>
        reuses CreateTodo and GetTodo by hoisting both child programs

SampleWorkflow.Api
    HTTP presentation
    DI supplies the default interpreter for each WorkFlow algebra
```

## Feature as Free Monad

A Feature does not own a runtime `RT` or return `Flow<RT, REQ, RES>`.

Its semantic contract is:

```text
Request
    -> Free<ALG, Response>
```

The Feature itself contains the WorkFlow. There is no parallel `TodoPrograms` layer.

## Feature composition through ALG

`CreateAndGetTodo` demonstrates that composition does not need another Feature category.

Its contract is still:

```text
Feature<CreateAndGetTodo, Algebra, Request, Response>
```

with:

```text
Algebra =
    AlgebraSum<
        CreateTodo.Algebra,
        GetTodo.Algebra>
```

The child programs remain independently defined:

```text
CreateTodo.Get(...)
    -> Free<CreateTodo.Algebra, ...>

GetTodo.Get(...)
    -> Free<GetTodo.Algebra, ...>
```

and the composing Feature embeds them into its larger vocabulary:

```csharp
Algebra.FromA(CreateTodo.Get(...))
Algebra.FromB(GetTodo.Get(...))
```

`FromA` / `FromB` are ergonomic hoist helpers. The mathematical mechanism remains an external natural transformation plus `Free` hoisting.

The child WorkFlows do not know which later Feature may reuse them.

## AlgebraSum arities

VSlices currently offers positional sums following the A..G convention:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
AlgebraSum<A, B, C, D>
AlgebraSum<A, B, C, D, E>
AlgebraSum<A, B, C, D, E, F>
AlgebraSum<A, B, C, D, E, F, G>
```

Each arity exposes the corresponding `FromA` ... `FromG` helpers and `InjectA` ... `InjectG` natural transformations.

The positional structure is mechanism. It does not imply semantic priority between child WorkFlows.

## Interpretation

A Grounding may provide one implementation that satisfies several independent WorkFlow algebras:

```text
InMemoryTodoWork
    AlgebraIO<CreateTodo.Algebra>
    AlgebraIO<GetTodo.Algebra>
    AlgebraIO<UpdateTodo.Algebra>
    AlgebraIO<DeleteTodo.Algebra>
    AlgebraIO<AddAttachmentReference.Algebra>
```

A composed algebra can combine the already-existing interpreters:

```text
CreateTodo interpreter --\
                        +--> AlgebraSumIO<CreateTodo.Algebra, GetTodo.Algebra>
GetTodo interpreter ----/
```

The composed interpreter only redirects operations. It does not reimplement either WorkFlow.

## CRUD API

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

`TodoDetail` covers only the semantic string. `Completed` remains a plain `bool`.

`Todo` is `Evolvable<Todo, Todo.State>`; PUT reads the current point, proposes the next state with `Todo.Update(...)`, and writes only the accepted evolved point.
