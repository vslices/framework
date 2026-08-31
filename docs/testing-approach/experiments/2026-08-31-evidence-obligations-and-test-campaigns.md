# Experiment: Evidence Obligations and Test Campaigns

Status: exploratory, non-canonical

Date: 2026-08-31

This note records an experiment performed against the current VSlices Framework semantics and the current VSIR corpus in `access-management-product`.

It is not a final test IR specification. Its purpose is to preserve findings that may guide a later specification.

## Research question

The experiment examined whether the current distinction between `Verification` and `Challenge` can be made operational without prematurely defining the concrete syntax of a test VSIR.

The working abstract meanings are:

```text
Verification
    intent to obtain evidence that an asserted semantic claim holds

Challenge
    intent to search for boundaries, counterexamples, missing assumptions,
    or technical realizations that invalidate or complicate that claim
```

These meanings should remain abstract first.

The next design problem is not to redefine them for every artifact kind. It is to determine how the same abstract intentions apply to Domain Types, reusable invariants, Features, infrastructure mappings, and future system-flow representations.

## Corpus inspected

The experiment considered:

- current Framework traits for `DomainType`, `Identifier`, `Entity`, `AggregateRoot`, and `Feature`;
- existing executable `Category` and `Arrow` laws;
- Domain Type, invariant, Feature, condition, representation, and lowering documentation;
- current Domain Type artifacts such as `StreetExtension`, `EmailAddress`, `FolderPath`, `Folder`, and `AttachedFile`;
- current invariant artifacts such as `AccountCanBeActivated` and `ExistingAccount`;
- current Feature artifacts such as `AddFile` and `UpdateAccount`;
- current persistence mappings for Accounts and Folders;
- current editable-projection and projection-drift rules.

The current Framework repository does not itself contain the production VSIR corpus. The concrete artifacts currently live in the product repository and are interpreted through Framework semantics and lowering rules.

## Main result

A test VSIR should probably not represent one concrete test case.

It is better understood as an input to a test-generation and evidence-collection process.

```text
Production VSIR
    +
Testing intent
    |
    v
Test campaign
    |
    v
Candidate tests
    |
    v
Execution and evidence
    |
    v
Human interpretation
    |-- reject -> discard
    |-- accept as regression -> promote
    |-- discover semantic gap -> revise knowledge first
    `-- unresolved -> retain as investigation evidence
```

The permanent suite is therefore curated output, not the direct output of a generative model.

## Evidence obligations

The strongest new concept produced by the experiment is an intermediate layer provisionally called **Evidence Obligations**.

A Production VSIR already contains semantic claims, but a testing intent should not need to restate those claims. A deterministic interpreter can derive obligations from the artifact, its referenced artifacts, its traits, and the catalogs on which its nodes depend.

```text
Production VSIR
    |
    v
Semantic dependency graph
    |
    v
Evidence obligations
    |-- selected for Verification
    `-- explored through Challenge
```

Evidence obligations are not necessarily author-written files. They may be an internal representation produced by tooling.

This layer separates three concerns:

```text
Production VSIR
    owns semantic claims

Evidence obligations
    expose what would count as relevant evidence for those claims

Testing intent
    selects how those obligations will be corroborated or challenged
```

This avoids copying invariants into test VSIR and avoids encoding xUnit, assertions, Docker, or other lowering mechanics in the semantic source.

## Obligations derivable from current Domain Type VSIR

A Domain Type defines a valid state space and the process by which candidate input may enter that space.

Current VSIR nodes imply several candidate obligations.

### Ordered construction

Construction steps are ordered semantic operations.

```text
normalize
    -> ensure
    -> apply
    -> refine
    -> establish state
```

Testing must preserve that order. Testing each predicate independently is insufficient when one input violates several conditions because the first owned failure may be part of observable semantics.

Candidate obligations include:

- every accepted candidate produces an instance inside the declared state space;
- every rejected candidate fails through an applicable declared failure;
- the order of overlapping failures matches construction order;
- normalization occurs before the conditions that follow it;
- refinement produces the declared bindings and final state;
- nested `apply` and `apply-seq` operations do not bypass the target Domain Type construction surface.

