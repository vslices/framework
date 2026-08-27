# Feature Model

Features are the main executable unit of application behavior in VSlices.

A Feature represents one explicit slice of behavior with:

- one request type;
- one response type;
- one runtime capability contract;
- controlled effectful execution;
- explicit failure propagation.

A Feature is not a controller, handler, service class, manager, or presentation adapter.

For canonical code shapes, see `docs/code-patterns.md`.

## Core shape

The canonical type is:

```csharp
Feature<F, RT, REQ, RES>
```

and its executable definition is:

```csharp
Flow<RT, REQ, RES> Get()
```

Conceptually:

```txt
RT + REQ -> effectful RES
```

`RT` remains part of the Feature type because runtime constraints are part of the execution contract.
Do not move the runtime generic to `Get<RT>()` when the Feature needs type-level capabilities.

For Unit responses, use the reduced form:

```csharp
Feature<F, RT, REQ>
```

## Service and Product specialization

VSlices distinguishes general Feature execution from authorization semantics.

A service capability is modeled with:

```csharp
ServiceFeature<F, RT, REQ, RES>
```

and exposes:

```csharp
static abstract ServiceClaim ExecutableBy { get; }
```

A product behavior is modeled with:

```csharp
ProductFeature<F, RT, REQ, RES>
```

and exposes:

```csharp
static abstract ProductRole ExecutableBy { get; }
```

This keeps `Feature` general while allowing service and product execution models to specialize it.

## Request and runtime are different channels

A `Flow` receives two independent inputs:

```txt
RT  = runtime capability carrier
REQ = request for this execution
```

Use the canonical readers:

```csharp
Flow.runtime<RT, REQ>()
Flow.request<RT, REQ>()
```

Do not obtain the request through `asks` or a runtime capability.
The request is not part of `RT`.

## Runtime capabilities

Features declare only the runtime capabilities they require.

Prefer:

```csharp
public sealed class CreateIdentity<RT> :
    ServiceFeature<CreateIdentity<RT>, RT, SrvIdentity.Input>
    where RT : HasRepositoryAccess<RT>
```

Avoid broad runtime constraints that contain unrelated capabilities.

Capabilities remain type-level requirements. Application code may expose focused effect values from them, for example:

```csharp
RepositoryAccessEnv<RT>.identities
```

Do not constructor-inject repositories or hide runtime requirements behind generic service objects.

## Feature body

Prefer a small declarative pipeline:

```csharp
public static Flow<RT, Request, Unit> Get() =>
    Flow.request<RT, Request>() >>
    (request => Validate(request)) >>
    (value => Persist(value).IgnoreF());
```

Use `>>` for linear composition when each step naturally consumes the previous result.

Do not extract `Execute`, `Core`, or similar wrapper methods when their only purpose is to preserve imperative structure.
Extract behavior when the name adds domain meaning, the operation is reused, or the local pipeline becomes harder to understand.

## Closed domain variants

When a request or domain type models a closed sum and provides `Match`, prefer it over a C# type switch:

```csharp
input.Match(
    Natural: n => CreateNatural<RT>.Invariants.RunEff(n).MapSuper(),
    Legal:   l => CreateLegal<RT>.Invariants.RunEff(l).MapSuper())
```

This gives one canonical expression for sum-type elimination.

Use a C# `switch` when no domain `Match` exists or when the pattern requires semantics that `Match` cannot express.

## ReqK execution

When a completed `ReqK<Eff<RT>, ...>` is executed from application code, use:

```csharp
rules.RunEff(input)
```

`RunEff` communicates execution. Do not use the former `ToEff` naming for this operation.

## Upcasting effectful domain values

When alternative branches produce domain subtypes and the next step consumes their common supertype, use a focused pure helper such as:

```csharp
.MapSuper()
```

instead of repeating generic identity maps such as:

```csharp
.Map<SrvIdentity>(x => x)
```

Such helpers must remain pure functor mappings and must not introduce behavior.

## Service Claims

A `ServiceFeature` exposes the stable claim that authorizes it.

Canonical shape:

```csharp
public sealed class CreateIdentityClaim :
    ServiceClaim,
    Const<CreateIdentityClaim>
{
    public static CreateIdentityClaim Value { get; } = new();

    public override string Service => "Identities";
    public override string Capability => "Create";
}
```

and:

```csharp
public static ServiceClaim ExecutableBy => CreateIdentityClaim.Value;
```

The Feature owns executable behavior. The Claim owns authorization identity.

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

The default reading is:

```txt
request
-> eliminate request variant
-> enforce contextual invariants
-> normalize to the consumed domain type
-> execute capability-backed effect
```

## Presentation adapters

Presentation adapters should only:

1. receive external input;
2. translate it into `REQ`;
3. execute the Feature;
4. translate the result into the presentation response.

Features must not contain HTTP, UI, worker, or transport-specific concerns.

## Side effects

Side effects occur through explicit runtime capabilities and effect values.

Do not perform uncontrolled side effects inside domain methods or static helpers.

Repository, clock, transaction, logging, external API, and similar access should remain visible in the capability requirements or effect composition.

## Expected failures

Expected domain and application failures must remain explicit values.

Do not throw exceptions for normal failure paths.
Preserve error meaning across `Req`, `ReqK`, `Eff`, and `Flow` composition.

## Testing

Test Features through the same `Flow` contract used in production.

Prefer test runtimes that implement the required capabilities over mocked service classes.

A test should exercise:

```txt
RT + REQ -> Flow result
```

## Rules

A Feature must:

- use `Flow<RT, REQ, RES>` as its execution model;
- keep `RT` constraints explicit;
- read `REQ` through the Flow request channel;
- keep orchestration local and readable;
- keep side effects controlled;
- remain presentation-independent.

A Feature must not:

- hide capability requirements;
- resolve dependencies through service locators;
- use constructor-injected service dependencies;
- treat the request as part of the runtime;
- introduce wrapper methods that add no semantic meaning;
- throw exceptions for expected failures.

## Deprecated model

Do not introduce new usages of `FeatureEff<RT, A>`.

New Feature execution code uses:

```txt
Flow<RT, REQ, RES>
```
