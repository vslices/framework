# Quantities

The current model separates algebraic shape, primitive measurement basis, and numeric carrier:

```text
M            algebraic magnitude shape
Coordinate   primitive measurement basis
T            numeric carrier
```

`Q<F,C,T>` remains the current single-basis quantity shape, but its universal status is intentionally open. `Speed` and `Acceleration` show that real quantities may need multiple primitive bases; `Power` shows that repeated algebraic shape can still use one primitive basis.

## Magnitudes

```csharp
M.Mass
M.Length
M.Duration
M.Temperature
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

Temperature adds the first affine-coordinate pressure. `TemperatureCoordinate` combines the scale of its associated temperature-difference unit with the coordinate value of absolute zero:

```text
kelvin = (value - AbsoluteZero) * ReferenceScale
```

The supported point coordinates are currently Kelvin, Celsius, and Fahrenheit. A `TemperatureDifference` uses only `ReferenceScale`; an absolute `Temperature` also uses the coordinate's `AbsoluteZero` origin. Expressing the shared physical origin directly avoids treating affine offsets as if they were multiplicative units and keeps the boundary exact in each coordinate.

## Product and Power

Same-factor multiplication materializes as:

```csharp
Product<F,C,T>
    : Q<M.Mul<F,F>,C,T>
```

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

Current named operations include:

```csharp
square(length)
square(duration)
cube(length)
```

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

## Quotient and Speed

`Length / Duration` materializes the direct algebraic division shape:

```csharp
Quotient<
    M.Length,
    LENGTH_C,
    M.Duration,
    DURATION_C,
    T>
```

`Speed` is explicitly established from that quotient:

```csharp
speed(distance / duration)
```

It preserves both primitive coordinate bases and therefore does not currently implement `Q<F,C,T>`.

## Repeated division and Acceleration

The next concrete `M.Div` pressure is repeated division by the same magnitude family:

```text
Div<Div<Length,Duration>,Duration>
```

The promoted algebraic reduction is:

```text
Div<Div<Length,Duration>,Duration>
-> Div<Length,Pow<Duration,N2>>
```

This gives one canonical acceleration base shape while preserving the two primitive bases Length and Duration:

```csharp
Quotient<
    M.Length,
    LENGTH_C,
    M.Pow<M.Duration,N2>,
    DURATION_C,
    T>
```

Both paths are valid:

```csharp
acceleration(distance / square(duration))
acceleration(speed(distance / elapsed) / interval)
```

For the nested path, the incoming `interval` is converted to the Duration basis already carried by Speed before the operator materializes the reduced quotient.

`Acceleration` is then defined semantically over that reduced quotient:

```csharp
Acceleration<LENGTH_C,DURATION_C,T>
    : DerivedSpace<
        Acceleration<LENGTH_C,DURATION_C,T>,
        Quotient<
            M.Length,
            LENGTH_C,
            M.Pow<M.Duration,N2>,
            DURATION_C,
            T>>
```

As with Area and Volume, algebraic reduction is not semantic establishment. `Speed / Duration` may reduce to the acceleration magnitude shape, but `acceleration(...)` remains explicit.

## Temperature: affine coordinates, bounded physical space

The old temperature value mixed absolute temperatures, scale conversion, vector-like arithmetic, and the absolute-zero invariant. The migrated model separates these responsibilities.

`TemperatureDifference<C,T>` is a quantity and vector:

```csharp
TemperatureDifference<C,T>
    : Q<M.Temperature,C,T>
    : VectorSpace<TemperatureDifference<C,T>,T>
```

An absolute temperature is a point expressed in an affine coordinate:

```csharp
Temperature<C,T>
```

but it deliberately does **not** implement `AffineSpace`. Physical temperatures are bounded below by absolute zero, so arbitrary translation by a temperature difference is not closed:

```text
Temperature - Temperature -> TemperatureDifference               total
Temperature.Translate(TemperatureDifference) -> Fin<Temperature> partial
```

`Temperature.Create(...)` owns physical admissibility and rejects coordinate values below that coordinate's exact `AbsoluteZero`. Point conversion between established temperatures is total; displacement conversion uses scale only.

This distinction is semantic rather than a CLR limitation. An unrestricted affine coordinate line and the physically admissible temperature region are not the same space.

## Current algebraic reductions

Only reductions pressured by concrete cases are promoted:

```text
Mul<X,X> -> Pow<X,N2>
Pow<Length,N2> x Length -> Pow<Length,N3>
Div<Div<Length,Duration>,Duration> -> Div<Length,Pow<Duration,N2>>
```

No general normalization engine exists yet. Product and Quotient preserve structural operations; Power and reduced Quotient forms express promoted algebraic reductions; Area, Volume, Speed, and Acceleration establish domain semantics.

## Deferred general-use coordinate surface

Before the quantity migration is considered ergonomically complete, the general-purpose coordinate vocabulary should be revisited. This is intentionally deferred until the semantic model stabilizes.

Length should consider support for at least:

```text
miles, nautical miles, yards, feet, inches,
kilometers, hectometers, decameters, meters,
centimeters, millimeters, micrometers, nanometers, angstroms
```

Mass should similarly revisit a broader useful surface around:

```text
grams, kilograms, micrograms, pounds,
and other broadly useful metric / imperial mass coordinates
```

The old `Module.cs` unit aliases may also be reconsidered as a final ergonomics layer, potentially generated rather than maintained manually.

## Open quantity question

Current evidence suggests:

```text
Q<F,C,T>
    useful for quantities with one truthful primitive basis
```

rather than proving that `Q` is the universal definition of quantity. `Speed` and `Acceleration` both require multiple primitive coordinate bases without manufacturing a synthetic coordinate algebra, while `Temperature` now shows that not every physically meaningful measured value is itself a vector quantity.