### Normalization and canonicalization

A normalization node often induces metamorphic relations rather than a finite list of examples.

For example, when input is trimmed before validation, useful obligations may include:

```text
construct(x) and construct(trim(x))
    produce semantically equivalent results when both are accepted
```

A representation mapping can induce canonicalization obligations such as:

```text
represent(construct(represent(validValue)))
    remains stable
```

The exact relation must be derived only when the construction and representation contracts make it valid. Round-trip must not be assumed merely because a `Repr` exists.

### Equality and trait laws

Traits and equality declarations contribute obligations that are not written as construction steps.

An `identifier` implies equality behavior through the Framework's discrete-space contract. An explicit equality intrinsic adds the selected equivalence relation.

Candidate obligations include:

- reflexivity;
- symmetry;
- transitivity;
- consistency between `Equals`, operators, and hashing;
- consistency between the declared equality source and the implementation;
- compatibility between semantic identity and technical uniqueness where an adapter uses the value as a key.

The current Framework already treats algebraic laws as executable semantic claims through `CategoryLaws` and `ArrowLaws`. Test tooling can generalize that pattern: traits may contribute reusable law catalogs to the evidence-obligation graph.

### Classification obligations

Classification may contribute additional obligations, but current executable contracts are not yet equally expressive for every classification.

- A Value Object is value-identified.
- An Entity has identity continuity.
- An Aggregate Root is an identity-bearing consistency boundary.

The semantic documentation is richer than some current trait surfaces. This difference should be treated as a design signal rather than silently filled by test generation.

## Obligations derivable from reusable invariant VSIR

A reusable invariant represents admissibility or refinement knowledge.

Its principal shapes are:

```text
A -> A
A -> B
(A, B, ...) -> C
```

Candidate obligations include:

- successful `A -> A` evaluation preserves the semantic input;
- successful `A -> B` evaluation produces a valid `B`;
- tuple fields are interpreted by semantic name and type;
- declared expected errors are produced for known failing conditions;
- repository observations are interpreted by the invariant rather than being mistaken for the invariant itself;
- an invariant requiring external capabilities can be exercised across relevant environment states;
- an invariant does not persist use-case state changes, because persistence belongs to Feature Flow.

The last point implies a useful infrastructure-backed verification: an invariant that reads a database can be executed against a real database while asserting that the observable database state remains unchanged.

## Obligations derivable from Feature VSIR

A Feature represents an application transition.

A useful abstract model is:

```text
Feature:
    Request + initial observable world
        -> Response + resulting observable world
```

The current VSIR provides validation, runtime requirements, domain establishment, repository operations, and return construction. Candidate obligations include:

- validation success enables Flow;
- validation failure prevents Flow effects;
- conditional invariant application performs no observation or transformation when its guard is false;
- candidate states selected by Application are established through Domain construction rather than bypassing it;
- resulting domain values remain valid;
- declared repository operations produce the expected resulting state;
- returned Response corresponds to the declared result binding;
- state not selected for change remains semantically preserved;
- persisted state can be reconstructed into the same semantic result;
- expected failures do not leave partial state changes unless such behavior is explicitly represented.

For Features, model-based and stateful property testing are natural techniques. A Feature is already a transition model. A catalog of Features may later support generated command sequences even before a dedicated journey or scenario VSIR exists.

## Infrastructure as translation between spaces

The current Production VSIR corpus represents Domain Types, invariants, and Features, but it does not yet structurally represent all infrastructure mappings and schemas.

The implementation reveals an important semantic model for infrastructure:

```text
Domain valid space V
    <->
Technical representable space T
```

An adapter typically contains at least:

```text
encode : V -> T

decode : T -> Result<V, Error>
```

This yields strong evidence obligations:

1. **Valid round-trip**

   ```text
   decode(encode(v)) ~= v
   ```

2. **Safe rejection**

   Technical states outside the valid domain space must not silently become invalid domain objects.

