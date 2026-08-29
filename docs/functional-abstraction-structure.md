# Functional Abstraction Structure Convention

VSlices functional abstractions should separate:

1. semantic definition;
2. Trait implementation;
3. public operation projection;
4. expression syntax;
5. lifting / entry-point operations.

The objective is to keep semantic behavior independent from the syntax used
to consume the monad.

A monad should not accumulate operators, LINQ support, fluent helpers,
Trait machinery, and semantic operations in the same definition file.

---

# Canonical structure

Using `Fin` as an example:

```text
Monads/
└── Fin/
    ├── Fin.Definition.cs
    ├── Fin.Implement.cs
    ├── Fin.Module.cs
    │
    ├── FluentAPI/
    │   └── ...
    │
    ├── Linq/
    │   └── ...
    │
    ├── Operations/
    │   └── ...
    │
    └── Lifting/
        └── ...
```

Each area has a distinct responsibility.

---

# `Fin.Definition.cs`

`Fin.Definition.cs` contains the semantic definition of the monad itself.

For `Fin<A>`, this includes the discriminated union that represents its
possible states:

```text
Fin<A>.Success
Fin<A>.Fail
```

It may also contain members whose purpose is directly semantic and tied to
the value represented by `Fin`.

Examples:

```text
Match
ThrowIfFail
```

It may contain fluent members when those members exist primarily to operate
on or observe the internal semantic value rather than to provide a generic
composition syntax.

The definition should expose what `Fin` *is* and what can be done with a
`Fin` value as a domain object.

It should not contain members whose primary purpose is:

* enabling LINQ query syntax;
* providing operator syntax;
* projecting Trait operations;
* providing cross-monad fluent composition;
* or implementing Trait machinery.

---

# `Fin.Implement.cs`

`Fin.Implement.cs` contains the concrete implementations of the Traits
supported by `Fin`.

These implementations are always private implementation machinery.

They operate directly over the native capabilities exposed by
`Fin.Definition`.

They should not depend on:

* extension methods;
* `Fin.Module`;
* LINQ syntax helpers;
* FluentAPI helpers;
* operator helpers;
* lifting helpers.

Trait implementations should use the most direct representation of the
monad available in `Fin.Definition`.

Conceptually:

```text
Fin.Definition
      |
      v
Fin.Implement
```

This is where the algebraic implementation of the Trait lives.

Trait implementation code should not depend on later syntactic projections.

---

# `Fin.Module.cs`

`Fin.Module.cs` exposes the public concrete forms of operations provided by
Traits.

Where Trait implementations may operate through abstractions such as:

```csharp
K<Fin, A>
```

the Module exposes the corresponding operation using the concrete monad:

```csharp
Fin<A>
```

Conceptually:

```text
Trait implementation
K<Fin, A>
      |
      v
Fin.Module
Fin<A>
```

`Fin.Module` is the public operational surface used by the expression
layers.

It does not need to expose every operation implemented internally.

It must expose the operations required by the supported expression forms
and by the public API of the monad.

The Module should not duplicate Trait behavior.

It projects existing behavior into the concrete `Fin<A>` API.

---

# Expression layers

Expression layers define how existing public operations are written.

They do not own semantic behavior.

All expression forms should ultimately delegate to operations available in
`Fin.Module` or directly to semantic members of `Fin.Definition` when that
is the actual intended operation.

The supported expression families are:

```text
FluentAPI
Linq
Operations
```

---

# `FluentAPI/`

The `FluentAPI` directory contains members whose primary purpose is to
enable fluent composition involving `Fin`.

These members are normally implemented through extension methods.

For example:

```csharp
value
    .Map(...)
    .Bind(...)
    .SomethingElse(...);
```

FluentAPI members should delegate to operations exposed by `Fin.Module`.

They should not contain independent implementations of `Map`, `Bind`, or
other algebraic behavior.

The fluent form is a syntax projection.

Conceptually:

