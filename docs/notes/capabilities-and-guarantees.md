# Capabilities and Guarantees

> Status: capability substrate partially validated; guarantees remain exploratory.
>
> The Point Algebra experiment validated and implemented the capability side for reading and writing semantic points. Guarantee representation, analyzers, laws, and guarantee-oriented VSIR syntax remain intentionally deferred while the immediate priority is producing useful migration/refactoring material for the September 25 delivery.

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

## Validated capability substrate

The earlier provisional vocabulary based on `Reading` and `Managing` was refined by implementation pressure into point-oriented capabilities.

The current validated primitives are:

```text
PointReader<ALG, POINT, ID>
PointWriter<ALG, POINT>
```

A service-owned algebra composes these capabilities across the semantic spaces it needs and implements `Functor<ALG>`.

Operations are lifted into `Free<ALG, A>`, so a Feature can construct an inert program before any Grounding is chosen.

Grounding supplies an `AlgebraIO<ALG>` interpreter, and Work requires it through:

```text
HasAlgebra<ALG, RT>
```

The experiment demonstrated that the same free program can be interpreted by different Groundings, that one algebra can span multiple point spaces, and that the resulting program composes inside the existing `Feature -> Flow` model.

See [Point Algebras](../point-algebras.md).

Deletion/removal remains open. It has not been folded into `PointWriter` merely because CRUD traditionally groups those operations.

Persistence-boundary questions such as staged versus autonomous persistence also remain open because they depend on additional semantics and guarantees rather than on reading/writing capability alone.

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

The capability side now has a concrete typed shape.

A service-owned algebra composes point capabilities and a Feature requires its interpreter through:

```csharp
where RT : HasAlgebra<AppAlgebra, RT>
```

Grounding supplies `AlgebraIO<AppAlgebra>`; the Feature never names the concrete Grounding implementation.

This preserves the original goal of making runtime requirements explicit without introducing pattern aliases such as `Repository`, `Store`, or `UnitOfWork` as semantic primitives.

The exact typed representation of guarantees remains intentionally open.

## Current scope decision

The capability substrate required for point reading/writing and free algebra interpretation has been implemented because the experiment produced direct executable evidence for it.

The following remain deliberately deferred:

- guarantee representation;
- analyzer diagnostics;
- law registration;
- guarantee-oriented VSIR syntax;
- extensible proof vocabulary;
- persistence-boundary semantics beyond the currently demonstrated point operations.

The current decision is therefore:

```text
adopt the validated capability substrate
    -> keep guarantees open
    -> return to the September 25 delivery
    -> resume guarantee work only under real pressure
```

This is a scope boundary, not a rejection of the guarantee model.