3. **Representability**

   Every valid domain value that the adapter promises to persist must be representable by the technical system.

4. **No unintended semantic loss**

   Distinct semantic values must not collapse through encoding unless the technical and domain contracts intentionally share that equivalence.

5. **Equality and uniqueness alignment**

   Database uniqueness, collation, key comparison, and domain equality must not disagree in ways that make a valid semantic transition fail or an invalid duplicate succeed.

6. **Container and ordering preservation**

   Sequence order, optionality, sum variants, and other structural semantics must survive translation.

7. **Corrupt-state behavior**

   Schema-valid but domain-invalid data should produce an explicit reconstruction failure.

This directly supports the proposed challenge technique of mutating database values and asking which technical states are convertible to the domain and which are rejected.

However, a Production Domain Type VSIR alone cannot enumerate every technical realization. Tooling will eventually need one or more of:

- an Infrastructure/adapter representation;
- a registered adapter catalog linking semantic types to technical mappings;
- a technical Challenge mode that deliberately inspects implementation, schema, and configuration.

This is a real gap exposed by the experiment.

## Intrinsics are semantic dependencies

Nodes such as:

```text
valid-email
split-first-rest
length-between
ordinal-ignore-case-equals
concat-space
```

are not mere lowering conveniences.

They delegate part of the semantic meaning to an intrinsic catalog and its implementation.

Therefore the semantic dependency graph is not only:

```text
Feature -> Invariant -> Domain Type
```

It also includes:

```text
Artifact -> Trait law
Artifact -> Intrinsic semantic contract
Intrinsic -> runtime/library implementation
```

This matters because runtime and library upgrades can change behavior without changing the source VSIR.

For example, `valid-email` currently lowers through `System.Net.Mail.MailAddress`. .NET 10 changed `MailAddress` validation for consecutive dots. The same VSIR and effectively the same C# projection can therefore recognize a different valid-value space after a runtime upgrade.

Consequences:

- intrinsic semantics should be documented independently from their current implementation mapping;
- intrinsic implementations need their own Verification and Challenge campaigns;
- runtime-version changes should trigger intrinsic regression and challenge runs;
- a deterministic source projection is not by itself a guarantee of semantic stability.

## Concrete micro-observations

### StreetExtension

`StreetExtension` declares:

- non-whitespace input;
- total input length from 3 to 16;
- refinement by `split-first-rest`;
- representation by `concat-space`.

Its current C# implementation splits on the literal regular-space separator with `RemoveEmptyEntries`, then reconstructs the representation with one regular space.

This immediately produces several classes of evidence:

#### Verification candidates

- one regular-space-separated name/value pair is accepted;
- missing rest is rejected with the declared structural error;
- lengths 3 and 16 exercise accepted boundaries when structure is valid;
- lengths 2 and 17 exercise rejected boundaries;
- representation of a valid state uses the declared single-space projection;
- reconstructing a projected value is stable.

#### Challenge candidates

- multiple regular spaces;
- leading and trailing regular spaces;
- tab, non-breaking space, zero-width characters, and mixed separators;
- values whose raw length exceeds 16 only because of removable-looking padding;
- inputs that satisfy non-whitespace and length but fail structural refinement;
- different inputs that canonicalize to the same representation.

These candidates do not all represent bugs. Some expose questions the current semantic contract may not answer, such as whether padding is accepted, whether only ASCII space is structural, and whether length constrains raw input or canonical state.

A Challenge failure can therefore be an implementation counterexample, an intrinsic-definition gap, or an ambiguity discovery.

### EmailAddress

`EmailAddress` provides a richer obligation graph:

- trim normalization;
- non-whitespace;
- maximum length 254;
- `valid-email` intrinsic;
- case-insensitive equality;
- string construction adapter;
- database storage with a unique Email index.

Useful relations include:

```text
construct(x) ~= construct(trim(x))
```

for accepted values, and:

```text
construct(caseVariantA) == construct(caseVariantB)
```

when the variants differ only according to the declared equality relation.

