# Modeling executable Features in .NET

This document is the reference for the executable structural checks in
[`FeatureModelingLaws`](../../src/VSlices.Work/Laws/FeatureModelingLaws.cs).
It describes a **current realization convention**, not a universal theory of
Work, or proof that a Feature's execution is correct.

Use `FeatureModelingLaws.InspectDefinitions(typeof(MyFeature).Assembly, "MyDomain.Work.Features")`
in a consumer test. Fail on returned violations: the stable IDs and URLs point
to the exact rule below. The scan examines compiled top-level concrete Feature
classes in the selected namespace and its descendants, not folders. A direct
`Inspect(typeof(MyFeature))` checks one concrete type.

## FM001 — Feature identity

**Error:** A candidate Feature does not implement exactly one
`Feature<SELF, ALG, REQ, RES>` with its own type as `SELF`.

**Why:** The nominal owner and executable contract must refer to the same
definition. A mismatched self type obscures who owns the operation.

**Preferred realization:** Implement `Feature<ThisFeature, ThisAlgebra,
ThisFeature.Request, ThisFeature.Response>` on the concrete class.

**Alternative and limit:** Arbitrary CLR services need not implement Feature.
Reflection verifies the interface relationship, not the operation's meaning.
When FM001 fails, dependent checks cannot reliably interpret generic bindings.

## FM002 — Request ownership

**Error:** The Feature has no directly nested `Request` type.

**Why:** Input to this execution should be owned by the Work operation that
interprets it rather than by an unrelated transport or module.

**Preferred realization:** Declare `public sealed record Request(...)`
inside the Feature.

**Alternative and limit:** Shared DTOs can be appropriate outside the Feature
boundary. This Law does not judge the contents of Request or source file layout.

## FM003 — Response ownership

**Error:** The Feature has no directly nested `Response` type.

**Why:** The operation should own the nominal representation of the outcome it
recognizes.

**Preferred realization:** Declare a public nested `Response` with encapsulated
construction.

**Alternative and limit:** Reusable result primitives (`Option<A>`,
`Either<L,R>`, and similar types) can still appear *inside* Response.
Reflection cannot prove an outcome was actually produced by successful work.

## FM004 — Nominal binding

**Error:** The generic `REQ` or `RES` differs from the declared nested
`Request` or `Response`.

**Why:** Merely declaring nested types is insufficient if the executable
contract actually accepts or returns unrelated types.

**Preferred realization:** Bind `ThisFeature.Request` and
`ThisFeature.Response` as the exact generic parameters.

**Alternative and limit:** A generic operation not following this nominal
ownership convention can be legitimate elsewhere, but is not this Feature
model. The check proves equality of CLR types, not the adequacy of their data.

## FM005 — Request construction

**Error:** Request is not public, or has no public instance constructor.

**Why:** A consumer must be able to propose the Work operation without an
internal factory or a semantic `Transformable` contract.

**Preferred realization:** Use a public positional record or another public
Request constructor. Keep validation of actual semantic values in their owning
Spaces; Request does not need to implement `Transformable`.

**Alternative and limit:** A public static factory with a private constructor
could provide construction, but the current structural convention deliberately
requires a public constructor. The Law cannot verify parameter validation or
whether the caller's proposed values are meaningful.

## FM006 — Response construction

**Error:** Response is not public, lacks a declared instance constructor, or
has any instance constructor that is not private.

**Why:** Consumers should observe the outcome but not mint an arbitrary
successful-looking Response independently of the Feature.

**Preferred realization:** Declare a public nested Response with exclusively
private instance constructors, accessed by its owning Feature.

**Alternative and limit:** `internal` constructors broaden construction to
other code in the assembly, so they do not satisfy this convention.
Private constructors are **not** proof of execution, authorization, or deep
immutability. FM008 checks the current construction mechanism separately.

## FM008 — Response factory

**Error:** The Feature does not expose exactly one private static
`NewResponse` marked with `UnsafeAccessor(UnsafeAccessorKind.Constructor)`
whose parameter types match a Response instance constructor.

