# Composable Intermediate Representations

## Status

Exploratory architectural note.

This document records a working hypothesis about the role of multiple intermediate representations across VSlices and the broader PResolver family. It is not yet a specification for a concrete syntax, compiler pipeline, or finalized set of file extensions.

## Motivation

VSlices currently focuses on technical and semantic representation of software concepts. This is intentional: a technical representation should not silently invent visual, experiential, stylistic, organizational, or other decisions that belong to different problem dimensions.

At the same time, software artifacts are rarely determined by technical semantics alone. A page, for example, can depend simultaneously on:

- domain and application semantics;
- interaction intent;
- visual composition;
- accessibility constraints;
- product conventions;
- available component capabilities;
- user customization;
- and implementation constraints.

Trying to collapse all of these dimensions into one representation would make the representation harder to reason about, harder to specialize, and more likely to couple unrelated concerns.

The alternative explored here is to allow several specialized intermediate representations to describe different dimensions of the same concept, and then compose them through explicit resolution.

## Working model

The current hypothesis contains at least three distinct roles.

### `.vsir` — technical representation

VSlices produces or consumes `.vsir` as the intermediate representation of the technical semantics of a software concept.

A `.vsir` should describe what is technically relevant to the concept: for example its domain meaning, inputs and outputs, invariants, capabilities, features, transformations, states, relationships, or other executable semantics recognized by VSlices.

It should not need to decide how that concept looks.

For a page, a future technical view-oriented `.vsir` could describe facts such as:

- which domain concepts are represented;
- which values can be edited;
- which transformations materialize user input into domain values;
- which operations can be submitted;
- which states exist;
- which errors can be observed;
- which capabilities are required;
- and which interactions have technical consequences.

This representation would remain technical even when its target materialization is a user interface.

### `.prir` — problem-resolution coordination

PResolver is a general architecture for resolving problems while preserving continuity of the knowledge involved in the resolution.

A `.prir` is proposed here as the intermediate representation concerned with coordinating resolution knowledge: how distinct concerns, constraints, decisions, evidence, artifacts, patterns, and specialized representations participate in solving a problem.

In this role, `.prir` is not a replacement for `.vsir`.

Rather, it may describe how a technical representation, a visual representation, user preferences, product constraints, and other specialized knowledge should be combined or resolved.

Conceptually:

```text
problem/context
    ↓
.prir
    ↓ coordinates
specialized representations + constraints + capabilities
    ↓
resolved artifact plan
```

The exact semantics and syntax of `.prir` remain to be formalized.

### Visual / experiential representation — derived from PResolver

A PResolver-derived specialization has already been proposed for interactive experiences: **IEnacta**.

IEnacta is broader than visual design. Its current purpose concerns the design, representation, implementation, observation, and validation of interactive experiences, preserving continuity between intended experience, interaction, system response, observed experience, and later adjustment.

A visual intermediate representation may therefore emerge from IEnacta or from a more specialized flavor derived from it.

This document intentionally does **not** assign a final file extension or claim that IEnacta itself is merely a visual-design system.

A future visual representation could describe concerns such as:

- hierarchy;
- layout;
- grouping;
- density;
- visual emphasis;
- responsive behavior;
- visual states;
- component variants;
- spacing;
- typography;
- design tokens;
- and other decisions that belong to presentation rather than technical semantics.

Other experiential concerns may require additional representations rather than being forced into the same visual IR.

## Composition instead of one universal IR

The important idea is not the existence of three file formats.

The important idea is that **a final artifact can emerge from the composition of several partial, specialized representations**.

For example, generating a page could eventually resemble:

```text
Technical semantics
    .vsir

Interaction / visual intent
    specialized PResolver-derived IR

Resolution and coordination knowledge
    .prir

User or product customization
    preferences / constraints

Available implementation capabilities
    component catalog / platform capabilities

            ↓ resolve together

Deterministic implementation plan

            ↓ lower

Razor / HTML / CSS / tests / documentation / other materializations
```

Each representation answers a different class of questions.

The resolver should combine them without forcing one representation to understand every dimension of the artifact.

## Determinism through constrained freedom

This model is particularly relevant to AI-assisted generation.

Without intermediate representations, a request such as "generate the registration page" leaves a very large space of possible implementations. The model must infer technical behavior, interaction patterns, visual structure, components, validation, styling, and architecture at the same time.

With specialized representations, much of that freedom becomes explicit knowledge.

For example:

```text
.vsir
    says that EmailAddress is an editable domain concept
    transformable from text
    and required by RegisterUser

visual IR
    says that the contact section uses a two-column layout on wide screens
    and a single column on narrow screens

.prir
    coordinates those requirements with the available component catalog

component metadata
    says that vTextInput<T> can materialize a text representation into T
```

