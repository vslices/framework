# Modeling semantic Spaces in .NET

This document is the reference for the executable structural checks in
[`SpaceModelingLaws`](../../src/VSlices.Spaces/Laws/SpaceModelingLaws.cs).
It describes a **current realization convention**, not a universal ontology
of all Spaces or a proof that a domain model is correct.

Use `SpaceModelingLaws.InspectDefinitions(typeof(MySpace).Assembly, "MyDomain.Spaces.Definitions")`
in a consumer test. Fail on returned violations; their IDs and URLs point
to the precise sections below. This checks compiled types, not source file paths.

## SM001 — Semantic construction

**Error:** A concrete class in the selected Definitions namespace does not
implement a target-owned `Transformable<FROM, SELF>` (or an explicit
`Transformable<SELF, FROM, TO>` owned by that semantic type).

**Why:** A semantic definition should establish its point through a named,
fallible construction rule. Otherwise consumers cannot tell where admissibility
is decided.

**Preferred realization:** Declare `SELF.Input` and implement `Transformable<SELF.Input, SELF>`. Input ownership is checked independently by SM006.

**Alternative and limit:** Direct public constructors are common and valid
for ordinary CLR data carriers, DTOs and infrastructure entities. They are not
appropriate for a definition whose creation must be governed by semantic
admissibility. This structural Law cannot prove that `Transform` checks the
correct rules; write behavioral tests for accepted/rejected inputs.

## SM002 — Constructor encapsulation

**Error:** A class in Definitions exposes a public instance constructor.

**Why:** Callers could create points while bypassing `Transform`, defeating
the boundary that establishes valid semantic values.

**Preferred realization:** Keep instance constructors private and materialize
accepted points through the semantic owner.

**Alternative and limit:** An `internal` constructor is not prohibited by
this particular law; it still allows construction inside the assembly, and may
need stricter review. Reflection cannot establish whether a `Transform`
implementation actually uses the constructor correctly. Do not apply this rule
indiscriminately to non-semantic DTOs.

## SM003 — Evolution State ownership

**Error:** An `Evolvable<SELF,STATE>` uses a State type not nested within SELF.

**Why:** The semantic owner should control how candidate and accepted state
are represented. Shared, freely constructed state types undermine that boundary
in our current reference model.

**Preferred realization:** Define a nested `SELF.State` and expose
`CurrentState` from SELF. See
[`Todo`](../../examples/SampleWorkflow/SampleWorkflow.Spaces/Todo.cs).

**Alternative and limit:** A separately owned State might be justified by
another semantic authority. That requires a case-specific decision; this Law is
for the current nested-State convention, not every possible Evolvable model.

## SM004 — State constructor encapsulation

**Error:** A nested evolvable State exposes a constructor that is not private.

**Why:** `internal` still lets unrelated code in the assembly mint State
instances. In this reference model, only SELF should establish a State.

**Preferred realization:** Give `State` a private constructor. The owner can
use a private `UnsafeAccessor(UnsafeAccessorKind.Constructor)` in the current
.NET realization, as `Todo` does. Verify the exact constructor signature.

**Alternative and limit:** Relaxing the constructor to `internal` is simpler
but broadens authority to the assembly. `UnsafeAccessor` is a realization
mechanism, **not** the semantic rule itself. Static reflection cannot prove
that only SELF calls that accessor; a source analyzer would be needed.

## SM005 — Accepted State exposure

**Error:** `CurrentState` is missing, has the wrong type, or exposes a setter.

**Why:** Consumers should observe the accepted State but not replace it
directly, bypassing the `Evolve(old, proposed)` decision.

**Preferred realization:** `public State CurrentState { get; }`, backed by
a private instance constructor. Use init-only changes in candidate record
copies for fields that are legitimately evolvable.

**Alternative and limit:** A mutable holder may suit an application DTO or
Grounding record, but is not equivalent to this immutable semantic point.
Reflection does not establish deep immutability, preservation of identity or
whether `Evolve` validates the actual transition; test those behaviors
separately with real domain examples.

## SM006 — Input ownership

**Error:** The definition does not declare a nested `Input`, or its
`Transformable` uses a different source type instead of `SELF.Input`.

**Why:** The information needed to establish a semantic Space belongs to the
Space that interprets and validates it. A nested Input makes that ownership
explicit and prevents unrelated or transport-level contracts from becoming
the accidental authority for construction.

