# VSlices Technical Recovery Objective

## Purpose

VSlices is currently being rebuilt around a semantic model that is more explicit about meaning, authority, work, capabilities, grounding, presentation, and realization.

This stage is not intended to discard the technical capabilities demonstrated by previous VSlices iterations. Its purpose is to recover those capabilities under a model whose semantics can be defended more precisely.

The primary historical benchmark for this recovery is `6.0.0-pre.6.5`, complemented by its real use in systems such as ServiuPro1.

> VSlices `6.0.0-pre.6.5` is a benchmark of demonstrated capability, not a specification of the current design.

The goal is therefore not to restore old abstractions such as `IIntegrator`, `VSlicesRuntime`, or the previous event and background-task taxonomies merely because they existed.

The goal is to understand what real capability each of those constructions enabled, identify the semantics that remain relevant, and reintroduce only the minimal mechanisms required to express those capabilities faithfully in the current model.

---

## Objective

VSlices should progressively recover, and eventually exceed, the demonstrable technical ceiling of its previous iterations without automatically restoring their historical abstractions, taxonomies, or mechanisms.

Each recovered capability should be reinterpreted from the current semantic model.

When relevant, the design should distinguish explicitly between:

- semantic meaning;
- semantic authority;
- executable work;
- required capability;
- grounding;
- presentation or invocation binding;
- host composition;
- concrete realization.

A historical capability is considered recovered when the current system can represent at least its materially relevant behavior and guarantees without introducing unjustified semantic coupling.

If the current design intentionally does not preserve an old behavior or guarantee, that divergence should be explicit and justified.

---

## Historical benchmark

The `6.0.0-pre.6.5` line demonstrated a broad technical surface, including:

- features composed from input, output, executable behavior, interceptors, and integrations;
- multiple integrations or presentations for executable work;
- use-case execution and dispatch;
- event publication;
- event queues;
- interchangeable event publishing mechanisms;
- background event processing;
- retry and dead-letter behavior;
- recurring work based on temporal conditions;
- background execution through `Microsoft.Extensions.Hosting`;
- background execution through Hangfire;
- persistence contracts;
- Entity Framework Core realizations;
- historical entity / aggregate abstractions (retired as current semantic categories);
- effectful execution through LanguageExt;
- runtime-provided capabilities and dependencies.

These capabilities are evidence of problems VSlices has already solved technically.

They do not imply that the old ownership boundaries or names were semantically correct.

Some historical abstractions have now been explicitly retired rather than migrated:

```text
Flow<RT, REQ, RES>
Repository / DatabaseIO
Entity
AggregateRoot
```

Their former responsibilities are either represented by smaller current concepts or remain open as independently named semantic questions. They are not recovery targets.

---

## Main recovery areas

### 1. Executable features and invocation bindings

The previous feature model could associate:

```text
input
-> output
-> behavior
-> interceptors
-> zero or more integrations
```

The current `Feature<F, ALG, REQ, RES> -> Free<ALG, RES>` model now recovers the executable core without a separate runtime/request carrier.

The remaining problem is to rediscover the relation between executable work and the concrete mechanisms that make that work reachable.

Examples include:

- HTTP endpoints;
- command-line invocation;
- scheduled invocation;
- message-driven invocation;
- other future bindings.

The old `IIntegrator` abstraction should not be restored automatically.

Its former uses should instead provide pressure for discovering the minimum semantic mechanism required by each case.

### 2. Events

The previous event subsystem distinguished several real concerns:

```text
event occurrence
!= transport
!= consumer discovery
!= consumer execution
!= retry policy
!= dead-letter handling
```

The current architecture should recover these capabilities without assuming that they belong to one abstraction.

Questions to resolve under real pressure include:

- What does an event semantically represent?
- Who owns the fact that an event occurred?
- Is publication executable work, a capability, or both at different boundaries?
- Which delivery guarantees belong to the requested capability?
- Which guarantees belong only to a particular grounding?
- When are an in-memory queue and a durable external broker equivalent, and when are they not?

Different realizations must not be treated as semantically equivalent when their guarantees materially differ.

### 3. Temporal and recurring work

The old recurring-work implementation combined several concerns:

```text
work
+ temporal condition
+ clock observation
+ waiting
+ scheduling
+ host lifecycle
+ execution
```

The current framework now separates the first two temporal concerns explicitly:

- `Feature` owns the executable WorkFlow;
- `Clock<ALG>` expresses observation of the current semantic `Moment`;
- `Delay<ALG>` expresses waiting for a `Duration<double>` or until a `Moment`;
- `ClockIO` and `DelayIO` are reusable Grounding contracts;
- `SystemTimeIO` realizes both through `TimeProvider`.

The old `HasClock<RT>` / `ClockEnv<RT>` carrier has been removed.

Scheduling, recurring invocation, and host lifecycle remain deliberately unresolved and should be discovered from real scheduling cases instead of introduced speculatively.

