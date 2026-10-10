# Point Algebras

## Status

Point algebras are the current validated model for Work capabilities that operate on points of semantic spaces.

Executable evidence now demonstrates that:

- atomic point capabilities can be composed by a Feature-owned algebra;
- one algebra can span multiple semantic spaces;
- `Free<ALG, A>` describes Work without executing it;
- the same Feature program can be interpreted by different Groundings;
- `PointReader`, `PointWriter`, and `PointRemover` can be expressed independently;
- concrete Groundings can realize those point capabilities without introducing a Repository abstraction;
- Entity Framework Core can realize point reading, writing, and removal while remaining a Grounding mechanism rather than Work vocabulary.

Guarantees are intentionally outside this model for now. They remain a separate semantic layer.

## Motivation

VSlices models domain meaning through semantic spaces. Concrete values are points in those spaces.

When Work needs external operations involving those points, the framework should describe the smallest operation that is actually known rather than infer a historical infrastructure pattern such as Repository, Store, DAO, or Unit of Work.

For example, documentation that establishes that an Account can be read by identity supports a small statement:

```text
read a point of Account by AccountId
```

That can lower to a `PointReader` capability. It does not by itself establish a Repository pattern, a persistence boundary, tracking, transactions, or any particular storage mechanism.

This is especially important for the intended continuity pipeline:

```text
documentation
    -> AI interpretation
    -> VSIR
    -> generated Work vocabulary
    -> Grounding
```

Atomic operations require less inferred knowledge and are easier to trace back to documentary evidence.

## Point capabilities

A point capability contributes one kind of operation to an algebra.

The current validated vocabulary is:

```text
PointReader
PointWriter
PointRemover
```

These capabilities describe instructions available to Work. They do not determine how those instructions are realized.

### PointReader

`PointReader<ALG, POINT, ID>` states that an algebra can express reading a point of `POINT` by `ID`.

```csharp
public interface PointReader<ALG, POINT, ID>
    where ALG : Functor<ALG>, PointReader<ALG, POINT, ID>
{
    static abstract K<ALG, Option<POINT>> Read(ID id);
}
```

The framework constructor lifts that operation into:

```csharp
Free<ALG, Option<POINT>>
```

Reading does not imply writing, persistence authority, tracking, transactions, or any specific Grounding.

### PointWriter

`PointWriter<ALG, POINT>` states that an algebra can express writing a point.

```csharp
public interface PointWriter<ALG, POINT>
    where ALG : Functor<ALG>, PointWriter<ALG, POINT>
{
    static abstract K<ALG, Unit> Write(POINT point);
}
```

A write describes the operation. It does not by itself define when or under which guarantees that write becomes durable.

Creation and update do not need separate primitive capabilities at this level. Establishing a new point and replacing the externally represented state of an existing point are both expressible as writing a point.

A Grounding may need an identity function or another storage-specific mechanism to realize that behavior. That mechanism belongs to Grounding rather than changing the Work vocabulary.

### PointRemover

`PointRemover<ALG, POINT, ID>` states that an algebra can express removing a point by identity.

```csharp
public interface PointRemover<ALG, POINT, ID>
    where ALG : Functor<ALG>, PointRemover<ALG, POINT, ID>
{
    static abstract K<ALG, Unit> Remove(ID id);
}
```

Removal is distinct from writing in the current vocabulary because real CRUD pressure required an independently expressible operation.

Its existence still does not imply rollback, durability, atomicity, or any wider Repository semantics.

## Feature-owned algebras

A Feature composes only the point capabilities its WorkFlow requires.

For example:

```text
GetTodo.Algebra
    PointReader<Todo, TodoId>

UpdateTodo.Algebra
    PointReader<Todo, TodoId>
    PointWriter<Todo>

DeleteTodo.Algebra
    PointReader<Todo, TodoId>
    PointRemover<Todo, TodoId>
```

The algebra is a signature of operations available to that WorkFlow. It is not the concrete execution mechanism.

The implementation of `Functor.Map` and the operation cases is substantially mechanical and remains a strong candidate for future Tooling generation.

## Free WorkFlows

Point capability constructors return `Free<ALG, A>`.

This allows a Feature to describe a program such as:

```text
read Account
-> transform the semantic point
-> write Account
-> read Account
```

without performing external effects while the program is being built.

The current Feature boundary is:

```text
Feature == WorkFlow == Free<ALG, Response>
```

The Feature owns the program directly. It does not require a parallel Repository or Programs layer merely to describe persistence-oriented work.

