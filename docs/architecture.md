# Architecture

VSlices is split by responsibility rather than by a Domain/Application/Infrastructure layering.

## VSlices

Core language and reusable mechanisms shared by the rest of the Framework.

Current examples include:

- `Req` / `ReqK`;
- `Flow`;
- category/arrow traits;
- literals and small cross-cutting primitives.

## VSlices.Space

Owns semantic meaning.

Space contains semantic values, structure, admissibility, identity where meaningful, pure transformations, quantities, temporal values, finance, and other semantic spaces.

A Space does not acquire external facts merely because a realization can do so.

## VSlices.Work

Owns executable behavior.

The current Feature contract is:

```text
Feature
    -> Flow<ALG, Request, Response>
```

For capability-backed Work, `ALG` is an executable algebra: a simple value that composes the smallest executable capability atoms the module exposes.

Example:

```csharp
public sealed record TodoAlgebra(
    PointReader<Todo, TodoId> Reader,
    PointWriter<Todo> Writer,
    PointRemover<Todo, TodoId> Remover,
    TodoIdSource Ids);
```

The algebra is the runtime required by the Flow. It is not a Free program, interpreter vocabulary, DI service bag, Repository, or Infrastructure layer.

## VSlices.Grounding

Owns concrete contact with the external world.

Grounding implements the executable atoms declared by Work and may expose the resulting algebra through:

```csharp
AlgebraIO<ALG>
```

For example, one grounding may implement point reading/writing/removal against memory while another realizes the same atoms through Entity Framework Core.

Grounding chooses mechanism. It does not acquire authority to redefine Work semantics.

## Composition

Independent Work algebras compose structurally:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
...
```

A child Flow is adapted to a composed runtime by projecting the algebra it requires:

```csharp
child.MapRuntime(
    (AlgebraSum<FileAlgebra, TodoAlgebra> sum) => sum.A)
```

Requests can be adapted independently with `MapRequest`, or runtime and request together with `ContraMap`.

This preserves the distinction:

```text
runtime composition
!= request adaptation
!= result mapping
!= Grounding realization
```

## Current boundary

The validated direction is therefore:

```text
Space
    semantic meaning

Work
    Feature -> Flow<Algebra, Request, Response>
    Algebra -> executable capability atoms

Grounding
    realizes capability atoms
    exposes executable Algebra
```

Historical Domain/Application/Infrastructure and Free-WorkFlow models are evidence of the path that led here, not the current architecture.
