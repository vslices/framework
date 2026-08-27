# Canonical Code Patterns

This document records preferred code shapes for VSlices.

Its purpose is to reduce implementation indeterminism for humans and code-generating agents.
When multiple equivalent implementations are possible, prefer the shapes below unless the domain requires otherwise.

## 1. Feature shape

A Feature is the executable application unit.
Keep `RT` in the feature type because runtime constraints are part of the execution contract.

```csharp
public sealed class CreateSomething<RT> :
    ServiceFeature<CreateSomething<RT>, RT, Request>
    where RT : HasRequiredCapability<RT>
{
    public static string Name => "Service.CreateSomething";

    public static ServiceClaim ExecutableBy => CreateSomethingClaim.Value;

    public static Flow<RT, Request, Unit> Get() =>
        Flow.request<RT, Request>() >>
        (request => /* domain/application transformation */) >>
        (result => /* capability-backed effect */);
}
```

Do not move `RT` to `Get<RT>()`. The feature must be able to state constraints such as:

```csharp
where RT : HasRepositoryAccess<RT>
```

## 2. Read Flow inputs explicitly

`Flow` has two independent input channels:

```txt
RT  = runtime / capabilities
REQ = execution request
```

Read them with the canonical helpers:

```csharp
Flow.runtime<RT, REQ>()
Flow.request<RT, REQ>()
```

Do not try to obtain `REQ` through `asks` or a runtime capability.
The request is not part of `RT`.

## 3. Prefer pipeline composition

For linear application behavior, prefer `>>` over wrapper methods whose only purpose is sequencing.

Prefer:

```csharp
Flow.request<RT, Request>() >>
(request => Validate(request)) >>
(value => Persist(value).IgnoreF());
```

Avoid when no additional meaning is introduced:

```csharp
Flow.request<RT, Request>() >>
(request => Execute(request));

private static Eff<RT, Unit> Execute(Request request) => ...;
```

Extract a named operation only when the name carries domain meaning, the operation is reused, or the local flow becomes harder to read.

## 4. Discriminated inputs use Match

When an input models a closed set of domain variants and exposes `Match`, prefer `Match` over a C# type switch.

```csharp
input.Match(
    Natural: n => CreateNatural<RT>.Invariants.RunEff(n).MapSuper(),
    Legal:   l => CreateLegal<RT>.Invariants.RunEff(l).MapSuper())
```

This makes sum-type elimination explicit and gives generators one canonical form.

Use a C# `switch` only when the type does not expose a domain `Match` operation or when pattern matching needs semantics that `Match` cannot express.

## 5. Execute ReqK with RunEff

When a completed `ReqK<Eff<RT>, ...>` is executed inside application code, use:

```csharp
rules.RunEff(input)
```

Do not use the former `ToEff(input)` name.
`RunEff` communicates execution rather than representation conversion.

## 6. Upcast inside functors with a named operation

When two branches produce different domain subtypes but the next operation consumes their common supertype, prefer an explicit reusable upcast helper.

```csharp
CreateNatural<RT>.Invariants.RunEff(n).MapSuper()
CreateLegal<RT>.Invariants.RunEff(l).MapSuper()
```

Prefer this over repeating:

```csharp
.Map<SrvIdentity>(x => x)
```

The helper should remain a pure `Functor` map. It must not add behavior.

## 7. Runtime capabilities stay type-level

A Feature states the capabilities it requires through `RT` constraints:

```csharp
where RT : HasRepositoryAccess<RT>
```

Capability access may then expose focused effect values:

```csharp
RepositoryAccessEnv<RT>.identities
```

Do not constructor-inject repositories into Features and do not hide required capabilities behind an untyped service bag.

## 8. Compose operations over effectful capability access

If resolving a capability returns an effect such as:

```csharp
Eff<RT, IIdentityRepository<RT>>
```

provide focused extension operations when this removes repetitive bind plumbing:

```csharp
extension<RT>(Eff<RT, IIdentityRepository<RT>> ma)
{
    public Eff<RT, SrvIdentity> Create(SrvIdentity identity) =>
        ma.Bind(repository => repository.Create(identity));
}
```

This allows application flows to stay declarative:

```csharp
entity => RepositoryAccessEnv<RT>.identities.Create(entity)
```

Inside such extensions, use the bound value. Do not accidentally recurse on the original effect.

## 9. Service Feature claims

A `ServiceFeature` exposes the `ServiceClaim` required to execute it.
Use one stable claim value per concrete capability.

```csharp
public sealed class CreateSomethingClaim :
    ServiceClaim,
    Const<CreateSomethingClaim>
{
    public static CreateSomethingClaim Value { get; } = new();

    public override string Service => "Service";
    public override string Capability => "CreateSomething";
}
```

And on the Feature:

```csharp
public static ServiceClaim ExecutableBy => CreateSomethingClaim.Value;
```

The claim represents authorization semantics. The Feature represents executable behavior.

## 10. Naming

Use these terms consistently:

```txt
Feature         general executable application behavior
ServiceFeature  service behavior authorized by a ServiceClaim
ProductFeature  product behavior authorized by a ProductRole
ServiceClaim    service capability used for authorization
ProductRole     product-owned composition of service claims
```

Avoid `AppClaim` and `AppRole`.

## Canonical service example

```csharp
public sealed class CreateIdentity<RT> :
    ServiceFeature<CreateIdentity<RT>, RT, SrvIdentity.Input>
    where RT : HasRepositoryAccess<RT>
{
    public static string Name => "Identities.Create";

    public static ServiceClaim ExecutableBy => CreateIdentityClaim.Value;

    public static Flow<RT, SrvIdentity.Input, Unit> Get() =>
        Flow.request<RT, SrvIdentity.Input>() >>
        (input => input.Match(
            Natural: n => CreateNatural<RT>.Invariants.RunEff(n).MapSuper(),
            Legal:   l => CreateLegal<RT>.Invariants.RunEff(l).MapSuper())) >>
        (entity => RepositoryAccessEnv<RT>.identities.Create(entity).IgnoreF());
}
```

Treat this as the default shape, not a mandatory template. Deviate when the domain or execution semantics require it, and make the reason visible in the code.