```text
Fin.Module.Bind
      |
      v
.Bind(...)
```

---

# `Linq/`

The `Linq` directory contains members whose purpose is enabling C# LINQ
query syntax.

Typical operations include:

```text
Select
SelectMany
```

and query-compatible overloads based on operations such as:

```text
Bind
Map
```

For example:

```csharp
from a in first
from b in second(a)
select combine(a, b);
```

LINQ-specific members should delegate to the public operations exposed by
`Fin.Module`.

They should not independently implement monadic behavior.

Conceptually:

```text
Fin.Module.Bind
Fin.Module.Map
      |
      v
SelectMany / Select
```

---

# `Operations/`

The `Operations` directory contains members whose primary purpose is
supporting C# operator syntax between `Fin` values or between `Fin` and
compatible values.

For example:

```csharp
first >> second
```

or any other operator representation supported by the monad.

Operators should be thin projections over operations exposed by
`Fin.Module`.

They must not become alternate implementations of the underlying algebra.

Conceptually:

```text
Fin.Module operation
      |
      v
operator ...
```

---

# `Lifting/`

The `Lifting` directory contains operations that introduce values into the
monadic context or provide access to values without requiring an existing
monadic value as the receiver.

Unlike FluentAPI, LINQ, and Operations, lifting operations can normally be
used "from nothing".

They often act as entry points into a monadic computation.

Examples include operations conceptually similar to:

```text
Pure
Fail
Lift
fromValue
fromEffect
```

depending on the capabilities of the monad.

These operations are often exposed through a Prelude or module-level
function.

For example:

```csharp
pure(value)
```

or:

```csharp
fail(error)
```

rather than:

```csharp
existingMonad.Something(...)
```

Lifting differs from the other syntax families because it does not express
composition over an already existing monadic value.

It establishes or introduces the monadic context itself.

Conceptually:

```text
raw value / effect / error
          |
          v
       Lifting
          |
          v
        Fin<A>
```

Lifting operations are commonly the starting point of a computation.

---

# Dependency direction

The intended dependency direction is:

```text
Fin.Definition
      |
      v
Fin.Implement
      |
      v
Fin.Module
      |
      +----------------+
      |        |       |
      v        v       v
 FluentAPI    Linq   Operations
```

`Lifting` may depend on:

```text
Fin.Definition
Fin.Module
```

depending on the operation being exposed.

The reverse dependencies should not occur.

In particular:

```text
Fin.Implement
```

must not depend on:

```text
Fin.Module
FluentAPI
Linq
Operations
Lifting
```

Trait implementation is the lowest reusable implementation layer and must
remain independent from public syntax.

---

# Core rule

The central rule is:

> Semantic behavior flows outward. Syntax never flows inward.

Or, more explicitly:

```text
Definition
    -> Implementation
    -> Public operation
    -> Expression syntax
```

Never:

```text
Syntax
    -> implementation behavior
```

---

# Classification guide

When adding a new member, classify it by asking:

## Is this operation part of what the monad semantically is?

Place it in:

```text
Definition
```

Examples:

```text
Match
ThrowIfFail
```

## Is this implementing a Trait?

Place it in:

```text
Implement
```

## Is this exposing a Trait operation using the concrete monad type?

Place it in:

```text
Module
```

## Is this enabling fluent method chaining?

Place it in:

```text
FluentAPI
```

## Is this enabling LINQ query syntax?

Place it in:

```text
Linq
```

## Is this enabling operator syntax?

Place it in:

```text
Operations
```

## Is this introducing a value, error, effect, or other source into the

monadic context without requiring an existing receiver?

Place it in:

```text
Lifting
```

---

# Design principle

A VSlices monad should have one semantic model and many ergonomic
representations.

The semantic model belongs to the definition and Trait implementation.

The concrete public algebra belongs to the Module.

FluentAPI, LINQ, operators, and lifting are ways of entering or expressing
that algebra.

They must not become separate sources of truth.
