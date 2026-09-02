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

They are not replacements for categories such as unit, integration, adapter, functional, E2E, or system tests. They describe why evidence is being sought rather than where or how the test executes.

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
- distributed behavior;
- environmental disturbance;
- alternative actor paths;
- hidden coupling;
- model incompleteness.

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
| Typical result | Continuity evidence | Counterexample, ambiguity, or stronger confidence |
| Discovery behavior | Protect known knowledge | Explore unknown or insufficiently characterized regions |
| Long-term role | Executable memory | Evolutionary pressure |

A useful deeper interpretation is:

> Regressive testing turns accepted knowledge into executable memory.

> Progressive testing creates deliberate pressure for current knowledge to evolve.

Neither is sufficient alone.

Verification without Challenge can preserve an incomplete model indefinitely.

Challenge without Verification can repeatedly rediscover the same failures without retaining what was learned.

Together they create an accumulating safety mechanism.

## Regressive testing as executable memory

Regression should not be understood only as "a bug must not return".

Any sufficiently important and sufficiently stable piece of accepted knowledge can potentially become executable regression evidence.

### Semantic regression

Preserves the meaning of Domain Types, invariants, equality, normalization, identity, transformations, and laws.

```txt
FolderName still means what our accepted model says it means.
```

### Transition regression

Preserves known Application transitions.

```txt
Given this admissible world and request,
CreateAccount still produces the accepted resulting world and response.
```

### Realization regression

Preserves the fidelity of technical realizations.

```txt
Domain -> SQL -> Domain
```

should continue preserving the relevant semantic value, identity, ordering, optionality, or other represented meaning.

### Compositional regression

Preserves knowledge that exists only when several parts interact.

Individually correct Domain Types, Features, adapters, authorization rules, and views can still compose into an incorrect system.

Functional and E2E verification become especially important when the accepted claim is inherently compositional.

### Scenario regression

Preserves accepted real-world paths.

For example:

```txt
Requester reports problem
    -> Analyst receives it
    -> Specialist acts
    -> Requester observes resolution
```

The claim does not exist inside one Domain Type or Feature. It exists only in the composed behavior of the product.

### Operational regression

Preserves accepted behavior under known operational conditions.

A resilience or failure-mode discovery can become regression knowledge once the expected behavior is understood.

This yields a general principle:

> Accepted knowledge may be promoted into regression evidence at the semantic surface where that knowledge actually exists.

## Progressive testing as adversarial engineering

Progressive testing should not be reduced to "generate more edge cases".

Its deeper role is to act adversarially toward the current model:

```txt
Claim
    -> deliberately search for contradiction
    -> expose the claim to hostile, unusual, or unexplored conditions
    -> collect evidence
```

Ethical hacking and red teaming are important specializations of this structure, oriented toward security claims.

VSlices can generalize the same adversarial stance toward any engineering claim:

- domain validity;
- application transitions;
- infrastructure fidelity;
- authorization;
- workflow consistency;
- concurrency;
- resilience;
- actor journeys;
- architecture;
- design assumptions;
- even VSlices abstractions themselves.

A useful working formulation is:

> Ethical hacking is one specialization of progressive Challenge against security properties.

Progressive testing is broader: it is adversarial engineering against explicit or implied semantic claims.

## Progressive testing can mutate more than input

A Challenge does not need to vary only function arguments.

Depending on the claim, its exploration space may include:

```txt
Input
x Stored State
x Time
x Dependency Behavior
x Network Conditions
x Resource Availability
x Concurrency Schedule
x Actor
x Navigation Path
x Deployment State
```

This makes several existing disciplines natural Challenge strategies.

### Boundary and property exploration

Searches the frontier between valid and invalid spaces and challenges algebraic or semantic properties.

Property-based testing can serve either Verification or Challenge depending on whether an accepted property is being preserved or a larger space is being explored for counterexamples.

### Fuzzing

Searches unusual, malformed, unexpected, or adversarial inputs.

With VSlices, fuzzing can become semantically guided rather than purely byte-oriented by using the represented valid space, boundaries, normalization rules, and technical realizations as search structure.

### Stateful and model-based exploration

Searches sequences of individually meaningful operations for unexpected resulting states.

This is particularly relevant for Features, which naturally represent transitions.

### Temporal and concurrency exploration

Challenges assumptions about ordering, stale state, retries, simultaneous transitions, and time-dependent invariants.

### Representational exploration

Searches for disagreement between two representations of the same semantic concept.

Examples include:

```txt
Domain <-> SQL
Domain <-> JSON
Domain <-> HTTP
Domain <-> UI representation
```

### Environmental and chaos-style exploration

Mutates the environment rather than the primary input.

Examples include dependency unavailability, network interruption, resource exhaustion, stale observations, or partial failures.

