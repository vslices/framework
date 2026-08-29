# Feature Model

Features are the main executable unit of application behavior in VSlices.

A Feature represents one explicit slice of behavior with:

- one feature-owned request type;
- one feature-owned response type;
- one runtime capability contract;
- controlled effectful execution;
- explicit failure propagation.

A Feature is not a controller, handler, service class, manager, or presentation adapter.

## Core shape

C# does not currently expose associated types directly. VSlices therefore keeps the request and response as generic bindings while requiring the concrete Feature to own their nominal definitions.

Canonical shape:

```csharp
public sealed class SomeFeature<RT> :
    Feature<SomeFeature<RT>, RT, SomeFeature<RT>.Request, SomeFeature<RT>.Response>
{
    public sealed record Request(...);
    public sealed record Response(...);

    public static Flow<RT, Request, Response> Get() => ...;
}
```

Conceptually:

```txt
Feature
├── Request   associated contract
├── Response  associated contract
└── Get       RT + Request -> effectful Response
```

`Request` and `Response` belong nominally to the Feature even when they are composed almost entirely from domain types.

Prefer:

```csharp
public sealed record Request(SrvIdentity.Input Identity);
```

over treating the domain input itself as the Feature request.

This keeps two meanings separate:

```txt
Domain Input      data required to construct/refine a domain value
Feature Request   data required to execute an application operation
Feature Response  value produced by that application operation
```

`RT` remains part of the Feature type because runtime constraints belong to execution rather than to the request/response contract.

## Service specialization

A service-owned Feature is modeled with:

```csharp
ServiceFeature<F, RT, REQ, RES>
```

A concrete `ServiceFeature` must declare:

```csharp
static abstract string UniqueName { get; }
static abstract string Description { get; }
```

and receives a default claim through:

```csharp
static virtual ServiceClaim Claim
```

The default Claim is derived from the Feature metadata. A concrete Feature may override `Claim` only when it intentionally represents an already-established claim or requires exceptional claim construction.

`UniqueName` is the stable machine-facing identity of the service capability. `Description` is mandatory human/agent-facing metadata.

## ServiceClaim

`ServiceClaim` is a sealed value, not a subtype hierarchy.

Its identity is:

```txt
UniqueName
```

Its descriptive metadata is:

```txt
Description
```

`Description` does not participate in equality or hashing. It can evolve without changing the authorization identity.

Claims are created through:

```csharp
ServiceClaim.New<OWNER>(uniqueName, description)
```

The runtime registry is idempotent for the same owner and key, and throws when a different owner attempts to register the same `UniqueName` in the same process.

For generic Features, ownership is normalized to the open generic type so different runtime instantiations of the same Feature share one claim.

The runtime registry is a local safety mechanism. Global uniqueness across independently running services must eventually be validated by build/tooling that inspects the complete claim universe.

## Product specialization

A product behavior is modeled with:

```csharp
ProductFeature<F, RT, REQ, RES>
```

and exposes its product authorization through:

```csharp
static abstract ProductRole ExecutableBy { get; }
```

Service claims and product roles intentionally have different ownership semantics.

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

## Runtime capabilities

Features declare only the runtime capabilities they require.

```csharp
public sealed class CreateIdentity<RT> :
    ServiceFeature<
        CreateIdentity<RT>,
        RT,
        CreateIdentity<RT>.Request,
        CreateIdentity<RT>.Response>
    where RT : HasRepositoryAccess<RT>
```

Capabilities remain type-level execution requirements. Prefer focused capability access such as:

```csharp
RepositoryAccessEnv<RT>.identities
```

over constructor injection, service bags, or service-location patterns.

## Feature body

Prefer a small declarative pipeline:

```csharp
public static Flow<RT, Request, Response> Get() =>
    Flow.request<RT, Request>() >>
    Validate >>
    Persist;
```

Use `>>` for linear composition when each step naturally consumes the previous result. Use lambdas only when projection, matching, capture, or return adaptation is required.

## Canonical service example

```csharp
public sealed class CreateIdentity<RT> :
    ServiceFeature<
        CreateIdentity<RT>,
        RT,
        CreateIdentity<RT>.Request,
        CreateIdentity<RT>.Response>
    where RT : HasRepositoryAccess<RT>
{
    public sealed record Request(SrvIdentity.Input Identity);

    public readonly record struct Response;

    public static string UniqueName =>
        "Identities.Create";

    public static string Description =>
        "Permite crear una identidad";

    public static Flow<RT, Request, Response> Get() =>
        Flow.request<RT, Request>() >>
        (request => request.Identity.Match(
            Natural: n => CreateNatural<RT>.Invariants.RunEff(n).MapSuper(),
            Legal: l => CreateLegal<RT>.Invariants.RunEff(l).MapSuper())) >>
        (identity => RepositoryAccessEnv<RT>.identities
            .Create(identity)
            .Map(_ => new Response()));
}
```

The default reading is:

```txt
Feature.Request
-> eliminate/adapt request structure
-> enforce contextual invariants
-> execute capability-backed effect
-> construct Feature.Response
```

## Closed domain variants

When a request or domain type models a closed sum and provides `Match`, prefer it over a C# type switch.

## ReqK execution

When a completed `ReqK<Eff<RT>, ...>` is executed from application code, use:

```csharp
rules.RunEff(input)
```

## Presentation adapters

Presentation adapters should only:

1. receive external input;
2. refine/translate it into `Feature.Request`;
3. execute the Feature;
4. translate `Feature.Response` into the presentation response.

Features must not contain HTTP, UI, worker, or transport-specific concerns.

## Side effects and failures

Side effects occur through explicit runtime capabilities and effect values.
Expected domain and application failures remain explicit values.

Do not throw exceptions for normal failure paths. The duplicate-claim exception is a framework configuration/programming error, not a domain failure.

## Testing

Test Features through the same `Flow` contract used in production and prefer test runtimes that implement the required capabilities.

## Rules

A Feature must:

- own nominal `Request` and `Response` types;
- bind those types through the current generic interface until C# supports associated types;
- use `Flow<RT, Request, Response>` as its execution model;
- keep `RT` constraints explicit;
- keep orchestration local and readable;
- remain presentation-independent.

A ServiceFeature must additionally:

- expose a globally intended `UniqueName`;
- expose a useful `Description`;
- use the default derived `Claim` unless an exceptional override is semantically required.
