# Simple Workflow

This example is a small CRUD API used to pressure the current VSlices Work model.

The current executable relationship is:

```text
Feature<ALG, Request, Response>
    Describe(request)
        -> Free<ALG, Response>
```

`Describe` builds an inert description of Work. It does not execute, interpret, lower, or ground that Work.

## Structure

```text
SampleWorkflow.Spaces
    TodoId
    TodoDetail
    Todo : Evolvable<Todo, Todo.State>

SampleWorkflow.Work.Algebras
    TodoAlgebra
        NextTodoId
        ReadTodo
        WriteTodo
        RemoveTodo

SampleWorkflow.Work
    CreateTodo
    GetTodo
    UpdateTodo
    DeleteTodo
    AddAttachmentReference

    each Feature describes a different program in TodoAlgebra

SampleWorkflow.Grounding
    InMemoryTodoWork
        AlgebraIO<TodoAlgebra>

SampleWorkflow.Process
    CreateAndGetTodo
        composes CreateTodo + GetTodo directly
        because both already speak TodoAlgebra

SampleWorkflow.Api
    HTTP presentation
    DI supplies AlgebraIO<TodoAlgebra>
```

## Module-owned algebra

The Todo module owns its default Work vocabulary:

```text
TodoAlgebra
    NextTodoId
    ReadTodo
    WriteTodo
    RemoveTodo
```

Features do not need to define a new algebra merely because they are separate use cases.

Instead:

```text
CreateTodo
    uses NextTodoId + ReadTodo + WriteTodo

GetTodo
    uses ReadTodo

UpdateTodo
    uses ReadTodo + WriteTodo

DeleteTodo
    uses ReadTodo + RemoveTodo
```

The algebra describes what the module can express. The individual Free program reveals which subset a particular Feature actually uses.

A Feature-specific algebra remains valid when a real case requires a smaller or semantically distinct language. It is a specialization rather than the default ownership rule.

## Feature as a Work description

A Feature does not own an ambient runtime and does not execute Work directly.

For example:

```csharp
public sealed class GetTodo :
    Feature<
        TodoAlgebra,
        GetTodo.Request,
        GetTodo.Response>
{
    public static Free<TodoAlgebra, Response> Describe(Request request) =>
        from todo in TodoAlgebra.Read(request.Id)
        select new Response(todo);
}
```

The Feature speaks the module language directly. It does not need to know the generic `PointReader` lifting machinery used to construct that language.

Conceptually:

```text
Feature
    owns the Work description

TodoAlgebra
    owns the module vocabulary

AlgebraIO<TodoAlgebra>
    owns interpretation boundary

Grounding
    owns concrete realization
```

## Same-module composition

`CreateAndGetTodo` demonstrates that Features sharing one module language compose without an algebra sum:

```csharp
from created in CreateTodo.Describe(...)
from read in GetTodo.Describe(...)
select ...
```

Both child descriptions are already:

```text
Free<TodoAlgebra, ...>
```

so no hoist is required.

This gives `AlgebraSum` a narrower role: composing genuinely different languages rather than routinely composing use cases from the same module.

## Cross-language composition

When Work crosses module or service vocabularies, `AlgebraSum` remains useful.

For example the SampleBFF composes:

```text
AddFile.Algebra
+
TodoAlgebra
```

through:

```text
AlgebraSum<AddFile.Algebra, TodoAlgebra>
```

The positional A..G structure is mechanism. It does not imply semantic priority.

## Interpretation

The default in-memory realization is:

```text
InMemoryTodoWork
    AlgebraIO<TodoAlgebra>
```

The same `TodoAlgebra` can later have different Groundings:

```text
InMemory
Entity Framework Core
direct SQL
Dapper
ADO.NET
or another realization
```

without changing Feature semantics.

## CRUD API

```text
POST   /todos
GET    /todos/{id}
PUT    /todos/{id}
DELETE /todos/{id}
```

`TodoDetail` covers the semantic string. `Completed` remains a plain `bool`.

`Todo` is `Evolvable<Todo, Todo.State>`; PUT reads the current point, proposes the next state with `Todo.Update(...)`, and writes only the accepted evolved point.

## Performance baseline

The maintained performance baseline executes the current `UpdateTodo` end-to-end against a real PostgreSQL 17 instance:

```text
UpdateTodo.Describe
    -> Free<TodoAlgebra, Response>
    -> AlgebraIO<TodoAlgebra>
    -> EntityFrameworkPointIO
    -> Npgsql
    -> PostgreSQL
```

This baseline exists to compare future implementation and Work-decomposition experiments without changing the meaning of the current Feature.
