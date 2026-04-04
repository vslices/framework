# Capabilities

## What is a Capability?

A Capability is a type-level description of something the runtime can provide.

Capabilities are inspired by LanguageExt typeclasses.

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

A feature must declare the minimum capabilities required from `RT`.

Capabilities should remain explicit in types.

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