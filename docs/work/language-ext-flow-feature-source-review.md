# LanguageExt v5: Deriving, IO/Eff, Activity and the proposed Feature → Flow boundary

> **Status:** source review and design update, **not** an implemented API, a proven compatible wrapper, or a new Law.
> **Date:** 2026-11-14. **VSlices baseline:** Framework `master` inspected at `5431c91d77ce42d46b4d96c539972fa0c1a755d3`; `src/VSlices/VSlices.csproj` pins `LanguageExt.Core 5.0.0-beta-78`.
> **LanguageExt source:** `louthy/language-ext` `main` tree `54d27a2d76cf3f14b964ce1ab421ae80a080bb7a`; the critical sources (`Deriving.MonadUnliftIO`, `Eff<RT,A>`, and `LanguageExt.Sys.Diag.Activity`) were also **read directly at tag `v5.0.0-beta-78`**, with matching lengths and the same critical code checks (including the null-Activity exception). Executable tests are still required to establish runtime behavior. `LanguageExt.Sys` is a separate library, **not automatically present** because `LanguageExt.Core` is installed.
> **User direction:** observability is **first-party and built-in**, not an opt-in separate VSlices package. `Feature<F, ALG, REQ, RES>` still owns the use case, request, response, semantic/operational identity and Feature-specific options; its only prospective signature change is `Eff<ALG, RES>` → `Flow<ALG, RES>` (name provisional). `Flow` is the observable execution mechanism of a Feature, **not a general-purpose replacement for all Eff or a request-aware Feature**.

## Executive findings

1. **Correction to earlier uncertainty:** `Deriving` is real and substantial. [LanguageExt v5 beta-51 release](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-51) introduced `Deriving` specifically to implement traits through **natural isomorphisms** (`Transform`/`CoTransform`), avoiding repeated monad boilerplate. [`Deriving.Monad`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Deriving/Monad.cs) provides `Bind`, `Flatten`, `Recur`; [`Deriving.MonadIO`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Deriving/MonadIO.cs) supports `LiftIO`; [`Deriving.MonadUnliftIO`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Deriving/MonadUnliftIO.cs) delegates IO behavior, including retry/repeat/schedule/timeout/brackets/uninterruptible. A Flow wrapper need **not reproduce these algorithms**.
2. **A wrapper is not automatically observable:** derived `Monad.Bind` transforms both Flow operands down to Eff, composes them using Eff, then CoTransforms the resulting Eff. Runtime metadata and observation must survive that roundtrip; putting arbitrary `FlowDescriptor` fields on the wrapper is insufficient. Deriving gives trait implementation, **not preservation of Feature-specific options or a guaranteed span boundary**.
3. **`Eff<RT, A>` is already a `ReaderT<RT, IO, A>`:** observed in [source](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Effects/Eff/Eff%20with%20runtime/Eff.cs). `IO<A>` has an interpreted DSL, async/sync execution, automatic resource tracking, retries and schedules. Wrapping the whole Eff is more promising than writing a new interpreter.
4. **Activity implementation exists and supports explicit nesting:** [`LanguageExt.Sys.Diag.Activity<M,RT>.span`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/Sys/Diag/Activity.cs) uses `ActivitySourceIO`, `use` resource tracking and `Local.with` to install the current activity; tests verify nested `parentId` / `parentSpanId` and current activity. It **is not safe to adopt unchanged as universal built-in instrumentation**: it maps a null `ActivitySource.StartActivity` result to a thrown `NullReferenceException`, although null is the normal no-listener case. It also captures `DateTimeOffset.Now` while building a span expression, potentially before actual execution.
5. **The activity adapter is not the whole observability stack:** LanguageExt.Sys Activity wraps `System.Diagnostics.ActivitySource`; it does not provide a ready-made Feature lifecycle, structured `ILogger` or `Meter` instrumentation. VSlices needs minimal *native .NET* coordination at the execution boundary (not a parallel tracing/logging/metrics system).
6. **Feature identity/options remain Feature-owned.** Flow should receive a stable, low-cardinality execution description and apply execution mechanics. A descriptor can be supplied at Feature boundary, but the shape and propagation through `Bind` are unproven.
7. **Evidence scope:** source and upstream tests inspected; **no independent VSlices build, executable spike or multi-host trace experiment was run in this review**.

## 1. Historical and current contracts: do not confuse Flow variants

