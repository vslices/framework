# Testing Approach

This document captures the current testing direction for VSlices.

It is intentionally conceptual.

The goal is not to define a testing framework, a required toolset, or a fixed test taxonomy. The goal is to preserve enough of the reasoning behind the approach so it can be reconstructed and evolved later.

## Starting point

VSlices already has a continuity cycle:

```txt
Understanding
    -> Contextualizing
    -> Planning
    -> Building
    -> Understanding
```

The testing approach is not a separate cycle attached to this one.

It is a natural extension of the transition from `Building` back to `Understanding`.

After something is built from what we currently believe to be true, VSlices needs ways to:

- deliberately challenge those beliefs;
- observe what the resulting software actually does;
- collect evidence;
- interpret that evidence;
- decide whether the current understanding is still sufficient.

Conceptually:

```txt
Understanding
    -> Contextualizing
    -> Planning
    -> Building
    -> Challenging / Observing
    -> Interpreting Evidence
    -> Understanding
```

The original cycle remains valid. The additional steps explain how the final return to `Understanding` can happen in a deliberate and reconstructible way.

## Knowledge flow across VSlices

The current direction distributes responsibility roughly as follows.

### VSlices Method, VSlices Design, and VSlices Docs Standard

These are primarily concerned with obtaining knowledge from the real world, shaping it, concentrating it, and preserving enough continuity for that knowledge to remain usable through engineering work.

They help establish things such as:

- domain vocabulary;
- context;
- meaning;
- constraints;
- expected behavior;
- decisions;
- invariants;
- known boundaries;
- unresolved uncertainty.

They do not merely produce documentation. They participate in the formation and preservation of the current engineering understanding.

### Intermediate Representation

A structured intermediate representation exists between concentrated knowledge and its materializations.

Its purpose is broader than code generation.

It exists so that sufficiently explicit knowledge can be propagated into multiple engineering representations without requiring each one to be recreated independently.

Conceptually:

```txt
Real world
    -> Method / Design / Docs Standard
    -> concentrated semantic knowledge
    -> intermediate representation
```

The intermediate representation has at least two important directions.

```txt
Intermediate Representation
    |-- software creation
    |     -> executable software
    |
    `-- knowledge retention / extension
          -> verification
          -> challenge
          -> evidence
```

This means the intermediate representation is not only a way to describe what software should be generated.

It is also a way to make current knowledge operationally examinable.

### VSlices Framework

VSlices Framework is the materialization of the acquired knowledge into executable patterns and structures.

The Framework does not define the real world directly.

It materializes the current understanding represented through the engineering model.

This distinction matters because the model is always revisable.

## Knowledge, representation, and evidence

Three concepts should remain distinct.

### Knowledge

What VSlices currently considers to be an adequate understanding of some part of the problem.

This may still be incomplete or wrong.

### Representation

A structured expression of that knowledge that allows it to be propagated into code, documentation, tests, contracts, or other artifacts.

A representation is not automatically true merely because it is structured or executable.

### Evidence

What is observed when a representation or one of its materializations is exercised under concrete conditions.

Evidence can support the current model, contradict it, or reveal ambiguity.

Evidence does not directly redefine the domain.

Instead:

```txt
Observation
    -> Interpretation
    -> Semantic decision
    -> Knowledge change, if warranted
```

Only after interpretation should the source knowledge be changed and propagated again.

## Invariants as a central propagation point

Invariants are especially important to this approach.

An invariant states something that must remain true for a concept or system to continue satisfying its intended meaning.

Because of that, an invariant can support several independent projections.

```txt
Invariant
    |-- implementation
    |-- documentation
    |-- verification
    |-- challenge target
    `-- contract or boundary constraints
```

For example, if a domain concept declares that a value cannot be semantically blank, the same invariant may participate in:

- constructing the domain implementation;
- documenting valid and invalid representations;
- generating regression evidence;
- generating boundary and property-based challenges;
- validating persistence or transport representations when relevant.

The invariant should be expressed as few times as practical. Its consequences can be propagated into multiple artifacts.

## Two complementary testing functions

The current model distinguishes two major testing intentions.

They are not replacements for categories such as unit, integration, adapter, or system tests. They describe why evidence is being sought rather than where the test executes.

