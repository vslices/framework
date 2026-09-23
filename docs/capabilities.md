# Capabilities

## What is a Capability?

A Capability is a typed description of an operation or behavior that Work may require.

Capabilities are inspired by LanguageExt typeclasses. Runtime constraints provide compile-time evidence that the required capability vocabulary can be interpreted or supplied.

They are used to express requirements like:
- time access
- persistence
- transactions
- event dispatch
- identity generation
- external integrations (but think more of a capability to call an external service, not the service client itself)

## What a Capability is not

A Capability is not:
- a service layer object
- a helper class
- a manager
- a static singleton
- a dependency injected directly into feature classes

## Main Rule

A feature must declare the minimum capability requirements it needs from `RT`.

For simple capabilities this can be a direct `Has*` requirement. For point capabilities, atomic operations compose into a Work-owned algebra and the Feature requires `HasAlgebra<ALG, RT>` as evidence that the runtime can interpret that vocabulary.

Capabilities and their runtime evidence should remain explicit in types.

## Why

This allows:
- compile-time verification of runtime requirements
- composable features
- small and honest dependencies
- easier testing through runtime substitution
- less hidden infrastructure coupling

## Preferred Style

Prefer:
- small capability constraints
- explicit effect composition
- typed failures
- runtime-driven composition

Avoid:
- constructor-injected feature dependencies
- large façade services
- hidden infrastructure access
- global mutable access patterns

## Tie-breaker Rule

When deciding between:
- introducing a service abstraction
- expressing a requirement as a runtime capability

prefer the runtime capability, unless there is a strong and explicit reason not to.

## Point Algebras

For Work operations over points of semantic spaces, the current capability model is based on atomic point capabilities composed by service-owned algebras.

The first validated point capabilities are:

- `PointReader<ALG, POINT, ID>`;
- `PointWriter<ALG, POINT>`.

A service-owned algebra composes only the operations it needs, `Free<ALG, A>` describes programs over that vocabulary without executing them, and `HasAlgebra<ALG, RT>` keeps the interpreter requirement explicit in the Feature runtime contract.

See [Point Algebras](point-algebras.md).

## Capabilities and Guarantees

Capabilities describe what Work can request.

Guarantees describe additional semantic properties that an admissible realization must preserve. Tracking, atomicity, isolation, ordering, and durability are examples of potential guarantees rather than independent capabilities.

The point-algebra capability substrate is now validated and implemented. The guarantee model remains exploratory and deliberately unimplemented while current delivery work has priority.

See [Capabilities and Guarantees](notes/capabilities-and-guarantees.md).
