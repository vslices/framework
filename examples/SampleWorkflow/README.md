# Simple Workflow

This example is a small CRUD API used to pressure the current VSlices Work model.

The current model is:

```text
Feature
    -> Eff<TodoAlgebra, Response> Get(Request)

TodoAlgebra
    -> executable capability atoms

Grounding
    -> implements those atoms
    -> exposes TodoAlgebra
```

There is no Free-monad/interpreter layer and no separate executable `WorkProcess` abstraction.

## Structure

```text
SampleWorkflow.Spaces
    TodoId
    TodoDetail
    Todo : Evolvable<Todo, Todo.State>

SampleWorkflow.Work
    TodoAlgebra
        PointReader<Todo, TodoId>
        PointWriter<Todo>
        PointRemover<Todo, TodoId>
        TodoIdSource

    CreateTodo
    GetTodo
    UpdateTodo
    DeleteTodo
    AddAttachmentReference

SampleWorkflow.Grounding
    InMemoryTodoWork
        implements the executable atoms
        exposes TodoAlgebra

SampleWorkflow.Process
    CreateAndGetTodo
        composes CreateTodo and GetTodo over the same TodoAlgebra

SampleWorkflow.Api
    HTTP presentation
    obtains TodoAlgebra from Grounding and executes Feature effects
```

## Module algebra

The Work module owns one executable algebra:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

The algebra is the runtime of the module's Features. Individual Features use only the atoms they require.

For example, `GetTodo.Get(new GetTodo.Request(id))` returns `Eff<TodoAlgebra, GetTodo.Response>` and can run using `RunIO(grounding.Algebra)`. The Response constructor is private and is reached through a Feature-owned `NewResponse` accessor.

## Same-module Feature composition

`CreateAndGetTodo` demonstrates that Features sharing the same module algebra compose directly.

The child Features keep ownership of their own request/response contracts. The composing Feature passes explicit child Requests and composes effects using their shared algebra.

No algebra hoisting or interpreter composition is required.

## Grounding

`InMemoryTodoWork` implements the executable atoms and exports the completed algebra:

```text
InMemoryTodoWork
    PointReader<Todo, TodoId>
    PointWriter<Todo>
    PointRemover<Todo, TodoId>
    TodoIdSource
        |
        v
    TodoAlgebra
```

The same Work algebra could be exposed by a different Grounding without changing the Feature contracts.

## CRUD API

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

`TodoDetail` covers only the semantic string. `Completed` remains a plain `bool`.

`Todo` is `Evolvable<Todo, Todo.State>`; PUT reads the current point, proposes the next state with `Todo.Update(...)`, and writes only the accepted evolved point.

## What this sample does not imply

Point capability atoms do not imply:

- Repository;
- Unit of Work;
- transactions;
- atomicity;
- isolation;
- durability;
- tracking.

Those remain independent semantic questions.
