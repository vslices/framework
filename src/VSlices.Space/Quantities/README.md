# Quantities

The current model separates algebraic shape, primitive measurement basis, and numeric carrier:

```text
M            algebraic magnitude shape
Coordinate   primitive measurement basis
T            numeric carrier
```

`Q<F,C,T>` remains the current single-basis quantity shape, but its universal status is intentionally open. `Speed` shows that real quantities may need multiple primitive bases; `Power` shows that repeated algebraic shape can still use one primitive basis.

## Magnitudes

```csharp
M.Mass
M.Length
M.Duration
M.Mul<A,B>
M.Div<A,B>
M.Pow<A,N>
```

Current exponents:

```csharp
N2
N3
```

Algebraic shape belongs to `M`; coordinates never encode squares, cubes, products, quotients, or powers.

## Coordinates

`Coordinate<F>` identifies a primitive measurement basis. `ReferenceScale` is oriented as:

```text
1 coordinate unit = ReferenceScale * canonical reference unit
```

Current fixed references are Gram, Meter, and Second for Mass, Length, and Duration respectively. They are intentionally not configurable yet.

## Product and Power

Same-factor multiplication now materializes as:

```csharp
Product<F,C,T>
    : Q<M.Mul<F,F>,C,T>
```

For example, `2 km * 300 m` becomes `0.6 km²` on the Kilometer basis.

A same-factor Product can reduce implicitly to the equivalent square Power:

```csharp
Product<F,C,T>
    -> Power<F,N2,C,T>
```

The conversion is implicit because it preserves value, basis, carrier, and adds no domain semantics. The reverse expansion is explicit.

Different algebraic shapes that share one primitive basis still use:

```csharp
Product<LEFT_F,RIGHT_F,C,T>
```

and independently based multiplication still uses:

```csharp
Product<LEFT_F,LEFT_C,RIGHT_F,RIGHT_C,T>
```

`Power` is:

```csharp
Power<BASE_F,EXPONENT,C,T>
    : Q<M.Pow<BASE_F,EXPONENT>,C,T>
```

Current named operations are `square(length)` and `cube(length)`.

## Area

Area is defined mathematically over squared Length:

```csharp
Area<C,T>
    : Q<M.Pow<M.Length,N2>,C,T>
    : DerivedSpace<Area<C,T>, Power<M.Length,N2,C,T>>
```

Both paths are valid:

```csharp
area(square(side))
area(width * height)
```

The second path means:

```text
Length x Length
-> Product<Length,C,T>
-> implicit Power<Length,N2,C,T>
-> explicit area(...)
```

The important boundary is: algebraic reduction may be implicit; semantic establishment remains explicit.

## Volume

Volume is defined over cubed Length:

```csharp
Volume<C,T>
    : Q<M.Pow<M.Length,N3>,C,T>
    : DerivedSpace<Volume<C,T>, Power<M.Length,N3,C,T>>
```

Both paths are valid:

```csharp
volume(cube(side))
volume(area(width * depth) * height)
```

`Area * Length` aligns the incoming Length coordinate to Area's primitive basis and reduces `Length² * Length` directly to `Length³`.

## Quotient and the open Q question

`Length / Duration` preserves both primitive bases in `Quotient<...>`. `Speed` therefore does not implement `Q<F,C,T>`.

Current evidence suggests:

```text
Q<F,C,T>
    useful for quantities with one truthful primitive basis
```

rather than proving that `Q` is the universal definition of quantity.

## Current algebraic reductions

Only reductions pressured by concrete cases are promoted:

```text
Mul<X,X> -> Pow<X,N2>
Pow<Length,N2> x Length -> Pow<Length,N3>
```

No general normalization engine exists yet. Product preserves performed multiplication; Power expresses reduced repeated shape; Area and Volume establish domain semantics.
