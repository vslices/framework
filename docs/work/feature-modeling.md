# Feature Modeling Laws

A Feature owns a nominal `Request`, a nominal `Response`, and an executable operation `static Eff<ALG, Response> Get(Request)`.

The request is proposed by the caller; it must be publicly constructible and does **not** need to implement `Transformable`. The response belongs to the Feature; its constructor is private and the Feature uses a private `NewResponse` constructor accessor. This is encapsulation, **not** proof that an effect completed or that the response is authorized.

The laws below are structural and inspected with `FeatureModelingLaws.Inspect(Type)` or `InspectDefinitions(Assembly, namespace)`.

## FM001 — Feature identity
Exactly one implemented `Feature<F, ALG, REQ, RES>` with `F == SELF`.

## FM002 — Request ownership
`Request` is declared directly inside the Feature.

## FM003 — Response ownership
`Response` is declared directly inside the Feature.

## FM004 — Nominal binding
The generic `REQ` and `RES` parameters equal the nested `Request` and `Response` types.

## FM005 — Request construction
`Request` is publicly accessible and has a public instance constructor.

## FM006 — Response construction
`Response` is publicly accessible, but every instance constructor is private.

## FM007 — Execution contract
A public static `Get(Request)` returns exactly `Eff<ALG, Response>`.

## FM008 — Response factory
Exactly one private static `NewResponse` uses `UnsafeAccessorKind.Constructor`; its parameters match a private `Response` constructor.

## FM009 — Explicit optionality
Public members and constructor parameters of `Request` and `Response` must not be nullable C# types. Use `Option<A>` only when absence is meaningful. This does not inspect nested generic arguments.

## FM010 — Response immutability
`Response` exposes no public setters (including init-only setters), or mutable public instance fields. This is a shallow structural check: it does not prove deep immutability of referenced objects.

## Behavioral responsibilities outside reflection

- **FM011 — Transport independence:** avoid transport-specific contracts in Feature's public model.
- **FM012 — Result ownership:** construction policy does not prove the operation has completed; explicitly test actual successful and failed effects.
- **FM013 — Explicit composition:** call child Features with explicit requests and explicitly project module algebras. Do not infer transactions or compensation from `Eff`.

These are review/testing obligations, not automatically enforced by `Inspect`.

## Migration from Flow

The former contract `static Flow<ALG, Request, Response> Get()` becomes `static Eff<ALG, Response> Get(Request request)`. The request is applied when `Get` is called; the returned effect retains the algebra until execution. `Flow` remains available independently in the core framework for other uses, but `MapRuntime`, `MapRequest` and `ContraMap` are not carried into the Feature contract.

Expected failures should be modeled as effect failures via `Error.New`, not as ordinary .NET exceptions. Exceptions are reserved for exceptional failures; broader migrations of existing point readers are distinct follow-up work.