## Verification Intent

A Verification Intent asks:

> Does this still do what we already believe it should do?

Verification is primarily regressive.

Its role is to preserve acquired knowledge and detect divergence from known claims.

A Verification Intent may originate from:

- an invariant;
- an expected behavior;
- a contract;
- an architectural rule;
- a previously understood bug;
- an incident;
- a domain decision;
- an accepted discovery from earlier exploration.

Conceptually:

```txt
Known semantic claim
    -> Verification Intent
    -> regression evidence
    -> "is this still true?"
```

Verification does not need to rediscover the meaning of the domain every time it runs.

Its purpose is to prevent already-acquired knowledge from being lost during change.

## Challenge Specification

A Challenge Specification asks:

> Where does what we currently believe stop being true?

Challenge is primarily progressive.

Its role is to search for limits, counterexamples, missing assumptions, unexpected compositions, or behavior the current model did not anticipate.

A challenge may explore areas such as:

- boundary values;
- unusual but type-valid inputs;
- generated values;
- normalization;
- Unicode and encoding behavior;
- persistence round trips;
- serialization;
- ordering;
- mutation;
- concurrency;
- state-transition combinations;
- resource limits;
- authorization combinations;
- distributed behavior.

Conceptually:

```txt
Current semantic model
    -> Challenge Specification
    -> exploration / falsification attempt
    -> "what is this?"
    -> "what is this not?"
    -> "where are its real limits?"
```

A challenge is allowed to ask questions the current source of truth cannot yet answer.

That property is important. If every challenge were only a mechanical repetition of the same rules used to generate the implementation, the system could become consistently wrong without discovering it.

## Regressive and progressive testing

Verification and Challenge form two complementary directions.

| Function | Verification | Challenge |
| --- | --- | --- |
| Main role | Preserve knowledge | Seek new knowledge |
| Direction | Regressive | Progressive |
| Starts from | Accepted claims | Current model and assumptions |
| Main question | Is this still true? | Where does this stop being true? |
| Typical result | Continuity evidence | Counterexample or stronger confidence |
| Discovery behavior | Protect known cases | Explore unknown cases |

Neither is sufficient alone.

Verification without Challenge can preserve an incomplete model indefinitely.

Challenge without Verification can repeatedly rediscover the same failures without retaining what was learned.

Together they create an accumulating safety mechanism.

## Preventive and reactive development safety

The same distinction can be interpreted as a model for safer development.

### Preventive

Challenge attempts to discover failures before they become known incidents.

It deliberately stresses assumptions and invariants.

```txt
Assumption
    -> Challenge
    -> possible weakness
```

### Reactive

Verification preserves knowledge once something is understood.

The knowledge may come from design work, a discovered defect, production feedback, an incident, or a previous challenge.

```txt
Known requirement or learned defect
    -> Verification
    -> regression protection
```

Reactive does not mean waiting for production to fail.

It means reacting to knowledge once that knowledge has been acquired.

## Progressive knowledge can become regressive protection

One of the strongest properties of the model is that successful exploration can become permanent regression protection.

```txt
Challenge
    -> Discovery
    -> Interpretation
    -> Semantic decision
    -> Refined invariant / behavior / boundary
    -> Verification
```

In other words:

> What is progressive today can become regressive tomorrow.

This makes safety cumulative.

A discovered edge case should not remain merely an interesting exploratory test forever if it reveals a meaningful semantic rule.

Once the rule is understood and accepted, it should be incorporated into the knowledge model and propagated into ordinary verification.

## Challenges do not define the domain automatically

A challenge can produce an observation, but an observation is not itself a domain decision.

For example:

```txt
Invariant:
    FolderName must reject semantically blank values

Challenge:
    unusual Unicode whitespace

Observation:
    some input bypasses the current implementation
```

The observation alone does not tell us whether:

- the implementation is wrong;
- the invariant needs refinement;
- the input is actually valid;
- the documentation is ambiguous;
- a technical boundary behaves differently from the domain boundary.

The evidence must return to the understanding process.

```txt
Evidence
    -> Method / Design / Docs Standard
    -> interpretation
    -> expected or unexpected?
    -> semantic decision
```

If the understanding changes, the new knowledge is represented again and propagated through the system.