At the time inspected, VSlices uses:

```csharp
public interface Feature<F, ALG, REQ, RES>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Eff<ALG, RES> Get(REQ request);
}
```

Existing `src/VSlices/Monads/Flow/Flow.Definition.cs` still defines `Flow<ALG, RQ, A>` around `Func<ALG, RQ, IO<A>>` with `RunFlow(ALG, RQ)` and `RunEff(RQ)`, plus its own HKTs/traits and derive/compute composition. It is **not** the proposed `Flow<ALG,A>`, so implementing the new shape requires a deliberate migration/renaming plan (existing APIs/tests/callers, and package compatibility). `docs/handoffs/free-workflows-workprocess-handoff.md` is historical evidence and **not authority** over current Feature.

The desired public shape is provisional:

```csharp
// DESIGN ONLY: not compiled, not a supported LanguageExt API yet
public interface Feature<F, ALG, REQ, RES>
    where F : Feature<F, ALG, REQ, RES>
{
    static abstract Flow<ALG, RES> Get(REQ request);
}

// Flow<ALG, A> encapsulates an Eff<ALG, A>, not the Request.
```

A Feature constructs its use case and supplies its own declaration/options. Flow **only** executes the composed effect under an observable lifecycle. Do not force every Feature to write `Flow.Create("Operation", effect)` until a lower-ceremony, derived design has been tested.

## 2. `Deriving` examined in source

### Exact mechanism

[Release beta-51](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-51) calls the pattern `Deriving`, and shows `Game` wrapping `StateT<GameState, OptionT<IO>>` with:

```csharp
// Excerpt from upstream release, for orientation
public static K<Wrapped, A> Transform<A>(K<Wrapper, A> fa) => /* unwrap */;
public static K<Wrapper, A> CoTransform<A>(K<Wrapped, A> fa) => /* wrap */;
```

The actual interface is an alias of `NaturalIso`: [`LanguageExt.Core/Traits/Deriving/Deriving.cs`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Traits/Deriving/Deriving.cs). Code-level checks:

| Interface | Concrete behavior in source | Implication for Flow |
| --- | --- | --- |
| `Deriving.Functor<Flow<E>, Eff<E>>` *shape only* | `CoTransform(Subtype.Map(f, Transform(ma)))` | Generic `Map` inherited without manual boilerplate |
| `Deriving.Applicative<...>` | `Pure`, `Action`, `Apply` delegated | Generic applicative composition possible |
| `Deriving.Monad<...>` | `Bind`, `Flatten`, `Recur` delegated | Generic monadic composition and recursion possible |
| `Deriving.MonadIO<...>` | `LiftIO` delegated | Native IO effects lifted without bespoke interpreter |
| `Deriving.MonadUnliftIO<...>` | `MapIO`, local context, timeout, bracket, retry, repeat, fold, etc. delegated | Broad, existing operational vocabulary available |
| `Deriving.Fallible<...>`, `Deriving.Final<...>`, `Deriving.Readable<...>` | Error, finalization and environment support delegated | Potentially useful, require selected trait bounds and tests |

**Important generic shape:** Eff's HKT constructor is `Eff<RT>` with bound value `A`; Flow would need `Flow<RT>`, implementing `K<Flow<RT>, A>` as `Flow<RT,A>`. A possible witness declaration is `Deriving.MonadUnliftIO<Flow<RT>, Eff<RT>>`, **not** `Deriving.Monad<Flow<RT,A>, Eff<RT,A>>`; the declaration remains a *hypothesis until compiled*.

### What Deriving does NOT guarantee

`Monad.Bind` is implemented as:

```csharp
Supertype.CoTransform(
    Subtype.Bind(Supertype.Transform(ma), x => Supertype.Transform(f(x))));
```

This is decisive for metadata. If each `Flow<RT,A>` stores a `FeatureDescriptor` alongside its `Eff`, `Transform` strips that descriptor when exposing the underlying Eff, and `CoTransform` must decide which descriptor to attach to the composed operation. The generic `Deriving` trait has no mechanism to merge both descriptors. **Do not claim that inheriting Monad automatically preserves outer Feature metadata**.

Candidate designs for a spike:

