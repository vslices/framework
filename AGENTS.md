# AGENTS.md

## Purpose
This repository contains a framework for building applications using:

- Domain-Driven Design
- Functional programming patterns inspired by LanguageExt
- Vertical Slice Architecture
- Progressive, composable, batteries-included design

The framework is not intended to be a general-purpose abstraction library.
Its purpose is to provide strong, composable primitives and conventions for building application slices with high correctness and low accidental complexity.

The main goal is to maximize:

- simplicity
- explicitness
- composability
- compile-time guarantees
- predictable feature construction

---

## Core Philosophy

This framework is based on four foundations:

1. Domain-Driven Design recommendations
2. Functional programming patterns inspired by LanguageExt
3. Vertical Slice Architecture
4. Batteries included, while remaining open to multiple interchangeable batteries

General principles:

- No null values
- No exceptions for control flow
- Prefer composition over inheritance
- Prefer compile-time safety over runtime safety
- Prefer explicit dependencies over hidden dependencies
- Follow SOLID, DRY, KISS
- Avoid over-engineering
- Keep abstractions honest and minimal

---

## Functional Style

This project follows a functional-first style.

Use these principles consistently:

- Use `Option` instead of null
- Use `Either` or equivalent explicit result types for failures
- Use `Eff`, `IO`, and related effect abstractions for side effects
- Avoid unmodeled side effects
- Prefer pure functions where possible
- Make effectful boundaries explicit in types
- Prefer composition of smaller operations over large mutable workflows

Do not introduce imperative or object-oriented patterns when a simpler functional composition is available.

---

## Architecture

This framework uses Vertical Slice Architecture.

Rules:

- Each feature is self-contained
- Features are the main application boundary
- Avoid shared service layers
- Avoid central orchestration services unless strictly necessary
- Domain logic belongs in the domain model or in focused domain behaviors
- Infrastructure must remain behind explicit capabilities/effects
- Application code should orchestrate, not hide dependencies

A feature should be understandable in isolation.

---

## Capability Model

### Definition

A Capability is a type-level requirement that describes something the runtime can do.

Capabilities are conceptually similar to LanguageExt typeclasses.

A Capability is NOT:

- a service object
- a helper
- a manager
- a static utility
- a dependency to inject through constructors

A Capability IS:

- a constraint on `RT`
- a typed statement of required runtime behavior
- a composable requirement that allows features to access effects safely

The runtime type `RT` is the carrier of capabilities.

Features should express their requirements in terms of what `RT` must support.

### Examples

Examples of capabilities include:

- current time access
- id generation
- persistence access
- transaction execution
- event dispatching
- external API access
- current user context
- logging
- configuration access

These should be modeled as runtime capabilities, not as ad-hoc service dependencies.

### Capability Usage Rules

- Capabilities must be expressed through `RT` constraints
- Capabilities must not be instantiated directly inside features
- Capabilities must not be hidden behind generic service abstractions
- Capabilities must not be resolved from a service locator
- Capabilities must not be accessed through global state
- Capabilities must remain explicit at the type level

When implementing a feature, prefer:

- explicit runtime constraints
- explicit effect composition
- explicit error propagation

Avoid:

- constructor injection for feature dependencies
- service classes that merely wrap capabilities
- indirect abstractions that obscure runtime requirements

### Capability Composition

Capabilities are meant to compose.

A feature can require multiple capabilities through `RT`.

The runtime acts as the composition point for available capabilities.

When multiple capabilities are needed:

- keep them explicit
- require only the minimum needed
- do not bundle unrelated capabilities into coarse abstractions
- do not introduce aggregate "application services" just to simplify signatures

Prefer small, honest capability requirements over broad opaque dependencies.

---

## Feature Execution Model

Features are the main executable unit of application behavior.

A feature should be modeled as a function from input to `FeatureEff<RT, A>` or the equivalent project-specific abstraction.

Conceptually:

`In -> FeatureEff<RT, Out>`

`FeatureEff<RT, A>` is the primary execution abstraction and should represent:

- runtime dependency through `RT`
- effectful execution
- explicit failure handling

### Feature Rules

- Features must not execute uncontrolled side effects
- Features must not depend on infrastructure concretions directly
- Features must use capabilities through `RT`
- Features must return explicit effectful values
- Features must keep orchestration local and readable
- Features should be small enough to reason about without scanning the whole codebase

### Error Handling

Failures must be modeled explicitly.

- Do not throw exceptions for expected flows
- Use explicit error types
- Prefer domain-specific error values over generic exceptions
- Preserve error information across compositions
- Prefer typed failures over string-based conventions