## Testing IR should describe semantic intent, not test framework mechanics

If VSlices introduces intermediate representations for testing, they should not primarily model xUnit classes, attributes, assertions, or other test-runner mechanics.

The useful abstraction is semantic intent.

A Verification Intent could express something conceptually similar to:

```txt
Given:
    the declared invariants of FolderName

Verify:
    known invalid constructions remain invalid
    known valid constructions remain valid
    normalization behavior remains preserved
```

A Challenge Specification could express something conceptually similar to:

```txt
Given:
    the current invariants of FolderName

Challenge:
    boundary conditions
    unusual representations
    generated values
    normalization interactions
```

The concrete testing projection may later choose xUnit, property-based testing, fuzzing, mutation testing, integration tests, or another mechanism.

The IR should preserve the reason for the test rather than the accidental structure of the test framework.

## Default derived verification and explicit challenge intent

Not every test intention necessarily needs to be authored independently.

If a domain IR already contains enough information, some verification can be derived automatically.

For example:

```txt
ValueObject / Entity / AggregateRoot IR
    -> declared invariants
    -> default verification profile
    -> generated regression tests
```

Explicit test-oriented IR becomes more valuable when it adds intent that is not already a direct consequence of the domain representation.

```txt
Domain IR
    -> default verification

Additional Verification Intent
    -> preserve a known scenario not fully encoded elsewhere

Challenge Specification
    -> explore assumptions or boundaries beyond known examples
```

This distinction should help avoid reproducing test source code in another syntax.

## AI-assisted engineering changes the economics

This approach assumes that VSlices engineering will make substantial use of AI-supported generation.

That changes the cost model.

The expensive activity is no longer necessarily writing every individual artifact by hand.

AI can reduce the cost of producing and maintaining:

- implementation code;
- tests;
- property cases;
- fixtures;
- contracts;
- documentation projections;
- repetitive architectural checks;
- challenge inputs.

The scarce resource becomes semantic correctness:

```txt
Understand
    -> specify clearly
    -> propagate
    -> obtain evidence
    -> interpret divergence
```

This permits more derived evidence than a fully manual development process could economically maintain.

However, AI also introduces a particular risk:

```txt
wrong specification
    -> coherent generated implementation
    -> coherent generated tests
    -> all tests pass
```

Everything can be internally consistent and still be wrong.

For that reason, VSlices should distinguish between propagation and challenge.

Generated representations preserve meaning.

Challenges should actively search for places where that meaning or its implementation may be incomplete.

The system should automate propagation without treating generated consistency as proof of truth.

## Evidence boundary is a separate dimension

Verification and Challenge describe purpose.

The boundary at which evidence is collected describes execution scope.

Possible evidence boundaries include:

- domain;
- application slice;
- contract;
- adapter;
- persistence;
- view/component;
- distributed system;
- architecture.

Therefore both directions can exist at multiple levels.

```txt
                    Evidence boundary
              Domain  Slice  Adapter  System
Verification    x      x       x       x
Challenge       x      x       x       x
```

A regression test does not automatically need a full distributed system.

A challenge does not automatically need to be a unit test.

The intended claim should be exercised at the smallest boundary that provides sufficient evidence for that claim.

## Aspire and whole-system evidence

Aspire can be useful as one outer evidence boundary for distributed composition.

It should not become the default mechanism for every test merely because the application is hosted through Aspire.

Its strongest role is to answer questions that require the real distributed composition, such as whether:

- resources can start together;
- declared dependencies are wired correctly;
- real infrastructure is reachable;
- service composition behaves correctly;
- system-level contracts survive actual execution boundaries.

Domain invariants or application behavior should normally be exercised at cheaper boundaries when those boundaries already provide sufficient evidence.

## Candidate testing techniques

Several existing techniques can serve the model without defining it.

### Property-based testing

Especially useful for invariants, algebraic properties, transformations, Value Objects, Entities, Aggregate Roots, and generated boundary exploration.

It can serve both directions:

- regressively, when testing an accepted property;
- progressively, when searching broad generated spaces for counterexamples.

### Contract testing

Useful for making capability and service boundaries executable and retaining continuity between declared contracts and implementations.

### Architecture testing

Useful for turning structural continuity rules into executable evidence.