The persistence perspective adds a separate question:

```text
Does database uniqueness implement the same equivalence relation as EmailAddress identity?
```

This cannot be answered faithfully by an isolated pure test. It needs the actual database configuration and adapter path.

### ExistingAccount

`ExistingAccount` is a refining invariant:

```text
AccountId -> Account
```

It requires Database, reads Accounts, rejects missing values with its owned error, and otherwise returns the observed Account.

Useful evidence includes:

- an existing ID yields the stored semantic Account;
- a missing ID yields `AccountNotFound` with the expected identity;
- malformed persisted data fails during reconstruction rather than becoming an invalid Account;
- evaluation does not modify database state;
- relevant database states are isolated between candidates.

### AddFile

`AddFile` is a transition requiring Database and ID generation. It reads a Folder, generates an AttachedFile identity, establishes an AttachedFile, appends it, establishes a new Folder state, persists it, and returns the persisted Folder.

Useful evidence includes:

- previous files remain present and ordered;
- exactly one semantically new file appears after success;
- generated identity and supplied content are preserved;
- the resulting Folder and AttachedFile satisfy their Domain Types;
- the database reconstructs the same resulting Folder;
- failure before persistence leaves prior state unchanged;
- repeated, concurrent, or colliding-ID executions expose behavior requiring interpretation.

A realistic Feature campaign therefore normally needs the actual adapters required by the Feature. It does not automatically need a distributed AppHost or browser.

## Evidence surface: fidelity rather than minimum size

The earlier phrase "smallest sufficient boundary" is too strongly influenced by the cost model of manually maintained tests.

The provisional replacement is:

> Use the evidence surface that most faithfully represents the semantic relation being tested.

An even more precise formulation is:

> Maximize claim-relative semantic fidelity, not topology size.

A larger environment is not automatically more realistic. It is more realistic only when the additional components participate in the claim.

Examples:

```text
Domain construction claim
    -> direct Domain construction surface can be faithful

Domain <-> persistence fidelity claim
    -> Domain + real adapter + real database

Database-backed invariant
    -> invariant + real repository/database state

Feature transition
    -> Application + Domain + required real adapters

HTTP/interface contract
    -> hosted application surface, potentially WebApplicationFactory

Distributed journey/topology
    -> Aspire or equivalent whole distributed system
```

This means a Domain Type can have several legitimate evidence perspectives. A direct construction campaign and a persistence-realization campaign are both about the Domain Type, but they test different semantic relations.

## Aspire and current VSIR scope

Aspire testing launches the complete AppHost and its resources as separate processes and is intended for closed-box testing of a distributed application.

The current VSIR artifact kinds generally do not require that surface.

For current Domain Types, invariants, and Features, the largest normal surface is likely a service host plus real dependencies, for example:

```text
WebApplicationFactory
    +
Dockerized database or other real resources
```

Aspire becomes semantically appropriate when the target claim concerns:

- multiple independently hosted applications;
- service discovery or distributed wiring;
- cross-service paths;
- a future scenario, journey, or system-flow representation;
- functional interface behavior whose meaning includes the complete distributed topology.

Aspire is therefore a possible lowering target, not a universal testing layer.

## Production generation and exploratory generation

The experiment confirms that VSlices needs different generation objectives.

### Productive projection

```text
Production VSIR
    -> constrained lowering
    -> convergent, repeatable implementation
```

The objective is to reduce semantic freedom after interpretation.

### Exploratory testing

```text
Production VSIR
    + Testing Intent
    -> diverse hypotheses and candidates
```

The objective is to expand the useful search space.

However, "maximum non-determinism" is not itself the goal. Unconstrained randomness can produce invalid, redundant, or semantically irrelevant tests.

The better objective is:

> Maximize semantic novelty subject to validity, realism, and evidence relevance.

Useful diversity dimensions include:

- different input partitions;
- boundary neighborhoods;
- different state histories;
- technical representation mutations;
- failure timing;
- concurrency schedules;
- alternative metamorphic relations;
- different visibility into the implementation.

