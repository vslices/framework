# VSIR naming semantics

This note records an experimental naming convention for VSlices Intermediate Representation artifacts and their corresponding application concepts.

The distinction is semantic rather than cosmetic.

A Feature and an Invariant are different kinds of things, so their names should describe different kinds of meaning.

## Core distinction

A Feature represents behavior to execute.

An Invariant represents a fact established about an input or output.

A useful short rule is:

> Feature names describe behavior to execute. Invariant names describe a fact established about their output.

This distinction should be visible directly in symbol names and filenames.

---

## Feature names

Features are executable application capabilities.

They consume a Request, perform behavior using runtime capabilities, and produce a Response.

Their names should therefore normally be verb phrases.

Canonical shape:

```text
<Verb><Object>
```

or, when needed:

```text
<Verb><Qualifier><Object>
```

Examples:

```text
CreateIdentity
ActivateAccount
DeactivateAccount
RestrictAccount
UpdateAccount
AssignRole
ResetPassword
RequestAccess
ApproveRequest
```

A useful naming test is:

```text
The system allows ______.
```

If the name naturally fills that blank as an action, it is likely Feature-shaped.

Conceptually:

```text
Feature
    Request
       |
       v
    behavior
       |
       v
    Response
```

The name describes what the application does.

---

## Why `CreateIdentity` is a good Feature name

`CreateIdentity` describes an action:

```text
create an identity
```

It answers:

```text
What can this application execute?
```

That is Feature semantics.

The same name is not appropriate for an Invariant because an Invariant does not primarily describe something the application executes.

It describes something that becomes known or established after the rule succeeds.

---

## Invariant names

An Invariant establishes admissibility or refinement.

Conceptually, current VSlices invariant shapes are:

```text
A -> A
```

and:

```text
A -> B
```

The name should describe the fact established by the successful transformation rather than the mechanism used to establish it.

Avoid mechanism-oriented names such as:

```text
ValidateAccount
CheckAccount
EnsureAccount
ValidateCreateIdentity
```

These names say that validation occurs but do not say what becomes true.

Prefer semantic names such as:

```text
EmailNotInUse
AccountCanBeActivated
IdentityCanBeCreated
ExistingAccount
RegisteredIdentity
AuthorizedUser
```

---

## `A -> A` invariant naming

When an Invariant preserves the same semantic value and only establishes admissibility, its name normally reads like a predicate or property.

Examples:

```text
EmailNotInUse
AccountCanBeActivated
AccountCanBeRestricted
RequestIsValid
IdentityCanBeCreated
```

These names answer a question similar to:

```text
What is now known to be true about this value?
```

Example:

```text
Account
   |
   v AccountCanBeActivated
Account
```

After the invariant succeeds, the application knows that the Account can be activated.

The invariant name describes that established fact.

---

## `A -> B` invariant naming

When an Invariant refines or derives a different semantic representation, a nominal or qualified-value name is often more natural than an explicit predicate.

Example:

```text
AccountId -> Account
```

A name such as:

```text
ExistingAccount
```

is preferable to a mechanical name such as:

```text
ValidateAccountId
```

because the result of successful refinement is semantically:

```text
an existing Account
```

Conceptually:

```text
AccountId
   |
   v ExistingAccount
Account
```

Other examples include:

```text
RegisteredIdentity
ExistingUser
AvailableEmail
AuthorizedAccount
ActiveSubscription
ResolvedCustomer
ValidatedAddress
```

The name describes the refined knowledge represented by the output.

---

## Two natural invariant naming families

Invariant names therefore tend to fall into two useful families.

### Property / predicate form

Common for `A -> A`:

```text
AccountCanBeActivated
EmailNotInUse
IdentityCanBeCreated
RequestIsValid
```

### Refined-value form

Common for `A -> B`:

```text
ExistingAccount
RegisteredIdentity
AuthorizedUser
ResolvedCustomer
```

These are guidelines rather than grammar rules.

The semantic test matters more than the surface form.

---

## Naming tests

A practical test for a Feature is:

```text
What does the system do?
```

Example answer:

```text
CreateIdentity
```

A practical test for an Invariant is:

```text
After this rule succeeds, what do we know?
```

Example answers:

```text
The account can be activated.
The email is not in use.
This AccountId identifies an existing account.
```

These naturally map to:

```text
AccountCanBeActivated
EmailNotInUse
ExistingAccount
```

A second technical test is:

```text
Feature
    Request -> effectful behavior -> Response

Invariant
    A -> admissible A
    A -> refined B
```

If the concept is an action in the first form, name it as behavior.
If the concept is knowledge established in the second form, name it as a fact or refined value.

---

## Filename convention

VSIR filenames should preserve the symbol name and expose the artifact kind explicitly.

Features:

```text
CreateIdentity.feature.vsir
ActivateAccount.feature.vsir
UpdateAccount.feature.vsir
```

Invariants:

```text
IdentityCanBeCreated.invariant.vsir
ExistingAccount.invariant.vsir
AccountCanBeActivated.invariant.vsir
EmailNotInUse.invariant.vsir
```

Editable C# projections follow the same identity:

```text
CreateIdentity.feature.vsir.cs
ExistingAccount.invariant.vsir.cs
```

PascalCase is preferred because these files represent application symbols rather than general prose documents.

---

## Naming should expose responsibility

The filename should make accidental responsibility confusion harder.

For example:

```text
CreateIdentity.feature.vsir
```

reads naturally as executable behavior.

By contrast:

```text
CreateIdentity.invariant.vsir
```

should look suspicious because `CreateIdentity` describes an action rather than a fact established by validation or refinement.

A better invariant name might be:

```text
IdentityCanBeCreated.invariant.vsir
```

or, depending on the actual transformation:

```text
CreatableIdentity.invariant.vsir
```

The exact name should follow what the successful output means.

---

## Core rule

The naming convention can be summarized as:

> Features do. Invariants establish.

More explicitly:

```text
Feature name
    describes executable application behavior

Invariant name
    describes an established property or refined semantic value
```

This semantic distinction should remain stable even if the concrete VSIR syntax or lowering strategy evolves.