Examples include dependency constraints between Domain, Application, Infrastructure, shared dependencies, products, and services.

### Mutation testing

Useful as a challenge against the quality of the existing verification system.

It can ask whether meaningful implementation changes would actually be detected by the current evidence.

Mutation score should be treated as diagnostic evidence rather than as a number to maximize.

### Real adapter testing

When an adapter's semantics depend on real infrastructure behavior, evidence should come from that real behavior where practical.

A fake application port can test application semantics, but it does not prove SQL Server, serialization, messaging, or another adapter behaves the same way.

### Component and browser testing

Component-level tools should be preferred when they provide enough evidence for UI behavior.

Full browser execution is appropriate when browser, DOM, JavaScript, rendering, navigation, or other real client behavior is itself part of the claim.

## Avoid interaction-heavy testing by default

VSlices should prefer evidence about meaningful state, output, behavior, and observable effects over tests tightly coupled to internal orchestration.

For example, prefer a claim such as:

```txt
Given a valid Folder request
When the CreateFolder capability executes
Then the Folder exists
And the expected domain effect is observable
```

over a test whose primary meaning is:

```txt
Repository.Add was called exactly once
```

Interaction counts are still valid when the interaction itself is meaningful behavior, such as idempotency, audit emission, event publication, or a required external call guarantee.

## Semantic test data

Generated test data and builders should use the public semantics of the domain rather than bypassing invariants merely to make setup easy.

The objective is to produce meaningful scenarios, not large object graphs detached from domain language.

## Core principles

The current approach can be summarized by the following principles.

### Preserve known correctness

Accepted knowledge should be convertible into regression evidence.

### Challenge assumed correctness

The current model should never be treated as automatically exhaustive.

### Promote discoveries deliberately

Challenges produce observations. Observations become domain knowledge only after interpretation and an explicit semantic decision.

### Make learned safety cumulative

Meaningful discoveries should be incorporated into the knowledge model and become permanent verification where appropriate.

### Use the cheapest sufficient evidence boundary

Run a claim at the smallest boundary that provides adequate evidence for what is being asserted.

### Generate representations; maintain meaning

Automation and AI may propagate knowledge into many artifacts, but semantic authority remains with the underlying engineering understanding.

## Compact formulation

A compact description of the model is:

> VSlices develops safely by preserving what is known to be correct, actively challenging what is assumed to be correct, and incorporating validated discoveries back into its semantic source.

An even shorter formulation is:

> Preserve. Challenge. Learn. Preserve again.

## Relationship to the VSlices cycle

This approach should remain recognizable as part of the original VSlices cycle rather than becoming a separate methodology.

The important extension is the mechanism between `Building` and the next `Understanding`.

```txt
Understanding
    -> Contextualizing
    -> Planning
    -> Building
    -> evidence from challenges and feedback
    -> interpretation
    -> Understanding
```

Testing is therefore not merely a QA stage after implementation.

It is one of the mechanisms by which the materialized understanding produces evidence that can return to the knowledge-forming parts of VSlices.

The resulting continuity is broader than knowledge-to-software continuity:

```txt
real world
    -> understanding
    -> representation
    -> software
    -> evidence
    -> improved understanding
```

That feedback path is the central idea this document intends to preserve.

## Open questions

The following areas remain intentionally unresolved and should be explored before turning this approach into rigid conventions or tooling:

- the exact shape and naming of Verification Intent IR;
- the exact shape and naming of Challenge Specification IR;
- which verification can be derived automatically from existing domain IR;
- how explicit invariants should be represented for the best propagation value;
- how discoveries should be recorded before becoming accepted semantic changes;
- how feedback from production and users participates in the same evidence model;
- which generated artifacts are authoritative, derived, or disposable;
- how provenance between knowledge, generated tests, implementation, and evidence should be tracked;
- how AI-generated challenges can remain sufficiently independent from AI-generated implementations;
- how test execution boundaries should be selected automatically or declaratively;
- how Aspire-based whole-system evidence should be isolated from cheaper test boundaries;
- whether this model eventually belongs only to Framework tooling or becomes a broader VSlices concept shared with Method, Design, and Docs Standard.

These questions are part of the future development of the approach, not omissions to silently fill with conventional testing practice.