Once a discovered failure behavior becomes understood and accepted, it may become operational regression knowledge.

### Adversarial security exploration

Asks explicitly:

> If an authorized tester wanted to violate this security claim without modifying the system, what paths would be attempted?

This includes authorization substitution, stale credentials, alternate entry points, ordering, concurrency, and other security-oriented adversarial strategies.

### Functional and E2E adversarial exploration

Challenges complete business behavior rather than isolated implementation units.

A progressive E2E campaign may vary:

- actor;
- navigation path;
- session changes;
- browser history;
- retries;
- stale tabs;
- valid operation ordering;
- interruption and resumption;
- concurrent actors.

The question is not merely whether the happy path works.

It is:

> What real actor behavior can take the system outside the world our model says should be possible?

This can be understood as adversarial exploration of functional semantics.

### Model and specification exploration

Progressive testing may reveal that the implementation is correct and the claim itself is insufficient.

For example:

```txt
Documentation:
    Only administrators need access to X.

Observed real workflow:
    SupportAgent also needs X to perform an accepted responsibility.
```

The code may perfectly implement the documentation while the documentation fails to represent the world.

This is a first-class finding.

### Framework and process exploration

The same structure can be applied to engineering abstractions and processes.

For example:

```txt
Claim:
    A Feature adequately represents this class of application transition.

Challenge:
    Find a real behavior that cannot be represented without semantic distortion.
```

Or:

```txt
Claim:
    This documentation-to-VSIR path preserves required knowledge.

Challenge:
    Find relevant knowledge systematically lost during the transition.
```

These uses may eventually extend beyond Framework tooling, but the common abstract operation remains Challenge.

## Testing versus observation and feedback

The concept of testing should not expand until every source of evidence becomes "a test".

A useful boundary is intentionality.

### Testing

Deliberate exposure of a claim to selected conditions in order to obtain evidence.

```txt
We intentionally create or select an experiment.
```

### Observation / Feedback

Evidence produced by real use or operation without having been deliberately created as the experiment under consideration.

```txt
The world exposes something to us.
```

Both can return evidence to Understanding:

```txt
Knowledge
    |-- deliberate Testing
    `-- Observation / Feedback
             |
             v
          Evidence
             |
             v
        Interpretation
             |
             v
        Understanding
```

A production incident, support ticket, user observation, or usability finding is therefore not automatically a VSlices test.

It may, however, expose a missing claim and later produce a Verification or Challenge campaign.

## Human-facing claims

Some engineering claims concern human interaction rather than machine state alone.

For example:

```txt
A representative user can understand and complete this workflow.
```

No number of deterministic browser executions proves that claim by itself.

Evidence may require controlled observation with representative users, accessibility evaluation, or other human-facing methods.

These methods should not all be relabeled as software testing, but their evidence can participate in the same knowledge loop when they examine claims represented or implied by the system.

## Claim geometry determines evidence geometry

One of the strongest emerging principles is:

> The geometry of the claim determines the geometry of the evidence.

A local claim can have a local faithful surface.

A compositional or world-facing claim may only exist at a broad surface.

Examples:

```txt
EmailAddress normalization
    -> Domain construction surface

EmailAddress persistence fidelity
    -> Domain + real adapter + real database

CreateAccount transition
    -> Application + Domain + required real Infrastructure

Requester can resolve a support problem through Ticket Support
    -> functional/E2E product composition
```

A broader surface should therefore be preferred when the claim is inherently compositional or world-facing.

The objective is not to minimize topology, and it is not to maximize topology mechanically.

The objective is:

> Use the evidence surface that most faithfully represents the semantic relation being tested.

Or, more compactly:

> Maximize claim-relative semantic fidelity.

## Functional and E2E evidence

Functional and E2E testing have high importance in this model because they can expose contradictions that local evidence cannot.

Local evidence primarily asks:

> Did we materialize this represented claim correctly?

Functional and E2E evidence can additionally ask:

> Does the composed system produced from our representation behave like the world we intended to model?

This distinction matters especially in AI-assisted development.

A wrong specification can produce coherent Domain code, Features, adapters, views, and local tests. A sufficiently faithful whole-path test can expose that the composed product does not support the real phenomenon the documentation claimed to represent.

Therefore E2E should not be treated only as expensive smoke coverage at the top of a pyramid.

When the claim is an actor journey or complete business outcome, E2E or another full functional surface is the natural evidence surface.

## Regressive and progressive testing are orthogonal to technique

Techniques do not inherently belong to only one direction.

```txt
                         Verification     Challenge
Domain laws                   X              X
Property testing              X              X
Database testing              X              X
Functional testing            X              X
E2E                           X              X
Security testing              X              X
Resilience testing            X              X
Architecture testing          X              X
```

For example:

```txt
Property testing used to preserve an accepted law
    -> regressive