**Why:** Response construction should remain explicit in its owning Feature
without broadening constructor accessibility to the assembly.

**Preferred realization:** Match the private constructor signature:

```csharp
public sealed record Response
{
    private Response(Item item) => Item = item;
    public Item Item { get; }
}

[UnsafeAccessor(UnsafeAccessorKind.Constructor)]
private static extern Response NewResponse(Item item);
```

**Alternative and limit:** `UnsafeAccessor` is the present .NET mechanism,
**not** the semantic guarantee. Reflection does not prove that every Response
was constructed through this accessor or that it follows successful execution.
FM008 checks parameter *types*, not all possible runtime binding failures.

## FM009 — Explicit optionality

**Error:** A public instance property, public instance field, or instance
constructor parameter in owned Request or Response uses a C# nullable type
(`string?`, `int?`, and similar annotations).

**Why:** Meaningful absence must be represented explicitly in the contract
instead of introducing nullable branches without an owned interpretation.

**Preferred realization:** Use a required `A` when the information must
exist. Use `Option<A>` when `None` is a meaningful and valid alternative,
and define what that alternative means.

**Alternative and limit:** Nullable annotations may be appropriate at
transport/interop boundaries outside the Feature. The reflection check cannot
prove semantic appropriateness of `Option`, nested generic nullability,
or non-null runtime values. It does not scan arbitrary private fields.

## FM010 — Response immutability

**Error:** Response exposes a publicly settable property (including a public
`init` setter) or a mutable public instance field.

**Why:** Consumers must not replace the visible outcome after the Feature
produced it.

**Preferred realization:** Use getter-only public properties and private
construction, and choose immutable values for their contents.

**Alternative and limit:** This check is shallow. A read-only property
referencing a mutable object or collection can still expose mutable state.
Review and test the owned result shape separately.

## Modeling practice — Explicit outcomes are not exception control flow

Expected rejection, missing data, or validation failure belongs in explicit
effect errors (`Error.New`) or in the modeled result when that distinction is
semantically required. Reserve .NET exceptions for exceptional failures.
An `Eff` return type by itself cannot enforce this practice.

A Feature's public Request expresses an attempted operation; the Response
type expresses an owned outcome. A private constructor does **not** prove that
an effect ran successfully or that external side effects committed. If a
failure is expected, model it rather than creating a misleading successful
response or throwing an ordinary `InvalidOperationException`.

This is **a design and behavior review practice**, not another executable
reflection Law.

## Modeling practice — Composition and boundary ownership

A composing Feature constructs child Requests explicitly and projects the
appropriate module algebra when running their `Eff`:

```csharp
Child.Get(new Child.Request(...)).RunIO(mix.A)
```

Work does not acquire transport authority, while Grounding owns the concrete
realization of executable capability atoms. `AlgebraMix` expresses availability
of child algebras, not a transaction, rollback, or compensation guarantee.

Transport independence, outcome semantics and composition correctness are
review and test obligations, **not** unverified FM011–FM013 executable Laws.
`Flow` remains independently available, but `MapRuntime`, `MapRequest`
and `ContraMap` are not needed in the current Feature contract.

## What these Laws do not establish

- **File layout:** one Feature per file and the `Features/` directory are
  authoring conventions. Reflection sees compiled types and namespaces,
  not source file locations.
- **Execution correctness:** private Response constructors do not prove
  success, error propagation, authorization, side-effect atomicity,
  cancellation, or completion. Test these behaviors using real Features.
- **Applicability:** assembly scanning intentionally targets concrete
  top-level classes implementing Feature in a consumer-selected namespace.
  It does not inspect every CLR class nor impose the same shape on unrelated
  Work abstractions.
- **Semantic optionality:** FM009 rejects C# nullability in owned contracts
  but cannot determine whether `Option<A>` is a justified part of the model.

A violation reports its stable rule ID, the concrete definition, an actionable
error, and a URL to the exact rule and rationale. Automated detection and
conceptual justification remain distinct.
