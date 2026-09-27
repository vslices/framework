# Point Algebras

## Status

Point operations are currently modeled as executable algebraic capability atoms and composed into Work-module algebras used directly as `Flow` runtimes.

The validated point atoms are:

```text
PointReader<POINT, ID>
PointWriter<POINT>
PointRemover<POINT, ID>
```

No Free-monad program or operation interpreter is required by this model.

## Why atoms

A statement such as:

```text
read an Account point by AccountId
```

supports a small capability:

```csharp
PointReader<Account, AccountId>
```

It does not establish Repository, Store, tracking, transactions, Unit of Work, or persistence authority.

The smallest supported vocabulary preserves continuity from documentary evidence into executable Work without inventing a larger infrastructure pattern.

## Executable atoms

### PointReader

```csharp
public interface PointReader<POINT, ID>
{
    OptionT<IO, POINT> ReadOrDefault(ID id);

    IO<POINT> Read(ID id) => ...;
}
```

### PointWriter

```csharp
public interface PointWriter<POINT>
{
    IO<Unit> Write(POINT point);
}
```

### PointRemover

```csharp
public interface PointRemover<POINT, ID>
{
    IO<Unit> Remove(ID id);
}
```

`ReadOrDefault` is the primitive a Grounding must realize. It keeps absence explicit as `OptionT<IO, POINT>`.

`Read` is a default required-value view over that primitive. If the point is absent it fails exceptionally, matching the distinction between LINQ `Single` and `SingleOrDefault`. An implementation may declare its own `Read` when required lookup has more specific semantics.

The `IO` foundation keeps world contact explicit while leaving realization to Grounding.

## Work algebra

A module composes the atoms its Features may execute:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

Features then use that algebra directly:

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

The algebra is both:

- an explicit description of the executable operations available to the module;
- the runtime required by the module's Flows.

## Grounding

Grounding implements the atoms and assembles the algebra.

```csharp
public sealed class InMemoryTodoWork :
    PointReader<Todo, TodoId>,
    PointWriter<Todo>,
    PointRemover<Todo, TodoId>
{
    public TodoAlgebra Algebra { get; }
}
```

The `Algebra` property is currently an ordinary concrete export. No Framework trait is needed merely to state that a Grounding exposes an assembled algebra. The former `AlgebraIO<ALG>` marker was retired because nothing consumed it generically.

## Entity Framework Core

`EntityFrameworkPointIO<TContext, POINT, ID, TProjection>` directly implements the point atoms.

Its storage mappings and calls to EF Core are Grounding mechanisms. They do not add Repository semantics to Work.

The same component may be used both when:

```text
semantic point == EF entity
```

and when:

```text
semantic point != EF projection
```

## Algebra composition

If a Feature consumes Work owned by independent modules, its runtime can mix those executable algebras structurally:

```csharp
AlgebraMix<FileAlgebra, TodoAlgebra>
```

`AlgebraMix` deliberately states only that the child algebras are available together for projection. It does not import the coproduct semantics of the earlier Free instruction model or claim a stronger mathematical construction than current evidence requires.

The child Flow is lifted into that larger runtime by runtime projection:

```csharp
AddFile.Get()
    .MapRuntime(
        (AlgebraMix<FileAlgebra, TodoAlgebra> mix) => mix.A)
```

This is a contravariant adaptation of the Flow runtime: the parent runtime knows how to provide the smaller runtime required by the child.

Requests are adapted separately with `MapRequest`.

## Current executable evidence

The experiment currently exercises:

1. point atoms directly through a Feature Flow;
2. a seven-way `AlgebraMix`;
3. temporal capability atoms as a Flow runtime;
4. a CRUD SampleWorkflow using one module algebra;
5. same-module Feature composition;
6. cross-service BFF composition using `AlgebraMix<FileAlgebra, TodoAlgebra>`;
7. in-memory Groundings;
8. Entity Framework Core point Grounding.

The model intentionally does not claim transaction, durability, isolation, tracking, query, enumeration or compensation semantics.