The generator no longer needs to invent the relationship between these facts. It needs to resolve a constrained composition.

This can make AI generation substantially more deterministic while still allowing controlled variation.

## Customization is a first-class requirement

Determinism must not imply that every VSlices application looks identical.

A bootstrap risk exists if the official VSlices visual layer eventually becomes the only visual vocabulary available: technically different products could converge toward many nearly identical pages simply because generation has only one canonical visual answer.

Therefore customization should participate as an explicit input to resolution.

A user, team, or product may provide additional constraints or replace defaults such as:

- design tokens;
- component mappings;
- composition preferences;
- density;
- visual language;
- interaction conventions;
- accessibility requirements;
- brand constraints;
- or even an alternative visual specialization.

The goal is not:

> one deterministic visual result for every technical concept.

The goal is closer to:

> given the same set of explicit technical, visual, product, and user constraints, resolution should converge toward a highly predictable materialization.

This preserves customization without returning to unconstrained generation.

## Multiple PResolver flavors

The same composition model need not be limited to views.

If PResolver can derive specialized suites for different families of problems, those flavors may produce representations concerned with different dimensions of an artifact.

Potential examples include representations specialized for:

- technical semantics;
- interaction and experience;
- visual design;
- testing and evidence;
- documentation;
- deployment or operation;
- accessibility;
- security;
- narrative continuity;
- research evidence;
- or other future concerns.

A complex artifact may then be generated from the intersection of several compatible representations.

Conceptually:

```text
                     ┌─ technical IR
                     ├─ visual IR
                     ├─ interaction IR
PResolver / .prir ───┼─ testing IR
                     ├─ documentation IR
                     ├─ user customization
                     └─ platform capabilities
                              │
                              ▼
                       resolved artifact
```

This suggests a family of interoperable IRs rather than a single universal schema.

## Implication for VSlices Views

The current decision to keep the VSlices view abstraction technical and visually unopinionated is compatible with this hypothesis.

An official VSlices visual layer can be designed later, once a sufficiently mature PResolver-derived visual specification exists.

Until then, real pages such as Login, RegisterUser, Profile, and other product views can be used to discover reusable technical view capabilities without prematurely freezing a universal visual language.

This also suggests a useful development direction:

1. discover stable technical view primitives from real product work;
2. expose their capabilities through machine-readable metadata;
3. separately formalize visual and experiential specifications;
4. make customization explicit;
5. resolve these representations against the component catalog;
6. lower the resulting plan into concrete UI code;
7. verify the generated materialization against executable tests and experiential evidence.

## Broader hypothesis

The deeper hypothesis is that software generation can become less dependent on free-form synthesis by moving knowledge into interoperable intermediate representations.

Instead of asking an AI to invent an artifact directly, the system can progressively constrain the solution through specialized knowledge:

```text
understand
    ↓
represent each concern
    ↓
coordinate representations
    ↓
resolve compatible choices
    ↓
produce a deterministic plan
    ↓
lower into concrete artifacts
    ↓
verify
```

In that model, AI remains useful for interpretation, resolution, exploration, and controlled synthesis, but it operates inside an increasingly explicit semantic space.

The combination of several PResolver flavors may therefore support artifacts that are simultaneously:

- highly customizable;
- highly reproducible;
- semantically constrained;
- machine-readable;
- evolvable;
- and substantially more deterministic than unconstrained code generation.

## Open questions

This note intentionally leaves several questions unresolved:

- What is the minimum stable semantic contract of `.prir`?
- Is `.prir` itself an executable coordination representation, a knowledge graph projection, or both?
- Should visual representation belong directly to IEnacta or to a narrower specialization derived from it?
- Which concerns deserve separate IRs rather than additional sections in an existing one?
- How are conflicts between representations detected and resolved?
- How are defaults distinguished from user overrides?
- How can component capabilities be described without coupling an IR to one UI framework?
- How should provenance be preserved so a materialized decision can be traced back to the representations that constrained it?
- How do verification and challenge operate over composed representations?
- At what point is a resolved plan sufficiently constrained to be considered deterministic?

These questions should be answered through real materializations rather than by prematurely expanding the schema.

## Current practical experiment

The migration of Serviu views provides an immediate experimental surface.

Login has already exposed technical primitives around typed input, domain transformation, validation, and submission.

RegisterUser can extend this with sections, heterogeneous fields, feedback, state, and orchestration across Accounts and Identities.

Profile and later views can expose additional needs.

The objective during this phase is not yet to invent a complete visual IR. It is to gather enough real evidence to distinguish stable representation concepts from page-specific accidents.

The resulting component set and generated pages may later become one of the corpora used to validate whether composed technical, visual, and coordination representations can reconstruct equivalent interfaces with less ambiguity and greater determinism.