- **Execution-boundary annotation (preferred to test first):** use Deriving for pure effect composition, then apply Feature-owned description/options once when the **outer Feature** is run. Do not treat subexpression `Bind` as a Feature execution.
- **Deferred metadata in underlying Eff:** wrap execution using an IO-local scope only when the boundary becomes known; test for nested retry and double decoration.
- **Custom override for selected operations:** only if a concrete algebraic law/property fails and we can prove consistent metadata propagation across `Bind`, `Map`, `Pure`, `Catch`, `Recur`.

The wrapper must satisfy monad laws and remain lazy; the form of `CoTransform` should not cause instrumentation at construction or erase necessary execution identity.

## 3. `Eff`, `IO` and existing operational behaviors

### Eff

[`Eff<RT,A>`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Effects/Eff/Eff%20with%20runtime/Eff.cs) is a `record` around `ReaderT<RT, IO, A>`, implementing `K<Eff<RT>,A>`. It supports `Lift`, `LiftIO`, `Map`, `SelectMany` (including other LanguageExt lifted types), `MapIO` and failure handling. The source distinguishes pure construction of an effect from actual `Run`/`RunIO` execution. Therefore *instrument at runtime*, not during Feature.Get.

### IO

[`IO<A>`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Core/Effects/IO/IO.cs) is an interpreted operational DSL. [Beta-42](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-42) rewrote it into DSL nodes interpreted by `Run`/`RunAsync`, with resource tracking; [beta-43](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-43) optimized the synchronous path. The source includes:

| Operation | Verified implementation / mechanism | Flow design pressure |
| --- | --- | --- |
| `IO.Retry`, `RetryWhile`, `RetryUntil` | On error, retry; scheduled overloads use `Schedule.PrependZero`, `yieldFor`, `BracketFail`, `Catch` | Determine whether attempts produce child observations or one aggregate execution; never silently retry non-idempotent writes |
| `IO.Repeat`, `RepeatWhile`, `RepeatUntil` | Repeats including **successful** effects, uses `Bracket` on iterations | This is not the same as retry; do not conflate the counters |
| `Schedule` | `recurs`, exponential/spaced and operators, `Run()` returns durations | Delay is part of operational runtime; no custom scheduler necessary |
| `IO.Timeout` | `IOTimeout` around EnvIO local cancellation/timeout context | Cancellation must be observed distinctly from ordinary error |
| `IO.Uninterruptible` | `IOUninterruptible` runs its inner effect under local `EnvIO` with `CancellationToken.None` | Effect may outlive outer cancellation; no unconditional “cancellation means execution already stopped” claim |
| `IO.Bracket` / `BracketFail` / `use` | Managed acquisition and disposal via EnvIO resource table | Activity scopes can use existing disposal mechanism but must be tested under exception/cancel |
| `IO.Fork` | Forks effect under separate resource/cancellation context | Check Activity.Current propagation and scope isolation under concurrency |
| `IO.RunAsync` | Sync fast path plus async interpreter; top-level resource cleanup | Avoid wrapping with gratuitous Task allocation; instrument both sync and async |
| `Memo` (separate type) | `Memo<A>` and `Memo<M,A>` infrastructure, plus trait `Memo` overloads | **Not proof that an entire IO/Eff execution is memoized safely by default**; semantics, cache scope, failure caching and retry interaction require a dedicated experiment |
| `Monad.Recur` | Generalized tail recursion | Borrow via Deriving rather than reimplement recursion |

[Beta-61](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-61) specifically describes `UninterruptibleIO`, timeout improvements and `awaitAny` canceling other computations by default; [beta-64](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-64) adds generalized `Monad.Recur`.

**Interpretation:** Flow does not need to own alternative retry, scheduling, timeout, or resource-management algorithms. It does need to define the **observation semantics of these operations** when they are part of a Feature's Flow. Feature policy may specify whether a given use case is eligible for retry, but mechanism stays with Eff/IO.

## 4. Actual LanguageExt Activity implementation and nested spans

