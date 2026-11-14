# Maybe: incremental LanguageExt structure experiment

This experiment lives outside the VSlices production libraries. It investigates how to create a new
structure step by step, **before** adding LanguageExt traits, and will provide grounded examples
for a later skill.

## Agreed semantics

- Flavor: an A that may or may not exist.
- Representation: **explicit discriminated union**: `MaybeV[N]<A>` has `SomeV[N]<A>` and `NoneV[N]<A>`.
- `V0`: just C#, no LanguageExt dependencies, K, traits, laws, Map or Bind.

## Stable test contract

`Contracts/MaybeContractTests.cs` owns behavioral checks of the shared flavor:
presence, absence, payload preservation and the distinction between `Some(0)` and `None`.
Every version inherits this suite and provides an adapter to its concrete representation.
The common contract must not depend on particular constructors, records or pattern-matching
strategies. Only add new shared tests when the expectation applies to **every** version.

No equality/nullability policy, exhaustive-match API, constructors, additional type parameters,
or monad laws have been agreed. No tests are asserted for these topics yet.

## V0 implementation

`Versions/MaybeV0.cs` declares an abstract record and two sealed record variants.
The `V0` suffixes keep versions side by side. There is no list and no delegated
`LanguageExt.Option<A>`.

Run (with the .NET 10 SDK installed):

```sh
dotnet test tests/experiments/Maybe.Experimental.Tests/Maybe.Experimental.Tests.csproj
```

Implementation / build status: check the PR evidence; creating source files alone is not
proof that the tests have been executed.

## Explicit arithmetic experiment (without modifying V0)

`Contracts/MaybeArithmeticContractTests.cs` defines the **same** tests for any future
version that opts into the arithmetic experiment. It is parameterized both by the concrete
Maybe representation and by a numeric type `T : INumber<T>`. V0 runs these tests for
`int`, `long` and `decimal`, plus separate nonnumeric payload tests for `string`
and a record.

`Versions/MaybeV0Arithmetic.cs` is an **external test-side utility**, not a method of
`MaybeV0`, and uses only plain C# pattern matching and `Func<T,T,T>`. It implements
the proposed *all operands required* semantics: arithmetic over a nonempty list of
Some operands returns Some; the first None anywhere propagates None. A fold is
left-associative, so subtraction is order-sensitive.

The empty-input case remains conceptually **unsettled**: sum could use Some(0),
product could use Some(1), and subtraction needs a specific convention. The V0
test utility currently requires one operand and throws ArgumentException for an
empty input; **this is a helper precondition, not a universal Maybe law**.
No assertion for empty input belongs in the stable semantic test contract yet.

Other potential interpretations of absence (e.g. skip absent operands) are explicitly
**not** adopted by Maybe; explore them separately.

The existing `MaybeV0.cs` is unchanged. Additional versions can implement the
same arithmetic contract with different internal capabilities.