Property testing used to search a wider generated space
    -> progressive

Known E2E business journey replayed continuously
    -> regressive

E2E explorer varying actor paths and ordering
    -> progressive
```

This distinction should remain stable even if individual tools or methodologies change.

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

Challenge itself may also remain valuable after one discovery. VSlices may eventually preserve recurring Challenge campaigns that continue exploring new regions while accepted discoveries are separately promoted into deterministic regression evidence.

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

The concrete testing projection may later choose xUnit, property-based testing, fuzzing, mutation testing, integration testing, functional testing, E2E, browser automation, chaos-style fault injection, or another mechanism.

The IR should preserve the reason for the test rather than the accidental structure of the test framework.

## Test generation as an iterative process

A test-oriented IR should not be assumed to represent one permanent concrete test.

The current direction is closer to:

```txt
Production VSIR
    +
Testing Intent
    |
    v
Candidate generation / campaign
    |
    v
Candidate tests or scenarios
    |
    v
Execution
    |
    v
Evidence
    |
    v
Human review
    |-- reject -> discard
    |-- accept -> promote to suite
    `-- semantic discovery -> revise knowledge first
```

Generated candidates do not become authoritative merely because they compile or pass.

The permanent suite is curated output from the process.

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

The test representation should reference the authoritative semantic source rather than duplicate its invariant definitions.

This distinction should help avoid reproducing test source code or domain knowledge in another syntax.

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
- challenge inputs;
- functional scenarios;
- E2E paths.

The scarce resource becomes semantic correctness:

```txt
Understand
    -> specify clearly
    -> propagate
    -> expose claims to reality
    -> obtain evidence
    -> interpret divergence
```

This permits more derived and broader evidence than a fully manual development process could economically maintain.

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

Functional, E2E, adversarial, and other broad-surface challenges become especially important when the purpose is not merely to verify code against the IR, but to test whether the IR itself captures the phenomenon represented by documentation and design.

The system should automate propagation without treating generated consistency as proof of truth.

## Productive and exploratory generation have different goals

AI or other generation used for productive projection should converge toward stable, reproducible materialization of represented knowledge.

Exploratory generation should instead seek semantic diversity.

```txt
Production generation
    -> reduce solution space
    -> converge

Challenge generation
    -> expand useful search space
    -> diverge
```

The goal is not arbitrary non-determinism.

It is useful semantic novelty under explicit constraints.

Candidate discovery may be non-deterministic, but retained evidence and promoted regression tests should be reproducible.

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
- hosted service;
- functional product;
- browser/E2E;
- distributed system;
- architecture.

Therefore both directions can exist at multiple levels.

```txt
                    Evidence boundary
              Domain  Slice  Adapter  Product  System
Verification    x      x       x        x       x
Challenge       x      x       x        x       x
```

The evidence surface should not be selected by a rule such as "always choose the smallest possible test" or "always choose the largest realistic system".

It should be selected according to the claim under examination.

> Use the evidence surface that most faithfully represents the semantic relation being tested.

Additional components improve evidence only when they participate materially in that relation.

## Aspire and whole-system evidence

Aspire can be useful as one outer evidence boundary for distributed composition.

It should not become the default mechanism for every test merely because the application is hosted through Aspire.

Its strongest role is to answer questions that require the real distributed composition, such as whether:

- resources can start together;
- declared dependencies are wired correctly;
- real infrastructure is reachable;
- service composition behaves correctly;
- system-level contracts survive actual execution boundaries;
- a represented full-system scenario can actually occur across independent deployments.

Current Domain Type, invariant, and Feature VSIR normally need smaller claim-faithful surfaces such as direct Domain execution or a hosted service with real Dockerized dependencies.

Future scenario, journey, or distributed-flow representations may naturally require Aspire or another whole-system surface.

## Candidate testing techniques

Several existing techniques can serve the model without defining it.

### Property-based testing

Especially useful for invariants, algebraic properties, transformations, Domain Types, Entities, Aggregate Roots, generated boundaries, and transition exploration.

It can serve both directions:

- regressively, when testing an accepted property;
- progressively, when searching broad generated spaces for counterexamples.

### Contract testing

Useful for making capability and service boundaries executable and retaining continuity between declared contracts and implementations.

### Architecture testing

Useful for turning structural continuity rules into executable evidence.

Architecture can also be challenged progressively by looking for indirect semantic coupling that simple dependency rules do not expose.

### Mutation testing

Useful as a challenge against the quality of the existing verification system.

It can ask whether meaningful implementation or semantic changes would actually be detected by the current evidence.

Mutation score should be treated as diagnostic evidence rather than as a number to maximize.

### Fuzzing and search-based generation

Useful for exploring unusual inputs, technical representations, and large spaces where deterministic enumeration is impractical.

