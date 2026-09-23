# Simple Workflow

This example is intentionally a very small CRUD API built with the current VSlices semantic-space and point-algebra model.

The concept is `Todo` and is split into the current architecture:

```text
SampleWorkflow.Spaces
    TodoId
    TodoDetail
    Todo

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

`TodoDetail` is a separate semantic space rather than treating a Todo as an incidental tuple of primitive values:

```text
TodoDetail.Input
    Title
    Completed
        -> TodoDetail.Transformation
        -> TodoDetail
```

`Todo` is then established from already-semantic values:

```text
Todo.Input
    TodoId
    TodoDetail
        -> Todo.Transformation
        -> Todo
```

Todo equality is owned by `TodoId`. Two Todo instances with the same semantic identity represent the same Todo point even when their detail differs.

## Identity generation

Creating a Todo does not accept an id from HTTP.

Work requires a focused `TodoIdGenerationIO` capability. The current Grounding realizes it through `Guid.NewGuid()` and then establishes the generated value as a semantic `TodoId`.

This keeps two different statements separate:

```text
TodoId.Transformation
    defines how a Guid becomes a valid TodoId

GuidTodoIdGeneration
    defines one concrete mechanism for obtaining source Guids
```

The CreateTodo Feature composes both:

```text
TodoDetail
    -> obtain TodoId
    -> Todo.Transformation
    -> TodoPrograms.Create
```

## CRUD semantics

The primitive point capabilities deliberately remain smaller than CRUD:

```text
Read
    PointReader

Create
    generate identity
    establish Todo
    read + write if absent

Update
    establish replacement Todo
    read + write if present

Delete
    read + remove if present
```

That means `Create` and `Update` are Feature semantics built from semantic construction plus the same `PointWriter` capability rather than separate persistence primitives.

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
  "title": "feel the semantics",
  "completed": false
}
```