## Algebra interpretation

A complete Feature algebra is interpreted through:

```csharp
public interface AlgebraIO<ALG>
    where ALG : Functor<ALG>
{
    IO<A> Interpret<A>(K<ALG, A> operation);
}
```

`FreeAlgebra.interpret` folds the inert WorkFlow through this interpreter.

`HasAlgebra<ALG, RT>` and `AlgebraEnv<ALG, RT>` were removed together with the retired Flow/runtime-carrier surface.

The maintained execution boundary is direct:

```text
Feature.Describe(request)
    -> Free<ALG, RES>
    -> AlgebraIO<ALG>
    -> IO<RES>
```

A future host or invocation mechanism may automate interpreter selection, but it must not reintroduce an ambient runtime merely to recover this wiring.

## Point Grounding contracts

Reusable Groundings can realize point operations independently of a particular Feature algebra.

The current contracts are:

```csharp
PointReaderIO<POINT, ID>
PointWriterIO<POINT>
PointRemoverIO<POINT, ID>
```

These contracts express concrete world contact in `IO`.

A Feature-specific `AlgebraIO<ALG>` can delegate its WorkParts to one or more of these point Groundings.

Conceptually:

```text
Feature-owned algebra
    ReadTodoPart
    WriteTodoPart
    RemoveTodoPart
        |
        v
AlgebraIO<Feature.Algebra>
        |
        +--> PointReaderIO<Todo, TodoId>
        +--> PointWriterIO<Todo>
        +--> PointRemoverIO<Todo, TodoId>
```

This keeps two responsibilities separate:

```text
Feature algebra
    owns the Work vocabulary

Point Grounding
    owns reusable realization of atomic point operations
```

## Entity Framework Core Grounding

`EntityFrameworkPointIO<TContext, POINT, ID, TProjection>` realizes:

```text
PointReaderIO<POINT, ID>
PointWriterIO<POINT>
PointRemoverIO<POINT, ID>
```

against an Entity Framework Core projection.

It receives explicit mappings for:

- semantic point -> persistence projection;
- persistence projection -> semantic point;
- semantic point -> identity;
- identity -> storage predicate.

This allows the Grounding to support both:

```text
semantic point == EF entity
```

and:

```text
semantic point != EF projection
```

without exposing `DbContext`, tracking, or Repository semantics to Work.

The current `Write` realization checks whether the identified projection already exists and uses EF add/update mechanisms accordingly. This is an implementation mechanism for the observed PointWriter semantics, not a new Create/Update distinction in Work.

The current implementation performs `SaveChanges` during each write/remove realization. That is a property of this Grounding implementation. It must not be generalized into a universal durability or transaction guarantee of `PointWriter` or `PointRemover`.

## Why Repository was removed

The previous transitional surface exposed:

```text
Repository<A, ID>
    Create
    Read all
    Read by id
    Any
    Update
    Delete
```

That surface bundled operations merely because they are commonly grouped by a historical pattern.

The current Work model has stronger evidence for smaller vocabulary:

```text
need to read one point
    -> PointReader

need to establish external point state
    -> PointWriter

need to remove one point
    -> PointRemover
```

If a future Feature needs enumeration, existence probing, querying, staged mutation, or another persistence-oriented operation, that capability should be introduced from the actual pressure rather than restored implicitly through Repository.

## What this model intentionally does not say

Point algebras and point Groundings do not currently define:

- tracking;
- atomicity;
- isolation;
- durability;
- staged versus autonomous persistence;
- transaction boundaries;
- enumeration/query semantics;
- a Repository / Store / Unit of Work taxonomy.

Those questions are not rejected. They are separate questions whose answers require guarantees, laws, analyzers, or additional capability vocabulary.

See [Capabilities and Guarantees](notes/capabilities-and-guarantees.md).

## Validation evidence

Executable tests now demonstrate that:

1. building a Feature `Free` program does not execute point operations;
2. the same Feature can be interpreted by different Groundings;
3. one Feature-owned algebra can compose multiple point capabilities;
4. point reading, writing, and removal are independently expressible;
5. an Entity Framework Core Grounding can realize those operations without Repository;
6. both direct EF entities and separate persistence projections can represent semantic points;
7. `PointWriter` can establish a missing point and replace the represented state of an existing point;
8. a Feature-owned algebra can delegate its point WorkParts to the reusable EF point Grounding.

This is sufficient evidence to remove Repository and DatabaseIO from the current Work surface while leaving stronger persistence guarantees open to future pressure.
