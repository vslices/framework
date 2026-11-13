# Point Algebras

## Status

Point operations are currently modeled as executable algebraic capability atoms and composed into Work-module runtimes. Each Feature declares a minimal `Algebra` interface implemented by the concrete module runtime.

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

All algebra atoms and their capability projections live in `VSlices.Work.Algebra`. Each `HasPoint*` interface is declared alongside its corresponding `Point*` atom, rather than in a separate catch-all capabilities file. Consumers must import `using VSlices.Work.Algebra;`.

The temporal atoms `ClockIO` and `DelayIO` are colocated in `src/VSlices.Work/Algebra/Temporal.cs` under the same namespace; `AlgebraMix` also belongs to that namespace.

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

`ReadOrDefault` is the primitive a Grounding must realize. It keeps absence explicit as `OptionT<IO, POINT>` and carries an at-most-one law: if the realization can observe more than one point for the same identity, it should fail the effect rather than choose one. This completes the intended `SingleOrDefault`-like semantics.

`Read` is a default required-value view over that primitive. If the point is absent it fails exceptionally, matching the required-presence contract and the LINQ `Single` versus `SingleOrDefault` distinction. This is intentional; a new catchable error category or `@catch` is not required solely to represent that absence. An implementation may declare its own `Read` when required lookup needs more specific failure or retrieval semantics, but the override should preserve the meaning that the point is required to exist.

Because default interface members are not exposed as ordinary instance members on a concrete implementing type, `PointReaderExtensions.Read` forwards concrete readers to the trait member. This lets a Grounding implement only `ReadOrDefault` and still call `grounding.Read(id)`. If the concrete reader declares its own `Read`, normal C# member resolution gives that specialized method precedence while interface dispatch preserves the override.

The `IO` foundation keeps world contact explicit while leaving realization to Grounding.

## Work algebra

A module composes the atoms its Features may execute, while each Feature declares only the atoms it needs through the reusable `HasPoint*` capability projections:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids) : GetTodo.Algebra;
```

Features then use that algebra directly:

```csharp
public sealed class GetTodo :
    Feature<GetTodo, GetTodo.Algebra, GetTodo.Request, GetTodo.Response>
{
    public interface Algebra : HasPointReader<Todo, TodoId> { }

    public sealed record Request(TodoId Id);
    public sealed record Response
    {
        private Response(Option<Todo> todo) => Todo = todo;
        public Option<Todo> Todo { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Option<Todo> todo);

    public static Eff<Algebra, Response> Get(Request request) =>
        Eff<Algebra, Response>.LiftIO(algebra =>
            algebra.Reader.ReadOrDefault(request.Id).Run().As()
                .Map(NewResponse));
}
```

The concrete module algebra assembles the operations available to the module and implements its Features' capability interfaces. Each Feature effect requires only its own `Algebra` view, not the entire concrete runtime. `HasPointReader<POINT, ID>`, `HasPointWriter<POINT>` and `HasPointRemover<POINT, ID>` live in `VSlices.Work.Algebra` and expose the corresponding point atoms through `Reader`, `Writer` and `Remover` properties.

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

Child Features receive their requests explicitly, and the parent projects the appropriate algebra when executing their effects:

```csharp
AddFile.Get(new AddFile.Request(name, content)).RunIO(mix.A)
```

No Flow `MapRuntime` or `MapRequest` operation is required in the Feature path.

## Current executable evidence

The experiment currently exercises:

1. point atoms directly through a Feature Eff;
2. a seven-way `AlgebraMix`;
3. temporal capability atoms as a Feature Eff runtime;
4. a CRUD SampleWorkflow using one module algebra;
5. same-module Feature composition;
6. cross-service BFF composition using `AlgebraMix<FileAlgebra, TodoAlgebra>`;
7. in-memory Groundings;
8. Entity Framework Core point Grounding.

The model intentionally does not claim transaction, durability, isolation, tracking, query, enumeration or compensation semantics.
