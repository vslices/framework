# Capabilities

## Current meaning

A Work capability is the smallest executable operation for which current evidence justifies an independent contract.

Capabilities are not service classes, managers, repositories, DI bags, or broad infrastructure abstractions.

Current examples include:

```text
PointReader<POINT, ID>
PointWriter<POINT>
PointRemover<POINT, ID>
ClockIO
DelayIO
```

These are **executable algebraic capability atoms**.

## Algebra

A Work module composes the atoms it exposes into a simple algebra value.

For example:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

The algebra is the runtime of Features owned by that Work module:

```text
Feature
    -> Flow<TodoAlgebra, Request, Response>
```

This gives the runtime a structural meaning: it is the algebra of executable operations available to that module, rather than an arbitrary collection of dependencies.

## Grounding

Grounding implements the atoms.

A grounding can expose a complete executable algebra through:

```csharp
public interface AlgebraIO<ALG>
{
    ALG Algebra { get; }
}
```

For example:

```text
InMemoryTodoWork
    implements PointReader<Todo, TodoId>
    implements PointWriter<Todo>
    implements PointRemover<Todo, TodoId>
    implements TodoIdSource
    exposes TodoAlgebra
```

A different Grounding may expose the same Work algebra through different mechanisms.

## Composition

A consumer that needs multiple independently owned algebras composes them structurally:

```csharp
AlgebraSum<FileAlgebra, TodoAlgebra>
```

Child Flows are adapted by projection:

```csharp
AddFile.Get()
    .MapRuntime(
        (AlgebraSum<FileAlgebra, TodoAlgebra> sum) => sum.A)
```

This makes the relationship explicit:

```text
larger algebra -> child algebra
```

Request adaptation is separate:

```csharp
.MapRequest(
    (ParentRequest request) =>
        new ChildRequest(...))
```

## Point capability atoms

The current point vocabulary is intentionally small:

```csharp
public interface PointReader<POINT, ID>
{
    IO<Option<POINT>> Read(ID id);
}

public interface PointWriter<POINT>
{
    IO<Unit> Write(POINT point);
}

public interface PointRemover<POINT, ID>
{
    IO<Unit> Remove(ID id);
}
```

Reading, writing and removal do not imply Repository, tracking, transactions, atomicity, durability, enumeration, or Unit of Work semantics.

Introduce new atoms only from real pressure.

## Capabilities and guarantees

Capabilities answer:

```text
what operation can Work execute?
```

Guarantees answer different questions:

```text
what properties must an admissible realization preserve?
```

Atomicity, isolation, ordering, durability and similar properties remain separate from the capability vocabulary until evidence supports a guarantee model.