Within VSlices, generators should preferentially use semantic structure rather than produce arbitrary noise when the IR provides meaningful partitions and boundaries.

### Model-based and stateful testing

Useful when the target is a transition system, especially Features and sequences of Features.

These techniques can explore paths, state histories, and combinations that are difficult to enumerate manually.

### Real adapter testing

When an adapter's semantics depend on real infrastructure behavior, evidence should come from that real behavior where practical.

A fake application port can serve as a model or isolated semantic aid, but it does not prove SQL Server, serialization, messaging, or another adapter behaves equivalently.

### Component, functional, and browser testing

Component-level tools remain useful for claims local to a UI component.

Full hosted or browser execution is appropriate when navigation, DOM, JavaScript, authorization, sessions, actor behavior, persistence, or complete product composition is part of the claim.

### Adversarial and security testing

Useful when deliberately attempting to violate security, authorization, integrity, availability, or trust claims.

Security is a specialized but important progressive Challenge domain.

### Chaos and fault-injection testing

Useful when the claim depends on behavior under environmental disturbance or dependency failure.

Once accepted failure behavior is learned, parts of those experiments can become regression evidence.

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

Progressive generation may deliberately construct malformed technical worlds, but those worlds should be identified as technical challenges rather than silently treated as valid Domain values.

## Core principles

The current approach can be summarized by the following principles.

### Preserve known correctness

Accepted knowledge should be convertible into regression evidence.

### Challenge assumed correctness

The current model should never be treated as automatically exhaustive.

### Treat regression as memory

Regression preserves more than historical bugs. It preserves accepted semantic, transitional, compositional, technical, and operational knowledge.

### Treat progression as evolutionary pressure

Progressive testing should deliberately search for evidence that current knowledge is incomplete, contradictory, or incorrectly realized.

### Promote discoveries deliberately

Challenges produce observations. Observations become domain or engineering knowledge only after interpretation and an explicit semantic decision.

### Make learned safety cumulative

Meaningful discoveries should be incorporated into the knowledge model and become permanent verification where appropriate.

### Let claim geometry determine evidence geometry

Local claims may have local faithful surfaces. Compositional and world-facing claims may require functional, E2E, or distributed surfaces.

### Maximize claim-relative semantic fidelity

Do not mechanically minimize or maximize the test environment. Reproduce the semantic relation under examination as faithfully as practical.

### Keep testing intentional

Testing deliberately exposes claims to selected conditions. Feedback and observation also produce evidence, but should not be renamed as testing when no deliberate experiment occurred.

### Generate representations; maintain meaning

Automation and AI may propagate knowledge into many artifacts, but semantic authority remains with the underlying engineering understanding.

## Compact formulation

A compact description of the model is:

> VSlices develops safely by preserving what is known to be correct, actively challenging what is assumed to be correct, and incorporating validated discoveries back into its semantic source.

A compact formulation of the two testing directions is:

> Regressive testing asks whether reality still agrees with what we know.

> Progressive testing asks what reality can teach us that we do not know yet.

An even shorter formulation remains:

> Preserve. Challenge. Learn. Preserve again.

## Relationship to the VSlices cycle

This approach should remain recognizable as part of the original VSlices cycle rather than becoming a separate methodology.

The important extension is the mechanism between `Building` and the next `Understanding`.

```txt
Understanding
    -> Contextualizing
    -> Planning
    -> Building
    -> deliberate testing + observation / feedback
    -> evidence
    -> interpretation
    -> Understanding
```

Testing is therefore not merely a QA stage after implementation.

It is one of the mechanisms by which the materialized understanding is exposed to conditions capable of producing evidence about both the implementation and the knowledge from which that implementation was derived.

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

- how Verification and Challenge apply concretely to each current VSIR artifact kind;
- the exact authored shape and naming of test-oriented IR;
- which evidence obligations can be derived automatically from existing Domain Type, invariant, and Feature IR;
- how test IR references one invariant, condition, intrinsic, law, or derived claim without duplicating it;
- whether Infrastructure/adapter semantics eventually require their own representation;
- how candidate generation, execution, evidence reduction, and human promotion are implemented;
- how provenance between knowledge, generated candidates, implementation, environment, and evidence should be tracked;
- how semantic diversity and redundancy of Challenge candidates should be measured;
- how challenge generation can use different visibility policies from production generation;
- which Challenge campaigns should recur instead of being reduced to fixed regression cases;
- how functional/E2E Scenario or Journey knowledge should eventually be represented;
- how human-facing evidence such as usability or accessibility should connect to the same knowledge loop without collapsing all research into "testing";
- whether the abstract preserve/challenge/evidence model eventually becomes transversal across Method, Design, Docs Standard, and Framework while executable tooling remains primarily a Framework concern.

These questions are part of the future development of the approach, not omissions to silently fill with conventional testing practice.