A likely pressure point is the distinction between:

```text
what work is executable
```

and

```text
under what temporal condition it should be invoked
```

Hangfire, Quartz, and `Microsoft.Extensions.Hosting` may realize overlapping capabilities while offering materially different guarantees. The model should preserve those differences rather than flatten them behind a falsely equivalent abstraction.

### 4. Effects, capabilities, and runtime

Previous VSlices versions represented executable behavior as:

```text
Eff<VSlicesRuntime, T>
```

A shared runtime exposed capabilities such as dependency resolution and file-system access.

This was technically expressive but allowed work to depend indirectly on broad runtime access.

The current Work model has moved further away from broad runtime access:

```text
Feature<F, ALG, REQ, RES>
    -> Free<ALG, RES>
```

Feature-owned algebras declare only the operations required by the WorkFlow.

For point-oriented external work, the validated vocabulary is:

- `PointReader<ALG, POINT, ID>`;
- `PointWriter<ALG, POINT>`;
- `PointRemover<ALG, POINT, ID>`.

Reusable concrete realization is expressed independently through Grounding contracts such as:

- `PointReaderIO<POINT, ID>`;
- `PointWriterIO<POINT>`;
- `PointRemoverIO<POINT, ID>`.

Entity Framework Core now realizes these capabilities through `EntityFrameworkPointIO` rather than through `DatabaseIO` or a Repository abstraction.

Temporal Work now follows the same direction: Feature-owned algebras express `Clock` and `Delay`, while `ClockIO` and `DelayIO` belong to Grounding rather than an ambient runtime carrier.

The recovery objective is to retain the expressive power of the old effectful runtime while making capability requirements and grounding boundaries smaller, explicit, and independently justified.

### 5. Host and lifecycle

Previous abstractions grouped several concerns under broad ideas such as integrations and background-task listeners.

The current design should rediscover host composition from real pressure.

Relevant responsibilities may include:

- selecting concrete realizations;
- maintaining application lifecycle;
- connecting executable work to invocation bindings;
- installing groundings;
- starting long-running listeners;
- starting schedulers;
- exposing presentations.

Whether these responsibilities belong to one `Host` abstraction or to several mechanisms remains intentionally open.

The old design provides evidence of required capabilities, not the answer.

---

## Success criteria

### Capability conservation

Materially useful behavior demonstrated by previous VSlices iterations can be expressed again.

Recovery is about behavior and guarantees, not API compatibility.

### Semantic improvement

The current representation should make it easier to explain:

- what a construction means;
- who owns its semantics;
- what capability is required;
- what mechanism realizes it;
- what guarantees are preserved.

### No accidental regression

Improved semantics must not silently remove useful properties already demonstrated historically, such as:

- extensibility;
- composability;
- alternative realizations;
- multiple invocation mechanisms;
- effectful composition;
- real infrastructure integration.

### Honest divergence

If an old capability, behavior, or guarantee is intentionally not recovered, the reason should be explicit.

Historical behavior should neither be preserved blindly nor silently lost.

### Pressure before abstraction

Recovery should proceed through real cases:

```text
historical capability
-> first limit in the current model
-> identify who owns that limit
-> discover the minimum mechanism that explains it
-> implement without over-generalizing
-> execute again
-> observe the next limit
```

A historical type name is not sufficient evidence that the same abstraction should exist again.

---

## Evaluation method

For each historical mechanism selected for recovery, record:

```text
historical artifact
-> demonstrated capability
-> relevant guarantees
-> old semantic decomposition
-> accidental coupling or ambiguity
-> current VSlices representation
-> required capabilities
-> grounding / realization
-> preserved guarantees
-> intentionally changed guarantees
-> remaining questions
```

This comparison should be based on executable cases whenever practical.

Implementation is evidence about the design.

---

## Initial sequence

The current preferred recovery sequence is:

```text
1. recurring work
   ->
2. event publication and delivery
   ->
3. multiple invocation bindings for a Feature
   ->
4. lifecycle and Host composition
   ->
5. re-express a complex real Shared.* / ServiuPro1 case
```

This order is provisional.

It should change if implementation pressure reveals a more fundamental missing mechanism.

---

## Non-goals

This effort is not intended to:

- reproduce the `6.0.0-pre.6.5` public API;
- recreate old package boundaries for compatibility;
- restore `IIntegrator`, `VSlicesRuntime`, or other historical abstractions by default;
- declare the current taxonomy final;
- maximize abstraction coverage before real use;
- use semantic vocabulary as a substitute for executable evidence.

---

## Working principle

The desired result is not:

```text
new semantics instead of old capability
```

It is:

```text
current semantic model
+ historically demonstrated technical capability
+ explicitly preserved guarantees
```

The strongest outcome would be to recover a substantial portion of the old technical surface through a smaller set of better-justified semantic mechanisms.

If that happens, VSlices will not merely have rebuilt its previous technical ceiling.

It will have explained it.
