# Architecture

VSlices is split by semantic responsibility rather than by traditional application layers.

## VSlices.Space

Semantic values, structure, admissibility, identity where explicitly meaningful, and pure transformation.

Examples include:

- discrete spaces;
- affine and vector spaces;
- quantities;
- `Transformable` / `Validatable`;
- `Evolvable` when accepted state evolution is meaningful.

`Entity` and `AggregateRoot` are not framework semantic categories.

## VSlices.Work

Executable behavior and WorkFlow vocabulary.

The current executable boundary is:

```text
Feature == WorkFlow == Free<ALG, Response>
```

A Feature owns its request, response, WorkParts, algebra, and Free program.

Reusable capability vocabulary currently includes examples such as:

- point reading, writing, and removal;
- clock observation;
- temporal delay;
- algebra composition.

`Flow<RT, REQ, RES>`, Repository, and ambient runtime carriers are not part of the current Work model.

## VSlices.Grounding

Concrete realization of WorkParts against an external world.

Examples include:

- Entity Framework Core point grounding;
- `TimeProvider` temporal grounding;
- in-memory interpreters used by executable examples.

Grounding may use technical mechanisms such as EF entities, projections, clocks, transports, or hosts without turning those mechanisms into semantic vocabulary.

## Presentation and invocation

HTTP, CLI, scheduled, message-driven, and other invocation mechanisms are presentation/invocation concerns.

The current HTTP sample interprets a Feature program explicitly. Reusable invocation bindings remain a future pressure point.

## Core VSlices

The `VSlices` project still hosts lower-level reusable implementation machinery such as arrows, literals, errors, and category-related abstractions.

Those mechanisms do not define the semantic ontology of the Framework by themselves.
