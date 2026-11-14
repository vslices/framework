# Work observability — research and executable-evidence plan

> **Direction update (2026-11-14):** subsequent source research adopts **first-party built-in Flow observability** as the design hypothesis, rather than a separate optional NuGet package. Feature remains the semantic owner; prospective `Feature.Get` returns `Flow<ALG, RES>` (instead of `Eff<ALG, RES>`), and Flow only owns observable execution. See [LanguageExt Deriving / IO / Eff / Activity source review](language-ext-flow-feature-source-review.md). The four approaches below remain historical alternatives for comparison and are **not** the currently preferred optional-package direction.


> Status: **investigation / proposal**, not an adopted public API, implementation, or VSlices Law.
> Based on `master` at `a94717447aaf7388ae97da5add56e6dcc982dbf3` (2026-11-14).
> This document separates verified existing code, platform facts, proposed directions, and evidence still missing.

## The real problem

Work currently models a Feature as `Feature<F, ALG, REQ, RES>` with a static `Get(REQ)` returning `Eff<ALG, RES>`. The Feature owns the operation's intent; its `ALG` expresses required executable capabilities. Grounding realizes mechanisms; Hosting configures runtime use. Nothing currently specifies how the execution of a Feature is observed across logs, distributed traces and metrics.

We want first-class **availability** of observability in VSlices Work without assuming every Feature must request an `ILogger`, a tracer, a metrics sink, a transport, or a vendor. Observability must not redefine domain outcomes, silently introduce side effects into pure construction, or require every consumer to install OpenTelemetry.

### Confirmed repository facts

- `src/VSlices.Work/Feature.cs` declares `static abstract Eff<ALG, RES> Get(REQ request)`.
- `src/VSlices.Work/Algebra/Clock.cs` and `Delay.cs` are Work operation contracts; `src/VSlices.Grounding/SystemTimeIO.cs` is a concrete realization.
- `VSlices.Work.DataAccess` distributes optional point algebras independently from `VSlices.Work`, while their C# namespace remains `VSlices.Work.Algebra`.
- `FeatureModelingLaws` checks Feature structure (including Feature-local `Alg`/`Algebra`) but does not presently define observability behavior.
- Current `Get` usage constructs effect descriptions; the code executing an `Eff` does so later by supplying its runtime algebra (e.g. `.RunIO(algebra)`). This is the point where execution can be instrumented.

### Confirmed .NET platform surface

.NET already supports the three signals:
- **Logs**: `Microsoft.Extensions.Logging.ILogger`, structured templates and optional scopes; source-generated logging is preferred for high-volume strongly typed events.
- **Traces**: `System.Diagnostics.ActivitySource` starts an `Activity` (an OTel span); there may be no activity when no interested listener is present.
- **Metrics**: `System.Diagnostics.Metrics.Meter` creates counters/histograms; histogram is commonly appropriate for durations.
- **OpenTelemetry** typically acts as a collector/exporter via those native .NET APIs. Producing instrumentation in a reusable library does not require binding that library to a specific exporter.

Sources (consulted 2026-11-14):
- https://learn.microsoft.com/en-us/dotnet/core/diagnostics/observability-with-otel
- https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-instrumentation-walkthroughs
- https://learn.microsoft.com/en-us/dotnet/core/diagnostics/metrics-instrumentation
- https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/library-guidance
- https://opentelemetry.io/es/docs/languages/dotnet/instrumentation/

## Separate four different responsibilities

| Concern | Proposed owner / boundary | Not to confuse with |
|---|---|---|
| Meaning of an operation and its **semantic** result | Feature / Work, and Spaces for domain meaning | Transport status, telemetry sink |
| Decision to instrument *actual execution* and stable operation identity | A Work-level execution boundary/utility or optional Work observability capability | Merely calling `Get(request)` |
| Production of logs, spans, measurements through native .NET APIs | A concrete, testable adapter / instrumentation module | Exporter, collector or dashboard |
| Export, filtering, sampling, redaction policy, resource identity and service setup | Hosting / deployment | Definition of the Feature or its `ALG` |

These boundaries remain proposals; the minimal workable mechanism is not yet proved.

