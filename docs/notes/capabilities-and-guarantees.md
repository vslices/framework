# Capabilities and Guarantees

> Status: exploratory semantic model.
>
> This note records a direction that is strong enough to preserve, but is intentionally **not** an implementation commitment yet. The immediate priority remains producing useful migration/refactoring material for the September 25 delivery. Implementation, analyzers, laws, and VSIR syntax should resume only when a real case creates enough pressure.

## Motivation

VSlices currently models runtime requirements primarily through capabilities.

That is useful, but it is not sufficient to describe cases where Work needs not only that an operation exists, but that the realization of that operation preserves additional observable properties.

Examples include:

- tracking;
- atomic persistence;
- isolation;
- ordering;
- delivery semantics;
- durability;
- semantic consistency.

The working distinction is:

```text
Capability
    what Work can request

Guarantee
    an additional semantic constraint over an admissible realization

Grounding
    how the capability is realized while satisfying the required guarantees
```

A Feature should be able to require a capability and only the guarantees it actually depends on.

Grounding owns the mechanism used to satisfy them.

## Direction of authority

The intended direction is:

```text
Feature / Work
    declares required capabilities
    declares required guarantees

Grounding
    supplies a realization
    supplies evidence that the guarantees hold

Tooling / Ruleset
    verifies structural and semantic coherence
    without defining the semantic ceiling
```

A mechanism must not leak upward as the semantic requirement.

For example:

```text
Work requires:
    Tracking

Entity Framework grounding may realize it through:
    DbContext + ChangeTracker
```

Work should not require `EntityFrameworkCore.TrackAll`.

## Guarantees are not capabilities

A guarantee does not normally represent an independently useful operation.

For example, `Tracking` is not a useful capability by itself. It constrains a stateful persistence interaction.

Likewise, `Atomic` does not describe an operation. It constrains the observable semantics of a persistence boundary or another suitable capability.

This suggests treating guarantees as orthogonal refinements rather than creating a new capability type for every combination.

## Avoid pattern aliases as primitives

Traditional names such as:

- Repository;
- Store;
- Unit of Work;

carry substantial historical semantics.

Using those names as VSlices primitives risks importing assumptions that may not be supported by the available documentation or VSIR evidence.

VSlices should prefer smaller concepts whose meaning can be independently evidenced.

This is particularly important for the intended flow:

```text
documentation
    -> AI interpretation
    -> VSIR
    -> tooling
    -> generated code
```

A document may establish that a system **reads**, **manages**, **stages**, **persists**, or **coordinates** without containing enough evidence to infer an entire historical pattern.

Atomic concepts allow knowledge to accumulate across Structure, Behavior, Consistency, and other documents without requiring premature pattern recognition.

## Provisional persistence vocabulary

The following vocabulary is exploratory.

### Reading<A, ID>

Represents observable access to persisted `A`.

It does not imply mutation, persistence authority, commit, tracking, or transactions.

Examples of operations that may belong to this area include:

- read by identity;
- read many;
- existence;
- query.

### Managing<A, ID>

Represents persistent management intent over `A`.

Creation and update currently appear to belong to the same broad case because both express an intention to establish or change persistent state.

The persistence may be realized in different ways.

#### Outsourced persistence

The managing surface can stage a mutation, while another persistence authority materializes it.

Conceptually:

```text
Managing<A, ID>
    + staged persistence
    + external persistence boundary
```

#### Autonomous persistence

The managing surface owns materialization of its own mutations.

Conceptually:

```text
Managing<A, ID>
    + self persistence
```

The historical term "Store" may describe such a composition, but VSlices does not need to encode that name as a primitive.

### Removing<A, ID>

Deletion/removal remains an open question.

It is structurally similar to management with respect to persistence, but its semantics may differ materially:

- physical deletion;
- logical deletion;
- deactivation;
- archival;
- revocation;
- expiration;
- tombstoning.

Do not fold it into `Managing` only because CRUD traditionally groups them.

### Persistence context

A persistence context provides contextual access to persistence-related surfaces.

It does not necessarily own the persistence boundary.

A context exposing only reading surfaces is semantically complete without commit authority.

A context exposing staged mutations requires some reachable authority capable of materializing them.

### Persistence boundary

A persistence boundary owns the decision or mechanism by which staged persistence becomes materialized.

The presence of a commit-like operation is a capability.

The claim that all declared participants are coordinated by that operation is a guarantee.

Historical "Unit of Work" semantics may emerge from this composition, but should not be treated as a primitive merely because the pattern is familiar.

## Tracking

Tracking belongs to a stateful persistence boundary or equivalent interaction scope, not to a database in the abstract.

A database does not intrinsically guarantee tracking.

A repository-like surface does not intrinsically guarantee tracking either.

A concrete realization may satisfy tracking by maintaining identity and state across a shared interaction scope.

Entity Framework may use `DbContext` and `ChangeTracker` to realize this, but those are mechanisms rather than the semantic definition.

