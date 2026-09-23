# Capabilities

## What is a Capability?

A capability is a typed description of an operation that Work may require.

In the current model, capabilities contribute vocabulary to a Feature-owned algebra.

Examples include:

- `PointReader<ALG, POINT, ID>`;
- `PointWriter<ALG, POINT>`;
- `PointRemover<ALG, POINT, ID>`;
- `Clock<ALG>`;
- `Delay<ALG>`.

A capability says what the WorkFlow can express.

It does not select the concrete mechanism that realizes the operation.

## What a Capability is not

A capability is not:

- a service layer object;
- a helper class;
- a manager;
- a static singleton;
- an ambient runtime service;
- a concrete infrastructure client;
- a guarantee.

## Main rule

A Feature algebra should implement only the capability vocabulary that its WorkFlow actually requires.

For example:

```text
GetTodo.Algebra
    PointReader<Todo, TodoId>

DeleteTodo.Algebra
    PointReader<Todo, TodoId>
    PointRemover<Todo, TodoId>

TemporalFeature.Algebra
    Clock
    Delay
```

The Feature then returns:

```text
Free<ALG, RES>
```

No `RT`, `Has*`, or runtime carrier is required at the Feature boundary.

## Grounding

Grounding owns concrete realization.

A Feature-specific `AlgebraIO<ALG>` interprets its WorkParts and may delegate to smaller reusable Grounding contracts.

Examples:

```text
PointReaderIO<POINT, ID>
PointWriterIO<POINT>
PointRemoverIO<POINT, ID>

ClockIO
DelayIO
```

Concrete realizations currently include:

- `EntityFrameworkPointIO`;
- `SystemTimeIO`;
- in-memory sample interpreters.

This keeps:

```text
capability
    what Work can request

Grounding contract
    reusable shape of external realization

concrete mechanism
    how this environment performs it
```

separate.

## Why

This allows:

- WorkFlows whose vocabulary is explicit in their algebra;
- inert `Free` programs before external execution;
- alternate Groundings for the same Feature;
- smaller dependency surfaces;
- composition without an ambient service bag;
- clearer separation between semantic operations and technical mechanisms.

## Preferred style

Prefer:

- small capability vocabulary;
- Feature-owned algebras;
- explicit WorkParts;
- `Free<ALG, A>` composition;
- `AlgebraIO<ALG>` interpretation;
- reusable Grounding contracts only when real implementations benefit from them.

Avoid:

- broad runtime carriers;
- `Has*` constraints as dependency bags;
- constructor-injected Feature services;
- Repository/Store/UnitOfWork aliases inferred from CRUD;
- hidden infrastructure access;
- global mutable access patterns.

## Point capabilities

The validated point vocabulary is:

- `PointReader<ALG, POINT, ID>`;
- `PointWriter<ALG, POINT>`;
- `PointRemover<ALG, POINT, ID>`.

These operations are independently expressible and can be realized by Entity Framework Core without introducing Repository semantics.

See [Point Algebras](point-algebras.md).

## Temporal capabilities

Clock observation and waiting are intentionally separate:

- `Clock<ALG>` observes the current semantic `Moment`;
- `Delay<ALG>` waits for a semantic `Duration<double>` or until a `Moment`.

One `SystemTimeIO` may realize both through `TimeProvider`, but that shared mechanism does not make them one semantic capability.

Scheduling, recurring invocation, and host lifecycle are separate unresolved concerns.

## Capabilities and guarantees

Capabilities describe what Work can request.

Guarantees describe additional semantic properties that an admissible realization must preserve.

Examples of potential guarantees include:

- tracking;
- atomicity;
- isolation;
- ordering;
- durability;
- delivery semantics.

A capability must not imply those guarantees merely because one Grounding happens to provide them.

See [Capabilities and Guarantees](notes/capabilities-and-guarantees.md).
