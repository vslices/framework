# Simple Workflow

This example is a small CRUD API used to pressure the current VSlices Work model.

The current model is:

```text
Feature
    -> Flow<TodoAlgebra, Request, Response>

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
    obtains TodoAlgebra from Grounding and executes Feature Flows
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

For example:

```csharp
public sealed class GetTodo :
    Feature<GetTodo, TodoAlgebra, GetTodo.Request, GetTodo.Response>
{
    public static Flow<TodoAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.Id)
                .Map(todo => new Response(todo)));
}
```

## Same-module Feature composition

`CreateAndGetTodo` demonstrates that Features sharing the same module algebra compose directly.

The child Features keep ownership of their own request/response contracts. The composing Feature adapts requests with `MapRequest` and composes the resulting Flows.

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