## Context exposure is not participation

An important distinction is:

```text
ExposedBy<Context>
    !=
ParticipatesIn<PersistenceBoundary>
```

A context may expose an autonomously persistent surface.

That does not imply that the surface participates in the context's external commit boundary.

This makes combinations such as a self-persisting surface exposed by a persistence boundary valid in principle.

The verifier must reason about explicit participation, not merely containment.

## Provisional semantic rules

These are candidate rules, not final syntax.

### Staged persistence requires a persistence path

```text
staged mutation
    => there must be a reachable persistence authority
```

If none exists, the semantics are incomplete.

### Self-persistence does not require an external boundary

```text
self-persisting mutation
    => external persistence boundary is not required
```

An external boundary may still coexist if it has a different scope.

### Reading does not require persistence authority

```text
Reading
    => no commit authority required
```

### A guarantee requires evidence, not a known mechanism

Do not encode rules such as:

```text
Atomic
    => same database transaction
```

or:

```text
Tracking
    => same DbContext
```

Those would confuse a known mechanism with the semantic ceiling.

Instead:

```text
same transactional boundary
    => sufficient evidence for Atomic

shared EF DbContext tracking scope
    => sufficient evidence for Tracking
```

Other mechanisms may establish the same guarantees.

## Unknown is not false

If VSlices cannot derive a guarantee through its built-in laws, the realization is not automatically invalid.

The intended distinction is:

```text
Proven
    VSlices can derive the guarantee from known laws.

Discharged
    the guarantee is established by explicit extension/evidence
    supplied by a grounding or ruleset.

Unresolved
    the guarantee is declared, but currently available evidence
    is insufficient to establish it.

Contradicted
    available semantics conflict with the declared guarantee.
```

`Unresolved` must not be collapsed into `Contradicted`.

A system may know something VSlices does not yet know.

## Extensible evidence

A grounding should be able to state that it knows a limit that the default ruleset cannot prove and supply the additional evidence or law necessary to discharge the obligation.

For example:

```text
Accounts -> PostgreSQL
Audit    -> external HTTP service

required guarantee:
    Atomic
```

VSlices must not reject this solely because the participants use different technologies.

A realization may implement a reconciliation protocol, outbox, idempotency, delayed completion, or another mechanism that satisfies the actual observable definition of the requested guarantee.

Whether that mechanism truly satisfies `Atomic` depends on the definition of `Atomic`, not on a technology whitelist.

The ruleset should allow such knowledge to extend the proof vocabulary rather than requiring suppression of a warning.

This follows the current VSlices principle:

```text
Tooling owns the rule language.
Rulesets own the rule vocabulary.

Built-ins bootstrap the language;
they do not define its semantic ceiling.
```

## Verification layers

Strong typing is useful, but cannot prove the behavior of a concrete grounding by itself.

The current direction separates three forms of evidence.

### Types

Types express structural requirements and declarations.

They can establish that:

- a Feature requires a capability;
- a runtime exposes that capability;
- a realization declares that it provides a guarantee.

They cannot prove arbitrary runtime behavior.

### Analyzer

An analyzer can detect semantic incompleteness or suspicious compositions.

Examples:

- staged mutation with no reachable persistence authority;
- a required guarantee with no available proof;
- conflicting scopes for staged and self-persisted mutation;
- impossible or contradictory declared participation.

Analyzer diagnostics should distinguish unresolved knowledge from contradiction.

### Laws

Laws provide executable behavioral evidence.

Examples may eventually include:

- tracking identity/state behavior within a boundary;
- persistence before/after commit;
- atomicity expectations;
- rollback behavior;
- ordering or delivery guarantees.

A grounding may contribute laws for guarantees that VSlices.Grounding does not know how to establish by default.

## Relationship to strong typing

The likely C# direction remains to let Work expose capability requirements through `Has*` constraints while preserving a service-owned contract.

For example, a service may define a persistence contract used by its Features, while a concrete Grounding implements it.

The exact generic shape is deliberately not fixed here.

Previous exploration considered forms analogous to:

```text
Has<service-owned-persistence-contract, RT>
```

with guarantees represented explicitly rather than by aliases such as `TrackingUnitOfWork`.

The important requirement is that the Feature should not need to know the concrete Grounding implementation.

## Why no implementation yet

This direction is promising, but it opens a large design surface:

- guarantee representation;
- capability scoping;
- persistence primitives;
- analyzer diagnostics;
- law registration;
- VSIR representation;
- extensible proof vocabulary;
- grounding evidence;
- code generation.

Entering that design surface now would compete directly with the immediate need to produce useful Serviu migration/refactoring material for September 25.

The current decision is therefore:

```text
preserve the semantic model
    -> do not implement it yet
    -> return to the concrete delivery
    -> resume when a real migration case creates pressure
```

This is not a rejection of the direction.

It is an explicit scope decision.
