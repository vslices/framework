# Point Algebras

## Status

Point algebras are the current validated model for Work capabilities that operate on points of semantic spaces.

This model was adopted after an executable experiment demonstrated that:

- atomic point capabilities can be composed by a service-owned algebra;
- one algebra can span multiple semantic spaces;
- `Free<ALG, A>` can describe programs without executing them;
- the same program can be interpreted by different Groundings;
- a Feature can require the algebra through `RT` and execute the free program inside the existing `Flow` model.

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

The first validated capabilities are:

```text
PointReader
PointWriter
```

Deletion/removal is deliberately not part of the current vocabulary yet. Its semantics need additional real pressure before deciding whether it belongs to writing or deserves a distinct capability.

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

Creation and update do not need separate primitive capabilities at this level: establishing a new point and overwriting the externally represented state of an existing point are both expressible as writing a point. If a future case requires a stronger semantic distinction, the vocabulary can grow from that evidence.

## Service-owned algebras

A service or domain Work surface composes the point capabilities it actually needs into its own algebra.

Conceptually:

```text
Account space
Role space

AppAlgebra
    PointReader<Account, AccountId>
    PointWriter<Account>
    PointReader<Role, RoleId>
```

In C#:

```csharp
public sealed class AppAlgebra :
    Functor<AppAlgebra>,
    PointReader<AppAlgebra, Account, AccountId>,
    PointWriter<AppAlgebra, Account>,
    PointReader<AppAlgebra, Role, RoleId>
{
    // operation constructors + Functor.Map
}
```

The algebra is a signature of operations known by Work. It is not the concrete execution mechanism.

The implementation of `Functor.Map` is mechanical over the operation cases and is therefore a strong candidate for future Tooling generation rather than handwritten application logic.

## Free programs

Point capability constructors return `Free<ALG, A>`.

This gives Work an inert description of a program:

```text
read Account
-> read Role
-> transform points
-> write Account
-> read Account
```

Constructing the program performs no external effect.

The program can be composed with ordinary monadic syntax and interpreted later.

A Feature is not replaced by `Free`. The current execution model remains:

```text
Feature
    -> Flow<RT, REQ, RES>
```

A Feature may construct a free algebra program as part of its Work and ask its runtime to interpret that program.

## Runtime requirement

A Feature that uses an algebra declares that requirement through `RT`:

```csharp
where RT : HasAlgebra<AppAlgebra, RT>
```

This means the runtime can provide an interpreter for the service-owned algebra.

The Feature does not know whether the Grounding is Entity Framework, an HTTP service, memory, an event store, or another mechanism.

## Grounding and interpretation

A current Grounding supplies:

```csharp
AlgebraIO<ALG>
```

which interprets one algebra operation into `IO`.

```csharp
public interface AlgebraIO<ALG>
    where ALG : Functor<ALG>
{
    IO<A> Interpret<A>(K<ALG, A> operation);
}
```

`FreeAlgebra.interpret` folds the complete free program through that interpreter.

`AlgebraEnv<ALG, RT>` connects this interpreter to the runtime capability model and exposes the resulting `Eff<RT, A>` to Work.

The current `IO` target is intentionally concrete. It matches the existing VSlices execution model and current evidence. Generalization to arbitrary target monads or monad-transformer stacks should happen only if a real use case requires it.

## Feature integration

A Feature remains a normal `Flow` while using a free point algebra:

```csharp
public sealed class RenameAccount<RT> :
    Feature<RenameAccount<RT>, RT, Request, Response>
    where RT : HasAlgebra<AppAlgebra, RT>
{
    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<AppAlgebra, RT>
            .run(AppPrograms.Rename(request.AccountId, request.Name))
            .Map(account => new Response(account)));
}
```

This preserves the existing Feature boundary while separating:

```text
Space
    what points mean

Algebra
    what operations Work can express over points

Free program
    how those operations are composed for an intention

Grounding
    how those operations are realized
```

## What this model intentionally does not say

Point algebras do not currently define:

- tracking;
- atomicity;
- isolation;
- durability;
- staged versus autonomous persistence;
- transaction boundaries;
- deletion semantics;
- a Repository / Store / Unit of Work taxonomy.

Those questions are not rejected. They are separate questions whose answers require guarantees, laws, analyzers, or additional capability vocabulary.

See [Capabilities and Guarantees](notes/capabilities-and-guarantees.md).

## Validation evidence

The initial implementation is covered by executable tests that demonstrate:

1. building a `Free` program does not execute its point operations;
2. the same program can be interpreted by different Groundings;
3. one service-owned algebra can compose point capabilities over multiple spaces;
4. the algebra can be required through `RT`;
5. the interpreted program composes inside the current `Feature -> Flow` execution model.

This is sufficient evidence to adopt the capability substrate while leaving guarantees and additional point operations open to future pressure.
