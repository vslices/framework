# VSlices.Space

`VSlices.Space` is the semantic surface for describing what values can validly exist and which structures are intrinsic to those values.

Its current working principle is:

> Space establishes meaning. Grounding establishes contact.

The abstractions in this project are pressure-tested from real cases. They should not gain operations merely because their CLR representation supports them.

## Core spaces

- `DiscreteSpace<SELF>` expresses semantic equality without introducing ordering or algebra.
- `VectorSpace<SELF,SCALAR>` expresses additive vector structure and scalar multiplication/division.
- `AffineSpace<POINT,DISTANCE,SCALAR>` separates points from the vectors that translate them.
- `MaintainedSpace<SELF>` expresses a complete recognized set maintained explicitly by the type.
- `DerivedSpace<SELF,BASE>` expresses semantic subset inclusion (`SELF ⊆ BASE`) and total widening to the base space.

## Quantities

The current quantitative surface lives under `VSlices.Space.Quantities`.

```text
M            algebraic magnitude shape
Coordinate   primitive measurement basis
T            numeric carrier
```

The current single-basis quantity membership is:

```csharp
Q<F,C,T>
```

Its universal status remains intentionally open: multi-basis semantic quantities such as Speed and Acceleration do not fit one truthful primitive coordinate `C`.

Current primitive magnitudes are:

```text
M.Mass
M.Length
M.Duration
```

and the current algebraic vocabulary is:

```text
M.Mul<A,B>
M.Div<A,B>
M.Pow<A,N>
```

Current semantic pressure includes:

```text
Area          = Length²
Volume        = Length³
Speed         = Length / Duration
Acceleration  = Length / Duration²
```

Algebraic structure and semantic establishment remain distinct. `Length * Length`, for example, first materializes structural multiplication and may reduce algebraically to `Power<Length,N2>`; only explicit semantic establishment produces `Area`.

## Temporal points

`Moment` is the first production pressure on `AffineSpace`.

```text
Moment + Duration -> Moment
Moment - Moment   -> Duration
```

A `Moment` is therefore a temporal point, while `Duration` is a displacement between temporal points.

The first concrete affine realization uses the default seconds-based `Duration<double>`, which is promoted to `VectorSpace<Duration<double>,double>` for this pressure. This does not yet claim that every coordinate-specialized Duration form has been proven as a vector-space realization.

`Moment` intentionally has no ambient `Now` capability. Obtaining the current time is contact with the environment and belongs to Grounding/Clock capabilities rather than the semantic Space itself.

## Coordinates

Coordinates expose `ReferenceScale` with the fixed orientation:

```text
1 coordinate unit = ReferenceScale * canonical reference unit
```

Current canonical references are Gram, Meter, and Second for Mass, Length, and Duration. Composed algebraic shape lives in `M`; coordinates do not manufacture parallel types such as squared or quotient coordinates.

## Working criteria

- Semantic structure precedes realization convenience.
- A composite abstraction must add independent meaning, laws, or authority.
- Structural operation, algebraic reduction, and semantic establishment are distinct.
- Carrier operations do not automatically belong to the semantic Space.
- Generated realization constraints should remain owned by Tooling when they do not constitute semantic vocabulary.
- Grounding owns contact with concrete reality; Space should not acquire ambient state by itself.
