# Simple Workflow

This example is intentionally a very small CRUD API built with the current VSlices semantic-space and point-algebra model.

The concept is `Todo` and is split into the current architecture:

```text
SampleWorkflow.Spaces
    TodoId
    TodoDetail
    Todo : Evolvable<Todo, Todo.State>

SampleWorkflow.Work
    TodoId generation capability
    TodoAlgebra
    PointReader<Todo>
    PointWriter<Todo>
    PointRemover<Todo>
    CRUD Features

SampleWorkflow.Grounding
    GuidTodoIdGeneration
    InMemoryTodoAlgebra

SampleWorkflow.Api
    HTTP presentation + runtime composition
```

## Semantic construction

The example does not expose public constructors for semantic points.

`TodoId` is a discrete semantic space established from a `Guid`:

```text
Guid
    -> TodoId.Transformation
    -> TodoId
```

`TodoDetail` gives semantic meaning only to the textual detail:

```text
string
    -> TodoDetail.Transformation
    -> TodoDetail
```

The completion flag remains a `bool`; this example has no evidence that it requires a separate semantic space.

`Todo` is established from already-semantic identity/detail plus the valid boolean state:

```text
Todo.Input
    TodoId
    TodoDetail
    bool Completed
        -> Todo.Transformation
        -> Todo
```

Todo equality is owned by `TodoId`.

## Evolution

`Todo` implements:

```text
Evolvable<Todo, Todo.State>
```

Its accepted state carries:

```text
TodoId Id          creation-fixed
TodoDetail Detail  evolvable
bool Completed     evolvable
```

The HTTP PUT does not construct a replacement Todo directly. Work first reads the current Todo point and then proposes:

```csharp
todo.Update(state => state with
{
    Detail = detail,
    Completed = completed
})
```

Only an accepted evolved point is passed to `PointWriter`.

This keeps two statements separate:

```text
Todo.Transformation
    establishes a Todo

Todo.Evolution
    establishes an admissible next Todo state
```

## Identity generation

Creating a Todo does not accept an id from HTTP.

Work requires a focused `TodoIdGenerationIO` capability. The current Grounding realizes it through `Guid.NewGuid()` and then establishes the generated value as a semantic `TodoId`.

## CRUD semantics

The primitive point capabilities remain smaller than CRUD:

```text
Read
    PointReader

Create
    generate identity
    establish Todo
    read + write if absent

Update
    read current Todo
    Todo.Update(...)
    write accepted evolved point

Delete
    read + remove if present
```

The CRUD operations are available as:

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

The Grounding is in-memory on purpose. Replacing it with EF Core or another mechanism should only require different Grounding implementations; Work should remain unchanged.

## Example request

```http
POST /todos
Content-Type: application/json

{
  "detail": "feel the semantics",
  "completed": false
}
```
