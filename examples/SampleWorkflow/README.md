# Simple Workflow

This example is a small CRUD API used to pressure the current VSlices Work model.

The experiment now treats:

```text
Feature == WorkFlow == Free<WorkFlowAlgebra, Response>

WorkFlowAlgebra
    contains the WorkParts required by that Feature

WorkProcess
    composes existing WorkFlow algebras
    hoists each Feature into the composed algebra
```

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

SampleWorkflow.Grounding
    InMemoryTodoWork
        interprets all four WorkFlow algebras

SampleWorkflow.Process
    CreateAndGetTodo WorkProcess
        composes CreateTodo.Algebra + GetTodo.Algebra
        hoists CreateTodo and GetTodo into that composed algebra

SampleWorkflow.Api
    HTTP presentation
    DI supplies the default interpreter for each WorkFlow algebra
```

## Feature as Free Monad

A Feature no longer owns a runtime `RT` or returns `Flow<RT, REQ, RES>`.

Its semantic contract is:

```text
Request
    -> Free<Feature.Algebra, Response>
```

The Feature itself contains the composition. There is no parallel `TodoPrograms` layer.

## WorkProcess through algebra composition

`CreateAndGetTodo` demonstrates the next level:

```text
CreateTodo
    Free<CreateTodo.Algebra, ...>

GetTodo
    Free<GetTodo.Algebra, ...>
```

The Process algebra is:

```text
AlgebraSum<CreateTodo.Algebra, GetTodo.Algebra>
```

Each Feature is lifted into that larger vocabulary with the same mathematical operation as `Free.hoist`.

VSlices uses an external natural-transformation witness so the composing Process owns the injection. A service WorkFlow algebra does not need to reference the BFF or Process that later composes it.

## Interpretation

A service can provide one implementation that satisfies several WorkFlow algebras:

```text
InMemoryTodoWork
    AlgebraIO<CreateTodo.Algebra>
    AlgebraIO<GetTodo.Algebra>
    AlgebraIO<UpdateTodo.Algebra>
    AlgebraIO<DeleteTodo.Algebra>
```

Normal service execution receives the appropriate interface from DI.

A Process interpreter composes the already-existing interpreters:

```text
CreateTodo interpreter ─┐
                        ├─ AlgebraSumIO<CreateTodo.Algebra, GetTodo.Algebra>
GetTodo interpreter ────┘
```

The composed interpreter only redirects. It does not reimplement either WorkFlow.

## CRUD API

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

`TodoDetail` covers only the semantic string. `Completed` remains a plain `bool`.

`Todo` is `Evolvable<Todo, Todo.State>`; PUT reads the current point, proposes the next state with `Todo.Update(...)`, and writes only the accepted evolved point.