The campaign may be non-deterministic. Every candidate retained for review or promotion must be reproducible.

At minimum, evidence output may eventually record:

- source VSIR fingerprint;
- executable projection fingerprint;
- repository commit;
- testing intent identity;
- generator profile/model/version;
- seed or preserved generated candidate;
- lowering/environment profile;
- execution result and diagnostics.

This provenance belongs to generated output/evidence metadata and does not need to block initial test IR design.

## Visibility policies for challenge generation

The challenger does not always need the same information.

A useful future distinction is:

### Semantic black-box Challenge

Sees authoritative semantic artifacts and public contracts, but not implementation details.

Purpose:

- search for counterexamples to represented claims;
- reduce correlation with the productive projection;
- expose missing semantic cases.

### Structural/gray-box Challenge

Sees semantic artifacts, traits, catalogs, capabilities, and lowering structure.

Purpose:

- target boundary relations and compositions;
- generate type- and structure-aware candidates;
- explore condition ordering and state transitions.

### Technical white-box Challenge

Sees implementation, schema, configuration, and runtime behavior.

Purpose:

- challenge the concrete realization;
- mutate persisted values;
- target precision, collation, serialization, concurrency, and platform-specific behavior;
- inspect whether lowering preserves the represented semantics.

### Mutation-guided Challenge

Receives artificial or observed faults not currently detected and generates candidates intended to distinguish the faulty realization from the intended one.

These policies are campaign concerns, not separate definitions of the abstract `Challenge` intent.

## The oracle problem

Verification often has an oracle derived from accepted claims.

Challenge frequently does not.

An AI-generated expected output must not become authoritative merely because it is syntactically plausible.

Potential oracle sources include:

- direct invariant/condition interpretation;
- an independent reference interpreter for VSIR;
- metamorphic relations;
- algebraic/trait laws;
- differential comparison between a semantic model and a real adapter;
- mutation survival or killing;
- human interpretation when the current source does not determine the answer.

A particularly promising architecture is to implement an executable reference interpreter for the semantic VSIR independently from the production C# lowerer.

```text
VSIR
    |-- production lowerer -> executable implementation
    `-- semantic interpreter -> reference result/model

same generated candidate
    -> compare outcomes
```

For a Feature, the semantic interpreter could execute against an abstract model of repository state while the real implementation executes against a real database. The resulting semantic states can then be compared.

This does not eliminate common-mode errors, especially when both depend on the same intrinsic definition. Intrinsics must therefore remain independently challengeable.

## Candidate outcomes and promotion

A candidate execution should not be reduced immediately to `pass` or `fail`.

Possible interpretations include:

```text
supports current claim
concrete implementation counterexample
semantic ambiguity discovered
source VSIR gap discovered
intrinsic/catalog gap discovered
technical adapter mismatch
invalid or irrelevant generated candidate
environment/lowering failure
```

Human review is responsible for the semantic decision.

Promotion can take more than one form.

### Implementation defect; semantics already sufficient

- fix the implementation;
- preserve a stable regression candidate or Verification Intent.

### Semantic gap

- revise Method/Design/Docs understanding;
- update the authoritative VSIR;
- regenerate or refine implementation;
- derive new Verification obligations;
- optionally retain the minimized concrete witness.

### Accepted technical boundary

- record the technical constraint explicitly;
- preserve verification at the relevant adapter surface.

### Useful recurrent exploration

- promote the Challenge campaign itself as a recurring exploration policy rather than freezing one concrete case.

### Noise or invalid hypothesis

- discard the candidate;
- optionally improve the generator or campaign constraints.

This suggests that VSlices may eventually maintain both:

```text
Regression suite
    accepted deterministic Verification artifacts/cases

Challenge campaigns
    accepted repeatable exploration policies producing new candidates
```

## Relationship to editable projections

Current VSlices rules distinguish:

```text
VSIR
    source of truth for intended semantic projection

.vsir.cs
    authoritative executable implementation