Inspected sources:
- [`Activity<M,RT>`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/Sys/Diag/Activity.cs)
- [Eff-specialized `Activity<RT>`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/Sys/Diag/Activity.Eff.cs)
- [`ActivitySourceIO` contract](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/Traits/ActivitySourceIO.cs)
- [Live `ActivitySourceIO` implementation](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/Live/Implementations/ActivitySourceIO.cs)
- [`ActivityEnv`](https://github.com/louthy/language-ext/blob/main/LanguageExt.Sys/ActivityEnv.cs)
- [upstream Activity tests](https://github.com/louthy/language-ext/blob/main/LanguageExt.Tests/Sys/Diag/ActivityTests.cs)

### Positive evidence

`Activity<M,RT>` uses constraints `RT : Has<M,ActivitySourceIO>, Local<M,ActivityEnv>` and is designed for **explicit spans around effects**:

```csharp
// Representative of actual structure; omitted overload arguments
from activity in startActivity(name, ...)
from result in Local.with<M, RT, ActivityEnv, A>(
    env => env with { Activity = activity }, operation)
select result;
```

`startActivity` gets the source and current activity, uses the current activity's `ActivityContext` (or supplied parent context) and acquires the new Activity through `use`. Actual upstream tests include:
- `Case13` current activity is some inside a span; `Case14` none outside;
- `Case15` nested `parentId`; `Case17` nested `parentSpanId`;
- `Case12` links, `Case28` explicit parent/tags;
- span cases check method/kind/metadata.

This **demonstrates source-level intent and existing upstream tests** for nesting, not VSlices-specific production compatibility. In particular, the generic `Local` environmental model can be heavy for a Feature whose `ALG` is an intentionally minimal operation capability interface: Feature algebras must not acquire diagnostics plumbing solely to make an Activity wrapper compile.

### Critical limitations / mismatch with built-in observability

1. **No-listener failure path.** Native `ActivitySource.StartActivity` returns null if there is no interested listener. The live adapter returns `IO<Activity?>`. But `Activity<M,RT>.startActivity` converts null to **`NullReferenceException("Activity is null")`**, after `Map`. Thus placing `Activity.span` around **every** Feature without an active collector/listener can break otherwise successful execution. Built-in Flow must work without any listener or exporter. This is a concrete source-level compatibility blocker, **also confirmed in the `v5.0.0-beta-78` tag source**. Execute the no-listener regression on that version before deciding whether to fix upstream or avoid this adapter.
2. **Timestamp captured during construction.** The simple `span` overload calls `DateTimeOffset.Now` as an eager C# argument before execution; for a lazy Flow, the timestamp must reflect **actual run** and be fresh on repeated executions. The source passes `startTime` through to `StartActivity` (instrumentation validity requires testing).
3. **Disposal and nested ownership require validation.** `ActivityEnv.Dispose()` disposes **both** `ActivitySource` and `Activity`. Nested `ActivityEnv` records may carry a shared source. The actual resource lifecycle is controlled by `Local.with` and `use` and needs tracing with two nested spans plus a shared source, including thrown failures/cancellation. We cannot infer safe source ownership from the passing parent-ID tests alone.
4. **Runtime constraint leakage.** `Has<M, ActivitySourceIO>` and `Local<M,ActivityEnv>` require the caller's RT/algebra to expose these capabilities. This conflicts with VSlices FM011's minimal Feature-specific `Algebra` if imposed universally. Prefer a Flow-native execution boundary that uses `ActivitySource` / `Activity.Current` without adding diagnostics traits to every Feature algebra.
5. **Doesn't cover all three signals.** Existing Activity code is a tracing abstraction; `ILogger` and `Meter` need their own straightforward .NET integration. No need to replicate an alternative Activity implementation if standard .NET suffices.
6. **Explicit parent and ambient parent are not identical experiments.** Upstream tests explicitly pass parent contexts in nesting cases. Need another test for implicit `Activity.Current` nesting and async/fork propagation under no exporter.

### Preliminary decision

**Reuse the platform's `System.Diagnostics.ActivitySource` and ambient propagation directly for the first VSlices Flow spike; learn from LanguageExt.Sys's `span` / local context pattern, but do not make `LanguageExt.Sys.Diag.Activity.span` the required mechanism as-is.** This is a provisional engineering preference, not an implementation decision about every future consumer. Consider an upstream fix or adapter reuse if behavioral tests demonstrate safe no-listener execution, correct nesting, cleanup and compatibility with minimal algebras.

## 5. Proposed Flow execution lifecycle, including retry

```text
Feature.Get(Request)   → Flow<ALG, RES> (description only)
Flow execution attempt → scope + Activity + start timestamp
                       → underlying Eff/IO execution
                       → observe technical success / failure / cancellation
                       → finalize span, metrics, logs and resources
                       → Fin<RES> or relevant IO result
```

Clarify **execution vs attempts**:

- Outer logical Feature execution: one root **operation observation** (one duration histogram observation and one completion outcome).
- Retries within that execution: potentially **attempt events or child spans**, only if the Feature's selected policy and actual retry mechanism expose a reliable per-attempt hook. Do not manufacture retry counts by parsing an error or assuming `RetryIO` reports iterations.
- Nested **explicit Feature executions**: candidate child spans. Ordinary `Eff.Bind`/IO steps do not automatically become child spans.
- Re-running a constructed Flow: independent scopes and timestamps on each run; no cached “already completed” flag unless a separate, explicit semantic contract demands it.
- Failure taxonomy: `Fin.Fail`, thrown exception, timeout/cancel, and a successful `RES` carrying a business-level rejection are distinct. Default telemetry cannot inspect arbitrary response meaning.
- Cleanup: `try/finally` or resource bracket must run on success, failure and cancellation; collector absence must not interrupt execution.
- Performance/privacy: bounded stable labels from Feature, no serialization of Request/Response/identity/tokens as default metadata.

**Ordering matters:** retry *inside* the outer Flow observation means one Feature operation with several attempts; retry *outside* that observation means multiple logical executions. The implementation must make this intentional and testable; simply applying `Deriving.MonadUnliftIO` does **not** settle placement.

## 6. Design options and where to put Feature-owned configuration

`FlowDescriptor` and `FlowOptions` are **not** settled names:

| Concern | Owner | Expected behavior |
| --- | --- | --- |
| Feature identity and operation name | Feature declaration, potentially derived from `F` | Stable, low-cardinality; no request-sensitive label |
| Feature-specific business response classification | Feature | Explicit, typed policy; not inferred from arbitrary `RES` |
| Retry eligibility/idempotency assumptions, when selected | Feature or owning use case policy | Never silently retry writes |
| Execution scope start/stop, instrumentation and resource cleanup | Flow | Built-in in Framework; no separate NuGet to turn on |
| Native `ActivitySource`, `Meter`, `ILogger` usage | Flow's execution mechanism / nearby internal implementation | Native .NET signals; no mandatory OTel exporter |
| Sampling, exporters, collection, retention and privacy policy | Hosting | Operational configuration; execution still works without collectors |

Experiment with a Feature declaration that owns descriptor/options **without requiring an extra `Flow.Create(...)` wrapper** in every Feature; no syntax should be advertised as working until compiled.

## 7. Minimal executable experiment (next step; NOT performed by this document)

Use `SampleWorkflow.GetTodo` or `CreateTodo` with a tiny prototype of `Flow<RT,A>` wrapping `Eff<RT,A>`, pinned to `LanguageExt.Core 5.0.0-beta-78`.

**Deriving and HKT:** compile `K<Flow<RT>, A>`, `Deriving.MonadUnliftIO<Flow<RT>,Eff<RT>>`, `Transform`, `CoTransform`; demonstrate `Map`, `Bind`, LINQ, `LiftIO`, `retry`, `uninterruptible`, `timeout`, and `recur`. Test `Fallible`/other traits separately. Verify compile-time generic constraints and extension dispatch; source-level availability is not enough.

**Observation:** no listener (effect succeeds); just constructing (no spans); one execution (one span); repeat execution (two spans); nested explicit Feature calls (parent/child); `Bind` substeps do not multiply spans; async/fork does not leak `Activity.Current`; cancellation/disposal; error and `Fin` classification; retry schedule and attempt metrics; bounded tag values.

**LanguageExt.Sys comparison:** run same nested scenario using `Activity.span` from the package/version actually installed or a matching source checkout, first with a listener and then without; record whether the null behavior, start times and source disposal reproduce the source concerns. Do not ship a new VSlices Flow public API or Law until this experiment passes.

## Source links and provenance

- Framework current Feature: [Feature.cs](https://github.com/vslices/framework/blob/master/src/VSlices.Work/Feature.cs); [existing Flow](https://github.com/vslices/framework/blob/master/src/VSlices/Monads/Flow/Flow.Definition.cs).
- LanguageExt official release notes: [beta-51 Deriving](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-51), [beta-42 IO DSL](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-42), [beta-43 IO execution](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-43), [beta-61 uninterruptible/timeout](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-61), [beta-64 Monad.Recur](https://github.com/louthy/language-ext/releases/tag/v5.0.0-beta-64).
- LanguageExt source files linked inline above; reviewed upstream main tree `54d27a2d76cf3f14b964ce1ab421ae80a080bb7a`.


## 8. Agreed tracing semantics: implicit nesting via .NET execution context (2026-11-14)

> **Design decision recorded from discussion; implementation not yet verified in VSlices.** This section supersedes the earlier question about manually declaring Feature-to-Feature parentage. No `NestedIn<A>()` or explicit parent/child declaration is required or desired.

### Agreed intent and telemetry ownership

- `Feature.Get(request)` **represents an intention** and produces a Flow without starting telemetry. The declaration itself need not produce an Activity.
- **Executing** the Flow starts an **implicit span** for that actual Flow execution, within whatever ambient context exists. A repeated run is a new span, not a mutation of an earlier one.
- **Logs are explicit** operations requested by a Feature, using .NET `ILogger` and inheriting correlation from the active Activity.
- **Metrics are explicit** measurements requested by a Feature, using native `System.Diagnostics.Metrics` instruments. Framework should not silently emit business counters/histograms; the standard Flow trace is the automatic observation.
- A single trace can contain **multiple spans**. “One trace per Flow” is imprecise: the agreed guarantee is **one implicit execution span per Flow run**, potentially part of an existing distributed trace.
- Feature retains semantic identity, options, Request/Response and use-case meaning; Flow is the mechanism of execution/observation. No diagnostics methods must be added to each Feature.Algebra.

### Nesting is implicit, including cross-server HTTP

The execution tree is determined by **actual distributed tracing context**, not compile-time coupling, declared Feature parentage, or static service topology.

```text
ticket-support-product (ASP.NET Core inbound Activity)
  -> Feature AttachFile (Flow execution span, Activity.Current child)
     -> HttpClient outgoing HTTP client span
        -> traceparent / tracestate propagation
           -> ticket-service (ASP.NET Core inbound server span)
              -> Feature StoreFile (Flow execution span, Activity.Current child)
```

The `ticket-service` Feature **need not know** `ticket-support-product` nor declare that it is nested beneath it. When correctly configured, outgoing HTTP instrumentation injects W3C Trace Context; inbound ASP.NET Core instrumentation extracts it. The receiving server's Activity and Flow Activity are correlated by the propagated context. No custom VSlices parent identifier or HTTP header format is warranted.

### Engineering constraints to prove

1. **Execution-time boundary:** start `ActivitySource.StartActivity(operationName, ActivityKind.Internal)` when Flow actually runs, using the ambient parent (`Activity.Current`); do not start at `Feature.Get` or while deriving/composing `Bind`/ `Map`.
2. **Listener absence:** `StartActivity` can return `null`; Flow must still execute and complete. Do not reuse `LanguageExt.Sys.Diag.Activity.span` unchanged given its verified null-to-throw path.
3. **Parent inheritance:** test in-process parent/child and detached execution. Parent span should be preserved/restored after nested completion.
4. **HTTP propagation:** verify actual outgoing HttpClient headers and inbound ASP.NET Core correlation using two local hosts, with tracing configured. A bare HTTP call without appropriate .NET instrumentation/configuration must not be claimed to guarantee nesting.
5. **Sampling:** absence of a recorded child span (due to sampling/listeners) does not mean the operation did not execute. Clarify whether a propagation-only Activity/current context exists for all configured cases.
6. **Async/cancellation/retry:** verify `Activity.Current` across awaits and forked work, exceptional paths, cancellation, and completed/finalized resource scopes. Retry inside a logical Flow may produce one Flow span over several attempts; don't automatically create new Feature spans for `IO.Retry` iterations.
7. **Structured logs and metrics:** confirm explicit logs correlate with Activity and explicit instruments remain low-cardinality and do not serialize request/response data automatically.
8. **No extra public nesting API:** the mechanism should remain transparent to Feature declarations. Hosting owns transport instrumentation/exporter setup, while Framework owns Flow instrumentation availability by default.

### Consequence for API design

Do **not** design `FeatureA.Nest(FeatureB)`, `FeatureB.NestedIn<FeatureA>()`, or require parent identifiers in `FlowDescriptor`/Feature options. Use the framework's ActivitySource and conventional .NET / W3C propagation; use explicit Feature options only for *meaningful semantic or emission policy*, not for implicit trace hierarchy.

The exact creation/execution method name, logger provisioning and instrumentation wiring are still research questions. This is a design agreement, not a claim that a Flow prototype, HTTP integration test or exporter configuration is already implemented.
