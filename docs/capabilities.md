# Capabilities

## What is a Capability?

A Capability is a typed description of an operation that Work may express.

In the current Work model, capabilities contribute vocabulary to a Feature-owned algebra.

Examples include:

- reading a semantic point;
- writing a semantic point;
- removing a semantic point;
- observing the current moment;
- delaying for a duration or until a moment.

A capability says what the WorkFlow can request.

It does not say how the request is realized.

## What a Capability is not

A Capability is not:

- a service layer object;
- a helper class;
- a manager;
- a static singleton;
- a dependency injected into a Feature;
- an ambient runtime service;
- a guarantee such as atomicity or durability.

## Main Rule

A Feature algebra should implement only the capability vocabulary required by that Feature.

For example:

```text
GetTodo.Algebra
    PointReader<Todo, TodoId>

UpdateTodo.Algebra
    PointReader<Todo, TodoId>
    PointWriter<Todo>

TemporalProbe.Algebra
    Clock
    Delay
```

The Feature then builds its WorkFlow as:

```text
Free<ALG, Response>
```

There is no separate runtime capability carrier in the Feature contract.

## Grounding

Grounding owns concrete world contact.

A complete Feature algebra is interpreted through:

```csharp
AlgebraIO<ALG>
```

Reusable Groundings may expose smaller realization contracts when useful.

Current examples include:

```text
PointReaderIO<POINT, ID>
PointWriterIO<POINT>
PointRemoverIO<POINT, ID>

ClockIO
DelayIO
```

A Feature-specific `AlgebraIO<ALG>` can delegate WorkParts to these reusable Groundings.

## Point capabilities

The current validated point vocabulary is:

- `PointReader<ALG, POINT, ID>`;
- `PointWriter<ALG, POINT>`;
- `PointRemover<ALG, POINT, ID>`.

These capabilities intentionally do not imply Repository, Store, Unit of Work, tracking, transactions, or persistence guarantees.

Entity Framework Core currently realizes these operations through `EntityFrameworkPointIO`.

See [Point Algebras](point-algebras.md).

## Temporal capabilities

Temporal Work currently distinguishes:

- `Clock<ALG>`: observe the current semantic `Moment`;
- `Delay<ALG>`: delay by `Duration<double>` or until a `Moment`.

Grounding distinguishes:

- `ClockIO`;
- `DelayIO`.

`SystemTimeIO` can realize both through one `TimeProvider`.

This shared realization does not collapse clock observation, waiting, scheduling, recurring invocation, or host lifecycle into one semantic capability.

## Capability composition

Capabilities compose inside the owning algebra.

Feature programs compose through ordinary `Free` composition.

When a Feature reuses another Feature WorkFlow whose algebra is different, the current mechanism is `AlgebraSum<A,...,G>` plus hoisting.

This means capability composition does not require a shared runtime object or a broad service algebra.

## Capabilities and Guarantees

Capabilities describe what Work can request.

Guarantees describe additional semantic properties that an admissible realization must preserve.

Examples of possible guarantees include:

- tracking;
- atomicity;
- isolation;
- ordering;
- durability;
- delivery semantics.

A concrete Grounding may happen to provide some of these properties through its mechanism. That does not make the guarantee implicit in the capability.

See [Capabilities and Guarantees](notes/capabilities-and-guarantees.md).

## Historical note

Earlier iterations represented capability requirements through runtime constraints such as:

```text
HasClock<RT>
HasDatabase<RT>
HasAlgebra<ALG, RT>
```

and executed Features through `Flow<RT, REQ, RES>`.

Those carriers have been removed from the current Feature path.

Their useful property was explicit dependency information. The current model preserves that information more locally in the Feature algebra without retaining an ambient runtime parameter.