```

Testing sits between them.

Verification and Challenge compare intended semantics with executable behavior while respecting that manual projection drift can be legitimate.

A candidate that detects drift is not automatically evidence of a defect. The drift may be:

- compatible with the represented contract;
- representable as a VSIR refinement;
- intentionally opaque implementation detail;
- a real semantic conflict.

Existing source/projection fingerprints are also a natural basis for evidence provenance.

## Provisional process architecture

Without fixing final file formats, the process can be decomposed into these stages:

```text
1. Resolve semantic target and dependency graph
2. Derive deterministic Evidence Obligations
3. Apply Verification or Challenge intent
4. Plan a claim-faithful evidence surface
5. Generate diverse candidate hypotheses/data/sequences
6. Lower candidates into runnable tests and isolated environments
7. Compile, execute, and minimize relevant counterexamples
8. Record evidence and provenance
9. Human interpretation and classification
10. Promote, revise semantic knowledge, retain investigation, or discard
```

Potential internal components are:

- semantic graph resolver;
- obligation extractor;
- campaign planner;
- novelty/diversity selector;
- environment/lowering planner;
- concrete test synthesizer and repair loop;
- runner;
- evidence recorder;
- triage assistant;
- human promotion decision.

This decomposition allows the creative challenger to be different from the deterministic production lowerer and from the concrete test compiler.

## Provisional principles

### Define intent abstractly; specialize application

Verification and Challenge retain one abstract meaning. Their concrete obligations depend on the semantic kind and relation being examined.

### Reference claims; do not duplicate them

Test VSIR should point to target artifacts, invariants, nodes, traits, or obligations rather than restating their semantic rules.

### Derive obligations deterministically

Creative generation should operate over a stable interpretation of what evidence is relevant.

### Let productive generation converge

Production lowering should reduce freedom and preserve semantic continuity.

### Let exploration diverge usefully

Challenge should seek semantic novelty, not arbitrary randomness.

### Make discoveries reproducible

Non-deterministic discovery must yield preserved, rerunnable candidates and evidence.

### Select surfaces by claim-relative fidelity

Do not minimize or maximize topology mechanically. Reproduce the semantic relation under examination as faithfully as practical.

### Separate candidate generation from authority

Generated tests are hypotheses. Human interpretation determines promotion and semantic change.

### Treat intrinsics and traits as knowledge

They contribute semantics and evidence obligations and can drift independently from artifact text.

### Accumulate safety

Validated discoveries should return through Method/Design/Docs, refine the semantic source when warranted, and become regression protection.

## Important unresolved questions

The experiment narrows the open questions to the following.

1. What exact Evidence Obligations are contributed by every current VSIR node, trait, classification, and intrinsic?
2. How should a test intent reference one invariant, condition path, intrinsic, trait law, or derived obligation without fragile textual paths?
3. Does VSlices need a represented Infrastructure/adapter contract, or is an implementation-inspecting technical Challenge sufficient initially?
4. Can a semantic VSIR interpreter be made independent enough from the production lowerer to serve as a useful oracle?
5. Which evidence perspectives should be mandatory for Domain Types, invariants, and Features?
6. How should semantic novelty and candidate redundancy be evaluated?
7. Which Challenge campaigns should recur after their initial exploration?
8. When should a discovery promote a concrete test, a Verification Intent, a new invariant, or several of these?
9. How should runtime and library changes invalidate or re-run intrinsic evidence?
10. What is the smallest useful authored test-intent model after deterministic obligation derivation is available?

## Suggested next experiment

A useful next vertical experiment would use three artifacts with increasing environmental requirements:

```text
StreetExtension
    pure Domain Type with refinement and canonical representation

ExistingAccount
    reusable invariant requiring database observation

AddFile
    Feature transition requiring database and identity generation
```

For each target:

1. derive obligations manually;
2. define one Verification intent and one Challenge intent without copying source claims;
3. generate several candidates;
4. select the most faithful runnable surface;
5. classify results through the proposed promotion model;
6. use the friction to design only the minimum next piece of test IR.

This would test the model vertically before attempting a complete testing language.