---

## Transactions

Transactions are an execution concern, not business logic.

If the framework provides a `TransactionRunner` capability, it should be treated as a runtime capability.

Rules:

- Do not manually control transaction mechanics inside features unless the abstraction explicitly requires it
- Do not spread transaction semantics across unrelated code
- Keep transaction boundaries explicit
- Use the transaction capability as composition infrastructure, not as a service pattern

---

## Events

Domain events and integration events must remain explicit.

Rules:

- Do not dispatch events implicitly through hidden infrastructure
- Do not mix domain mutation and event dispatch in opaque service methods
- If event dispatch is effectful, model it through capabilities
- Event buffering and dispatching should remain explicit execution concerns

Prefer designs where:
- events are collected intentionally
- publication happens through explicit runtime capabilities
- background or deferred dispatch remains modeled, not magical

---

## Domain Modeling

Prefer rich domain modeling where it reduces accidental complexity.

### Value Objects

- Must be immutable
- Must validate on creation
- Must not expose invalid states
- Should avoid primitive obsession where meaningful domain concepts exist

### Entities / Aggregates

- Protect invariants explicitly
- Keep mutation controlled
- Emit events intentionally when appropriate
- Avoid anemic modeling when behavior belongs in the domain

### Domain Rules

- Put business rules close to the domain concepts they govern
- Avoid scattering domain decisions across handlers, repositories, and utilities
- Prefer explicit domain language in names and types

---

## Modification Guidelines

When making changes:

- Keep changes minimal and localized
- Preserve the current architectural direction
- Prefer improving composition over adding new layers
- Do not modify multiple layers unless necessary
- Do not introduce abstractions without at least two real use cases
- Do not add new dependencies without strong justification
- Do not "prepare for the future" unless the current code already demands it
- Prefer extending existing primitives over inventing parallel ones

When proposing a refactor:

- explain the current problem
- explain why the new design is simpler
- explain what complexity is being removed
- explain what constraints are being preserved

---

## What To Avoid

### Anti-Patterns

Do NOT:

- introduce null
- throw exceptions for control flow
- use exceptions as expected-domain-failure signaling
- inject services into features through constructors
- create "Manager", "Helper", "Utility", or "Service" classes without strong justification
- hide runtime requirements behind façade objects
- bypass `RT` with static/global access
- create premature abstractions
- generalize with no second real use case
- introduce inheritance where composition is enough
- centralize behavior that should remain inside slices
- create thin wrappers that add indirection but no clarity
- replace explicit capability constraints with vague object-oriented dependencies

### Specific Capability Anti-Patterns

Do NOT model capabilities as:

- dependency injection services for feature classes
- singleton objects accessed globally
- bags of unrelated methods
- infrastructure objects passed around without type-level meaning

Do NOT say:
- "inject the repository service"
- "use a service layer to access runtime concerns"
- "wrap all capabilities inside one application service"

Instead, think in terms of:
- runtime requirements
- capability constraints
- effect composition
- explicit typed dependencies

---

## Testing

Testing should preserve the same architectural model.

Rules:

- Prefer testing features through their effectful API
- Prefer fake/test runtimes over mocking service classes
- Test capabilities through explicit runtime composition where possible
- Prefer adding tests over rewriting stable production code
- Do not distort production design only to satisfy test style preferences

When tests require dependencies, prefer supplying an appropriate test `RT` rather than inventing service abstractions just for testability.

---

## Style Expectations for Code Changes

When generating or modifying code:

- prefer small focused files
- prefer intention-revealing names
- prefer expressions over deeply nested statements
- avoid hidden control flow
- avoid ambient mutation
- keep the execution path easy to follow
- optimize for readability by a maintainer who values functional composition and DDD

If a simpler solution exists, prefer it.

If a proposed abstraction does not clearly remove duplication or complexity, do not introduce it.

---

## Output Expectations for Proposed Changes

When proposing changes, always provide:

1. Summary of intent
2. Files modified
3. Reasoning
4. Risks
5. Why this design fits the framework philosophy

If a change introduces a new abstraction, also explain:

- why the existing design was insufficient
- why this is not premature generalization
- which concrete complexity it removes

---

## Decision Heuristics

When in doubt, prefer:

- explicit over implicit
- typed over ad-hoc
- composable over centralized
- local reasoning over framework magic
- honest constraints over convenient hiding
- compile-time guarantees over runtime conventions
- minimal viable abstraction over speculative architecture

The framework should feel progressive, composable, and strict in the right places.