## Candidate designs to compare

### A. Instrument the execution boundary (preferred first experiment)

Provide an **opt-in** wrapper around the execution of an `Eff<ALG, RES>` with a stable, explicit operation name and an observation policy. The caller or Work executor supplies the algebra and execution context. This does not add observability members to every Feature's `Algebra`.

**Advantage:** one cross-cutting instrument can collect duration, completion, error/cancellation information with consistent correlation; a Feature does not need to ask for logging to be observable. **Limitation:** it does not infer domain-specific outcomes merely from `RES`; Feature-local events need a distinct explicit mechanism.

Key semantic test: the wrapper must instrument the effect **when run**. Calling `Feature.Get(request)` alone must not start an `Activity`, log completion, or record a metric. Re-running the same effect should generate distinct observations. Exceptions/failures/cancellation must not generate fake success.

Sketch only (NOT an agreed or verified API):

```csharp
// Conceptual usage; signature is deliberately undecided.
var effect = CreateTodo.Get(request);
var observed = ObserveExecution("SampleWorkflow.CreateTodo", effect);
var response = await observed.RunIO(algebra).RunAsync();
```

Investigate wrapping at the Eff/IO boundary versus a helper receiving the complete executable delegate. Both may differ in cancellation and environment propagation, and LanguageExt v5 must be checked by real tests.

### B. Explicit Feature-level capabilities for meaningful events

A Feature can request a purpose-specific diagnostic/event algebra through its own `Feature.Alg`, but **only** if the Feature intends a meaningful explicit observation beyond generic execution. These are operations defined by Work; a concrete Grounding can interpret them using `ILogger` or other mechanisms.

**Advantage:** precision about event meaning and placement, testable without infrastructure. **Risk:** mechanically adding `Logger`, `Tracer`, or `Metrics` to every `Feature.Alg` leaks vendor/runtime vocabulary and raises accidental complexity. For this reason, do not make a universal `HasLogger`/ `HasTracer`/ `HasMeter` mandatory.

An important distinction: an **operational log/trace event** is not automatically a **domain event**, business fact, audit record or contractual notification. Audit requirements must not be satisfied through best-effort telemetry alone.

### C. Direct native .NET instrumentation inside Work

Expose or use `ActivitySource`, `Meter` and `ILogger` directly in the Work package.

**Advantage:** no new abstraction for concepts already supported by .NET, easy OpenTelemetry integration. **Risks:** mandatory `Microsoft.Extensions.Logging` dependency if embedded in `VSlices.Work`; unclear control of operation names and automatic instrumentation boundaries; tests couple to ambient activities; library-wide instrumentation might be too eager. Investigate whether a small dedicated package (e.g. `VSlices.Work.Observability`) is justified by actual consumer pressure, analogous to `VSlices.Work.DataAccess`. The package name is **not** a decision.

### D. Automatic instrumentation from conventions / reflection / generated code

Potentially attach instrumentation to Feature execution based on naming or generated accessors.

**Advantage:** minimal repetitive call sites. **Risks:** reflection cannot establish semantic intent or whether `Get` is merely assembling a lazy effect; intercepting the wrong boundary creates misleading telemetry. Defer until a concrete execution hook and stable convention are demonstrated. Do not add a new Law as a substitute for behavior evidence.

## First-party does not mean compulsory

Proposed meaning of first-party: VSlices supplies an explicit, documented, tested and composable mechanism for instrumenting Work execution. It is discoverable and supported within Framework. It **does not mean** every `VSlices.Work` consumer necessarily takes a tracing vendor dependency or emits telemetry by default.

A useful initial distribution could be either:
- Native reusable Work instrumentation with no required exporter plus optional Hosting wiring; or
- `VSlices.Work.Observability` with the same `VSlices.Work` core unchanged, if a separate dependency is warranted.

Compare package boundaries through a real consumer. Keep package ID, CLR namespace, and runtime responsibility distinct.

## Semantics and safety criteria to decide before API commitment

