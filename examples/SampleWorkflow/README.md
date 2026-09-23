# Simple Workflow

This example is intentionally a very small CRUD API built with the current VSlices point-algebra model.

The concept is `Todo` and is split into the surfaces requested by the current architecture:

```text
SampleWorkflow.Spaces
    Todo
    TodoId

SampleWorkflow.Work
    TodoAlgebra
    PointReader<Todo>
    PointWriter<Todo>
    PointRemover<Todo>
    CRUD Features

SampleWorkflow.Grounding
    InMemoryTodoAlgebra

SampleWorkflow.Api
    HTTP presentation + runtime composition
```

## Why the API supplies the id

The POST request supplies the `Guid` intentionally.

Generating identity would introduce another capability and would make this example test two ideas at once. The example is only pressure for point capabilities, free algebras, Features, Grounding, and presentation.

## CRUD semantics

The primitive capabilities deliberately remain smaller than CRUD:

```text
Read
    PointReader

Create
    read + write if absent

Update
    read + write if present

Delete
    read + remove if present
```

That means `Create` and `Update` are Feature semantics built from the same `PointWriter` capability rather than separate persistence primitives.

The CRUD operations are available as:

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

The Grounding is in-memory on purpose. Replacing it with EF Core or another mechanism should only require a different `AlgebraIO<TodoAlgebra>` interpreter and host composition; Work should remain unchanged.

## Example request

```http
POST /todos
Content-Type: application/json

{
  "id": "11111111-1111-1111-1111-111111111111",
  "title": "feel the semantics",
  "completed": false
}
```