**Preferred realization:** Declare `Input` as a nested subtype of the
definition and implement `Transformable<SELF.Input, SELF>`. For example,
`Item.Input` belongs inside `Item`. Input may be a record, record struct,
or class, depending on its semantics.

**Alternative and limit:** Accepting `string`, `Guid` or an external DTO
directly can be convenient, but does not encode this ownership convention.
The law is scoped to explicit consumer Definitions namespaces; it does not
retroactively redefine all historic Framework examples or other transformation
contexts. Reflection verifies the type relationship, not whether Input
contains the correct evidence or Transform enforces all semantic invariants.

## SM007 — Explicit optionality in owned Input and State

**Error:** A member of a definition's nested `Input` or `State` uses
C# nullable syntax (`string?`, `ItemId?`, `int?`, etc.). The Law inspects
compiled public instance properties and fields, plus constructor parameters.

**Why:** Semantic absence must be represented by an explicit algebraic type,
not an implicit nullable escape hatch. If a value is required, its type should
be non-nullable and construction should not invent an absent case just to
reject it later at runtime. If absence is meaningful, it belongs in the model.

**Preferred realization:** Use the required type directly (`ItemId`,
`ItemName`) when the information must exist. When the concept is genuinely
optional, use `Option<A>` from Language-Ext, exposed under `ForkExt`,
rather than `A?`. Keep the handling of `None` explicit.

**Alternative and limit:** Nullable C# types are useful in ordinary DTO,
interop and infrastructure boundaries, but these scoped semantic contracts
follow the explicit optionality convention. This reflection Law reads nullable
metadata and catches `Nullable<T>` and annotated reference types; it does
not establish that every non-nullable reference actually contains a value at
runtime. Null-forgiving operators, oblivious annotations and generic nested
nullability require source analysis and compile-time warnings-as-errors for
stronger enforcement. This Law does not prohibit nullable types outside
owned `Input` and `State` contracts.

## Modeling practice — Optionality follows construction semantics

An `Input` member should be optional **only when its absence is a valid and
meaningful possibility at the moment the Space is constructed**. The mere fact
that `Option<A>` can represent `None` does not justify admitting `None` in
a particular construction contract.

- **Required at construction:** use `A` directly. For example, if an `Item`
  cannot be established without an `ItemId` and an `ItemName`, its contract
  is `Input(ItemId Id, ItemName Name)`, not `Input(Option<ItemId> Id,
  Option<ItemName> Name)`. Do not invent a `None` branch merely to reject it
  in `Transform`. Preserve the constraint in the type signature.
- **Genuinely optional at construction:** use `Option<A>` (Language-Ext,
  exposed through `ForkExt`) when both `Some` and `None` are admissible
  and have distinct, defined semantics. Document what `None` means and how
  construction interprets it.
- **Unknown, pending, or supplied later:** do not automatically equate these
  situations with optionality. First determine whether the Space can validly
  exist without that information. If the answer depends on a lifecycle or
  state transition, model that distinction explicitly instead of making
  every field optional.

**Why it matters:** Unnecessary `Option<A>` adds representable states,
branches for `Some`/`None`, validation work, and cognitive overhead without
providing domain meaning. Avoiding those branches may also reduce runtime
work, but the primary concern is **semantic accuracy and compile-time
guidance**, not an unmeasured performance claim.

**Review question:** *Can this Space legitimately be constructed when this
value is absent?* If not, require `A`. If yes, explain the significance of
`None` and use `Option<A>`.

This is a **modeling practice, not another executable Law**: SM007 can detect
C# nullable annotations, but reflection cannot decide whether a particular
`Option<A>` is semantically justified. That decision belongs to the Space
model and its owner.

## What these Laws do not establish

- **File layout:** one definition per file and the `Definitions/` folder are
  authoring conventions. Reflection sees namespaces and compiled types, not
  source paths. Enforce file layout through a separate source analyzer if needed.
- **Correct domain semantics:** the Laws do not infer validation rules, valid
  transitions, idempotency, equality, value immutability or domain ownership.
  Those require behavioral tests or stronger verified mechanisms.
- **Applicability:** these structural checks apply to explicitly selected
  Definitions namespaces. They do not require every CLR class, every Space
  category, or every `Transformable` implementation to have the same shape.

The message emitted by a failing Law includes its stable ID, the concrete
definition, a specific error, and the URL of the corresponding rule and its
rationale. Automated detection and conceptual justification are distinct.