1. **Operation identity:** stable, bounded operation name (`Module.Feature` or explicitly supplied), not unbounded request values or dynamic resource identifiers. Decide naming and versioning from consumer examples.
2. **Execution timing:** start on execution, stop on completion, including cancellation and exceptional paths. An `Eff` can be executed more than once; each execution must have its own scope.
3. **Failure classification:** distinguish infrastructure exception, cancellation, and a successfully produced `RES` expressing a domain failure. No generic utility can classify arbitrary `Either`/`Fin`/`Option` hidden in `RES` without an explicit classifier.
4. **Trace correlation:** preserve `Activity.Current` parent-child context across asynchronous `IO` and `Eff`; observe no-listener behavior; avoid a span around *construction*.
5. **Log correlation:** logs emitted inside an active Activity should carry correlation; choose structured event names and categories, not ad hoc string interpolation. Logging scopes must dispose reliably.
6. **Metrics:** start with low-cardinality operation counter and duration histogram (unit seconds), with a limited result classification; do not put request IDs, user names, account IDs, arbitrary exception strings or high-cardinality values in metric labels.
7. **Sensitive data:** no automatic serialization of `REQ`, `RES`, identities, tokens, credentials, domain objects or exception stack traces to telemetry. Use explicit approved fields and Hosting policy.
8. **Noninterference:** observations must not change business outcomes or create false success. Decide explicitly whether and how telemetry callback failure is isolated from Work execution; do not silently swallow bugs in an adapter without an accountable policy.
9. **Costs:** behavior without collectors/listeners must remain functional and cheap; inspect allocations and high-throughput behavior before introducing global middleware.
10. **Ownership:** Work names the executable operation; Grounding can realize optional operational emissions; Hosting chooses collection/export and diagnostics policy. Do not turn Hosting into a mandatory runtime for unit-testable Features.

Platform references: .NET ActivitySource instrumentation, .NET metrics instrumentation and recommended tag cardinality, and logging library guidance (links above).

## Controlled experiment (proposed next step, not yet executed)

Use **one existing Feature** in `examples/SampleWorkflow` (e.g. `CreateTodo`) and record evidence in tests with in-memory `ActivityListener`, `MeterListener` and an in-memory `ILogger` provider; no external APM needed.

| Scenario | Expected evidence |
|---|---|
| Call `Get` without executing | No operation span, metric, or completed log |
| Execute success once | Exactly one complete operation observation; underlying operation executed once |
| Re-run same effect | Two independent executions, not one shared span |
| Existing parent Activity | Child is correlated and parent remains intact |
| No listener/collector | Same returned value, no exception caused by absent observability infrastructure |
| Unhandled runtime error | Failed execution observed, no fabricated success |
| Cancellation | Correct disposal, no fake success; propagated cancellation |
| Response carries domain rejection | Correct classification via explicit policy, not arbitrary guessed outcome |
| Nested Feature execution | Verify whether one parent with child spans is intended; no accidental duplicated root counts |
| Distinct concurrent requests | No cross-request scope/Activity leakage |
| Safe tags | No sensitive or high-cardinality request values |
| Performance comparison | Measure overhead on enabled and disabled instrumentation paths |

Compare **A** (execution decorator) against **B** (explicit Feature-specific operational observations), then decide if a small dedicated package, optional Host wiring, or both are justified. Avoid adding an API to the default package until the test explains why it belongs there.

## Open questions

- Where is the actual shared execution boundary for `Eff<ALG, RES>` in VSlices consumers: explicit `RunIO`, host-side orchestration, or a reusable effect combinator?
- Should the generic wrapper classify a result only as execution success/failure and expose an optional strongly typed classifier for domain outcomes?
- When child Features compose within one parent Feature, how should spans and metrics avoid duplicated or meaningless measurements?
- What is the minimum first-party logging contract without inventing a second `ILogger`?
- Do we need trace/metrics interfaces in `Feature.Alg` at all, or can most observation stay outside of the contract?
- Which names, schema and event identifiers remain stable enough to expose publicly in a preview?
- Is `VSlices.Work.Observability` a useful distribution boundary or merely organizational ceremony?

## Decision checkpoint

No code instrumentation or new NuGet package is introduced by this document. Proceed with an executable test-first spike on a separate commit before accepting any public API or adding observability-related modeling Laws.
