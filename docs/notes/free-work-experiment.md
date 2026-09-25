# Free Work Experiment

> Status: **paused / preserved for later research**
>
> This document preserves the reasoning, executable evidence, failures, design tensions, and open questions discovered while experimenting with Free Monads as a representation of VSlices Work.
>
> The experiment is intentionally **not** treated here as the current required architecture. The code may be reverted or simplified without discarding the knowledge recorded below.
>
> The purpose of this note is continuity: if VSlices returns to this direction later, the exploration should resume from the actual limits already observed rather than rediscovering the same path from scratch.

## 1. Executive summary

The experiment began while trying to replace historically overloaded persistence concepts such as Repository, Store, and Unit of Work with smaller Work capabilities.

The first useful distinction was:

```text
Capability
    what Work can request

Guarantee
    a semantic property an admissible realization must preserve

Grounding
    how the capability is realized
```

That led to small point-oriented capabilities:

```text
PointReader
PointWriter
PointRemover
```

The first implementation returned `IO` directly from capability interfaces. That immediately revealed a problem: an `IO`-returning capability combines two distinct things:

```text
instruction vocabulary
+
execution / interpretation
```

LanguageExt's `Free<F, A>` appeared to solve that separation naturally.

The central hypothesis evolved through several stages:

```text
initial idea
    Free = an implementation detail used inside Feature

first executable experiment
    Feature -> Flow -> Free program -> interpreter

problem
    Feature became a wrapper while TodoPrograms owned the real behavior

revised idea
    Feature == WorkFlow == Free<ALG, Response>

later pressure from TicketSupport.BFF
    WorkProcess coordinates multiple WorkFlows

composition hypothesis
    child WorkFlow algebras can be injected into a larger algebra
    and their Free programs can be hoisted into that algebra
```

This was technically promising and several pieces worked in executable tests.

However, the experiment also exposed substantial unresolved costs:

- algebra/ADT ceremony became large very quickly;
- much of that ceremony was mechanical rather than semantic;
- a Free program only exposes operations represented in its algebra, so ordinary C# continuations can hide semantically meaningful WorkParts;
- composed algebras can flatten WorkFlow boundaries that may later matter for retry, authorization, tracing, guarantees, compensation, or ownership;
- positional `AlgebraSum<A,...>` composition scales poorly as a handwritten API;
- the approach caused a large redefinition of `Feature`, execution wiring, Grounding, and documentation before VSIR/Tooling had a corresponding language;
- guarantees such as Atomic, Tracking, Durability, ordering, staged persistence, and cross-service compensation remained unsolved;
- the exact relationship between WorkPart, WorkFlow, WorkProcess, Feature, algebra, interpreter, and host composition still required more evidence.

The important conclusion is therefore not "Free was wrong".

The useful conclusion is:

> Free Monads remain a plausible mechanism for representing compositional Work, especially WorkFlow composition, but the current implementation crossed the line from a useful experiment into an architectural commitment before the surrounding VSlices Work model and Tooling language were sufficiently settled.

The code can be rolled back while the experiment remains worth revisiting.

---

# 2. Original pressure: persistence vocabulary was too coarse

The exploration did not begin with Free Monads.

It began with a problem in the previous persistence surface:

```text
Repository<A, ID>
    Create
    Read
    Read(id)
    Any
    Update
    Delete
```

This grouped operations because traditional software architecture commonly groups them, not because VSlices had independently established that the grouping was semantically meaningful.

The same problem existed with names such as:

```text
Repository
Store
UnitOfWork
PersistenceContext
```

These names carry historical expectations about:

- CRUD;
- transaction boundaries;
- staged state;
- persistence authority;
- identity;
- aggregation;
- tracking;
- save semantics;
- DDD ownership.

For the intended VSlices continuity pipeline:

```text
documentation
    -> interpretation
    -> VSIR
    -> Tooling
    -> code
```

that is dangerous.

A document may establish only:

```text
can read X
can write X
can remove X
```

without establishing enough evidence to infer "Repository".

The working principle became:

> Prefer the smallest semantic capability supported by evidence. Do not require an AI, VSIR ruleset, or developer to recognize an entire historical pattern when the available evidence only establishes one operation.

This remains useful independently of Free Monads.

---

# 3. Capabilities and guarantees separated

A second important discovery was that capability and guarantee are not the same category.

For example:

```text
PointWriter
```

describes an operation Work can request.

By contrast:

```text
Atomic
Tracking
Durable
Ordered
Isolated
```

do not normally describe useful operations by themselves. They constrain admissible realizations.

The working model became:

```text
Capability
    what can be requested

Guarantee
    what must remain true about the realization

Grounding
    mechanism used to realize the capability
    while satisfying the required guarantees
```

Examples:

```text
Work requires:
    Tracking

EF Grounding may realize:
    DbContext + ChangeTracker
```

but:

```text
Tracking != DbContext
```

Similarly:

```text
Atomic != SQL transaction
```

A SQL transaction may provide evidence for an Atomic guarantee, but it is not the semantic definition of Atomic.

This distinction also produced the important epistemic rule:

```text
Unknown != False
```

A guarantee can be:

```text
Proven
    derived from known laws

Discharged
    established by explicit Grounding/ruleset evidence

Unresolved
    requested, but evidence is currently insufficient

Contradicted
    available semantics conflict with the guarantee
```

This part of the exploration is largely orthogonal to Free and remains potentially useful even if all Free-related code is reverted.

See:

```text
docs/notes/capabilities-and-guarantees.md
```

---

# 4. Point capabilities

The first concrete capability vocabulary discovered under real pressure was:

```text
PointReader<ALG, POINT, ID>
PointWriter<ALG, POINT>
PointRemover<ALG, POINT, ID>
```

The names intentionally use "Point" because the Framework already describes values as points inhabiting semantic spaces.

The intended meanings were deliberately small.

## PointReader

```text
read a point of semantic space POINT by ID
```

It does not imply:

- tracking;
- persistence authority;
- transactions;
- write access;
- enumeration;
- query language;
- Repository semantics.

## PointWriter

```text
establish externally represented state for a semantic point
```

The experiment initially treated both creation and replacement/update as expressions of PointWriter.

That was deliberate:

```text
Create
    may be Feature semantics:
        read
        if absent -> write

Update
    may be Feature semantics:
        read
        if present -> evolve
        write
```

This avoided introducing separate primitives merely because CRUD traditionally has separate verbs.

This remains an open semantic choice rather than a universal rule.

## PointRemover

PointRemover was not introduced for symmetry.

It appeared when the CRUD experiment reached Delete and required an independently expressible operation.

That was considered good design pressure:

```text
real case
    -> first missing operation
    -> introduce minimum vocabulary
```

---

# 5. Why direct IO capability interfaces felt wrong

An early candidate looked conceptually like:

```csharp
interface PointReader<A, ID>
{
    IO<Option<A>> Read(ID id);
}
```

This was attractive because it was small.

It also collapsed two things:

```text
what operation Work means
    +
how/when that operation executes
```

The capability object became the interpreter.

This made it harder to preserve the VSlices distinction between:

```text
semantic Work
Grounding
```

and made a Feature immediately commit to `IO`.

That pressure led directly to the Free Monad experiment.

---

# 6. Free Monad discovery

The relevant modern LanguageExt shape was:

```csharp
Free<F, A>
    where F : Functor<F>
```

with:

```text
Pure<F, A>
Bind<F, A>
Free.lift(...)
Free.pure(...)
```

A Free program can sequence instructions from a functorial vocabulary without executing those instructions.

This immediately mapped onto the desired separation:

```text
algebra / instruction functor
    what operations exist

Free<ALG, A>
    program composed from those operations

interpreter
    how the operations execute

Grounding
    concrete contact with the outside world
```

The initial mapping was:

```text
PointReader / PointWriter / ...
    primitive operations

service algebra
    vocabulary

Free<ALG, A>
    program

AlgebraIO<ALG>
    interpreter
```

This was the first strong reason to continue the experiment.

---

# 7. First executable experiment

The first executable experiment used a small Account algebra.

It proved several things:

1. constructing a Free program did not execute anything;
2. the same program could be interpreted by different Groundings;
3. multiple semantic spaces could appear in one algebra;
4. an interpreter could execute the instructions into `IO`;
5. the Free program could initially be embedded inside the then-current `Flow` execution model.

The test shape was roughly:

```text
program
    ReadAccount
    WriteAccount
    ReadAccount

Grounding A
    existing account

Grounding B
    missing account
```

The same program structure produced different results because interpretation was separate from program construction.

This validated the core syntax/interpreter separation.

At this stage the model was still approximately:

```text
Feature
    -> Flow
        -> Free program
            -> AlgebraIO
                -> IO
```

This was later rejected as the primary Feature model.

---

# 8. CRUD experiment and semantic-space pressure

A deliberately small Todo CRUD API was created to test whether the model survived contact with a VSlices-like example.

The target shape was:

```text
SampleWorkflow.Spaces
SampleWorkflow.Work
SampleWorkflow.Grounding
SampleWorkflow.Api
```

The first version exposed a weakness unrelated to Free:

```csharp
record Todo(TodoId Id, string Title, bool Completed)
```

was typed C# but not especially VSlices-like.

The experiment was corrected using the older Serviu Domain code as evidence.

## TodoId

`TodoId` became a semantic space established from `Guid`:

```text
Guid
    -> TodoId.Transformation
    -> TodoId
```

This preserved the distinction between:

```text
source representation
semantic identity
```

Identity generation was also separated from identity meaning:

```text
TodoId.Transformation
    says how a Guid can establish TodoId

identity generation Grounding
    says how a new source Guid is obtained
```

## TodoDetail

An intermediate version incorrectly made TodoDetail own both:

```text
string detail
bool completed
```

This was corrected.

The final experiment treated:

```text
TodoDetail : DiscreteSpace<TodoDetail>
    wraps only the semantic string

Completed
    remains bool
```

because there was no evidence requiring a separate semantic space for the boolean.

## Todo and Evolvable

Todo was corrected to implement:

```text
Evolvable<Todo, Todo.State>
```

with identity fixed and mutable/evolvable coordinates such as:

```text
TodoDetail Detail
bool Completed
```

The PUT path therefore became conceptually:

```text
read current Todo
    -> Todo.Update(state => state with {...})
    -> write only the accepted evolved Todo
```

rather than:

```text
construct arbitrary replacement Todo
    -> overwrite
```

This was an important reminder:

> Free composition must not replace Space semantics. Work should still use semantic construction/evolution mechanisms owned by Space.

---

# 9. First major problem: TodoPrograms became the real Feature

The first CRUD implementation introduced:

```text
CreateTodo Feature
GetTodo Feature
UpdateTodo Feature
DeleteTodo Feature

TodoPrograms.Create
TodoPrograms.Read
TodoPrograms.Update
TodoPrograms.Delete
```

The Features mostly:

- accepted Request;
- called `TodoPrograms.*`;
- interpreted/adapted the result;
- returned Response.

The actual use-case behavior lived in TodoPrograms.

This created a clear architectural smell:

```text
Feature
    wrapper

TodoPrograms
    actual application behavior
```

That contradicted the intended role of Feature.

The useful conclusion was not "Free is incompatible with Feature".

The useful conclusion was:

> If Feature is the WorkFlow, the Free program must be the Feature's behavior rather than a parallel architectural unit one-to-one with the Feature.

The `TodoPrograms` layer was therefore considered a negative result of the experiment.

---

# 10. Revised hypothesis: Feature == WorkFlow == Free program

The next hypothesis was much stronger:

```text
Feature == WorkFlow
Feature behavior == Free<ALG, Response>
```

Conceptually:

```csharp
Feature<F, ALG, REQ, RES>
{
    static Free<ALG, RES> Get(REQ request);
}
```

The intended meaning became:

```text
REQ
    Feature/WorkFlow input

ALG
    WorkPart vocabulary available to this WorkFlow

Free<ALG, RES>
    composition of those WorkParts

RES
    WorkFlow result
```

Runtime disappeared from the semantic Feature contract.

This was considered attractive because:

```text
Feature describes Work
runtime executes Work
```

rather than:

```text
Feature already contains runtime requirements and effect execution mechanics
```

The then-existing `Flow<RT, REQ, RES>` abstraction eventually became unnecessary under this experiment and was later removed from master for the broader Work simplification.

That removal should not be interpreted as proof that the Free model is necessarily correct; it was part of a wider simplification trajectory.

---

# 11. Feature-owned algebra

The service-wide algebra idea also changed.

Initially:

```text
AppAlgebra
    PointReader<Account>
    PointWriter<Account>
    PointReader<Role>
```

was shared by multiple Work operations.

Later pressure suggested:

```text
each WorkFlow owns only the WorkParts it needs
```

For example:

```text
GetTodo.Algebra
    ReadTodo

UpdateTodo.Algebra
    ReadTodo
    WriteTodo

DeleteTodo.Algebra
    ReadTodo
    RemoveTodo
```

This made the type-level vocabulary much more precise.

It also introduced significant ceremony.

For each Feature algebra the code typically needed:

- an operation base type;
- one record per WorkPart;
- continuation fields;
- implementation of `Functor.Map`;
- PointReader/Writer/Remover adapters;
- an interpreter branch for every operation.

Much of this code was mechanically derived from the operation vocabulary.

That immediately suggested:

> If this direction ever returns, Tooling/VSIR should probably generate the ADT, continuation plumbing, Functor.Map, and possibly interpreter adapter skeletons.

Handwriting those pieces is poor signal-to-noise.

---

# 12. WorkPart visibility problem

A deeper problem appeared when looking at UpdateTodo.

Suppose the Free algebra contains only:

```text
ReadTodo
WriteTodo
```

but the Feature code contains:

```csharp
todo.Update(state => state with
{
    Detail = ...,
    Completed = ...
})
```

Mathematically this works.

But the Free AST only has explicit nodes for:

```text
ReadTodo
WriteTodo
```

The semantic evolution is hidden inside an arbitrary C# continuation.

This creates a serious question:

> What exactly is a WorkPart?

If "Evolve Todo" is a meaningful WorkPart, then representing only effectful external operations in the algebra produces an incomplete WorkFlow representation.

If pure semantic operations are intentionally *not* WorkParts, then the Free program is not a complete structural description of the WorkFlow; it is only the compositional description of externally interpreted operations.

Both models are possible.

The experiment did not settle which one VSlices actually wants.

This is one of the strongest reasons not to treat the current Free representation as finished.

---

# 13. Relationship to the Work hierarchy

During the experiment the older Work hierarchy became relevant:

```text
1 WorkLine
    : N WorkProcess

1 WorkProcess
    : M WorkFlow

1 WorkFlow
    : Q WorkPart
```

Free composition looked much closer to this hierarchy than to a generic persistence abstraction.

The strongest mapping discovered was:

```text
WorkPart
    instruction

WorkFlow
    Feature
    Free program composed from WorkParts
```

This was a much better conceptual fit than:

```text
Free == repository replacement
```

The experiment therefore shifted from "Free persistence capabilities" toward:

> Free may be a general executable representation of VSlices Work composition.

That remains an interesting research direction.

---

# 14. TicketSupport.BFF pressure: WorkProcess is real

TicketSupport.BFF supplied real evidence for the next level.

For example, attachment upload currently composes behavior from two different service boundaries:

```text
TicketSupport.BFF.AttachFile
    -> Folders.AddFile
    -> Tickets.AddAttachmentReference
```

Likewise removal composes:

```text
Tickets.RemoveAttachmentReference
    -> Folders.RemoveFile
```

The BFF does not own the internal semantics of either service Feature.

It owns the product-level coordination.

This matched the Work hierarchy naturally:

```text
Folders.AddFile
    WorkFlow

Tickets.AddAttachmentReference
    WorkFlow

TicketSupport.AttachFile
    WorkProcess
```

This was stronger evidence than the artificial Todo CRUD because it already existed in a real product boundary.

Relevant current evidence lives in:

```text
atom-dev-serviu/access-management-product

products/ticket-support-product/TicketSupport.BFF/Features/Tickets/AttachFile.cs
products/ticket-support-product/TicketSupport.BFF/Features/Tickets/RemoveAttachment.cs
products/ticket-support-product/TicketSupport.BFF/Features/Tickets/DownloadAttachment.cs
products/ticket-support-product/docs/final-integration-state.md
services/ticket-service/docs/capability-ownership.md
services/ticket-service/docs/concept-classification.md
```

---

# 15. First WorkProcess idea: higher-order RunWorkFlow operation

One possible representation was considered:

```text
WorkProcess algebra
    RunWorkFlow(Free<ChildAlgebra, A>)
```

This would make WorkFlow boundaries explicit.

Conceptually:

```text
RunWorkFlow(Folders.AddFile)
    >>= stored =>
RunWorkFlow(Tickets.AddAttachmentReference(stored.Id))
```

This has a useful property:

```text
WorkFlow remains an explicit node inside WorkProcess
```

That could matter later for:

- tracing;
- authorization;
- retries;
- compensation;
- observability;
- guarantee scopes;
- ownership boundaries.

However, this is a higher-order effect shape because an operation contains another program.

The experiment did not pursue that implementation deeply because a simpler mechanism became available.

---

# 16. Simpler WorkProcess composition: algebra sum + hoist

The simpler idea was:

```text
Child WorkFlow A
    Free<AAlgebra, X>

Child WorkFlow B
    Free<BAlgebra, Y>

Process algebra
    AAlgebra + BAlgebra
```

Each child program is then injected into the larger algebra using a natural transformation and hoisted:

```text
AAlgebra
    -> ProcessAlgebra

BAlgebra
    -> ProcessAlgebra
```

so:

```text
Free<AAlgebra, X>
    -> Free<ProcessAlgebra, X>

Free<BAlgebra, Y>
    -> Free<ProcessAlgebra, Y>
```

After that, ordinary monadic composition can sequence the child WorkFlows.

This mapped directly to LanguageExt's mathematical `Free.hoist` operation.

The experimental mechanism became:

```text
AlgebraSum<A, B>
InjectA / InjectB
FreeAlgebra.hoist
AlgebraSumIO<A, B>
```

and later positional arities were explored for more child algebras.

---

# 17. Important ownership issue with LanguageExt Free.hoist

LanguageExt's `Free.hoist` API uses a constraint conceptually similar to:

```csharp
where F : Natural<F, G>
```

That shape means the source algebra type `F` knows how to transform itself into `G`.

For VSlices composition this ownership is often backwards.

Example:

```text
FoldersAlgebra
    must not know TicketSupportAlgebra
```

The BFF/Product Process should own the injection:

```text
FoldersAlgebra
    -> TicketSupportAlgebra
```

because the composing context knows both vocabularies.

The experiment therefore introduced an external natural-transformation witness:

```text
InjectFoldersIntoTicketSupport
```

and a VSlices `FreeAlgebra.hoist<N, F, G, A>` helper whose natural transformation is supplied separately.

This preserved:

```text
mathematical mechanism
    Free hoist

ownership
    composing Process owns the injection
```

This was considered a good result.

---

# 18. Interpreter composition and DI

The same composition appeared on the realization side.

A service can have one concrete implementation that satisfies several independent WorkFlow algebras:

```text
TodoServiceGrounding
    AlgebraIO<CreateTodo.Algebra>
    AlgebraIO<GetTodo.Algebra>
    AlgebraIO<UpdateTodo.Algebra>
    AlgebraIO<DeleteTodo.Algebra>
```

When executing one WorkFlow directly, DI only needs to provide the corresponding interface:

```text
AlgebraIO<CreateTodo.Algebra>
```

For a BFF/Product Process with a composed algebra:

```text
ProcessAlgebra
    = AlgebraSum<FoldersAlgebra, TicketsAlgebra>
```

the Process interpreter can simply delegate:

```text
Folders operation
    -> Folders interpreter

Tickets operation
    -> Tickets interpreter
```

This is attractive because the BFF does not reimplement service Grounding.

The semantic and realization compositions mirror each other:

```text
SEMANTIC

AAlgebra ----\
              +--> ProcessAlgebra
BAlgebra ----/


REALIZATION

AInterpreter -\
               +--> ProcessInterpreter
BInterpreter -/
```

This remains one of the strongest parts of the experiment.

---

# 19. AlgebraSum scaling problem

The simple binary sum is manageable:

```text
AlgebraSum<A, B>
```

But real BFF composition can involve several WorkFlows.

The experiment began extending positional sums:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
...
AlgebraSum<A, B, C, D, E, F, G>
```

with:

```text
FromA
FromB
...
InjectA
InjectB
...
```

This is mechanically possible.

It is also strong evidence that handwritten positional algebra sums are not a satisfying semantic API.

Problems include:

- type signatures become very large;
- positional letters have no domain meaning;
- reordering children changes the type;
- adding one child can change a substantial generic surface;
- handwritten injection/interpreter code grows quickly;
- diagnostics become difficult to read;
- Tooling/VSIR must know how to generate and lower these compositions.

Potential future alternatives include:

- Tooling-generated sums;
- named sum types owned by the Process;
- generic coproduct encodings;
- generated nested binary sums;
- another extensible algebra representation;
- higher-order WorkFlow invocation instead of flattening.

No option was proven superior.

---

# 20. Flattening can erase WorkFlow boundaries

Hoisting child algebras into one Process algebra has an important semantic consequence.

After hoisting:

```text
Folders WorkParts
Tickets WorkParts
```

all inhabit one Free program.

This preserves the source algebra branch, but it may not preserve an explicit node saying:

```text
BEGIN Folders.AddFile
END Folders.AddFile
```

That may be acceptable if WorkFlow boundaries do not matter during interpretation.

It may be unacceptable if VSlices later needs guarantees/scopes such as:

```text
retry this WorkFlow
authorize this WorkFlow
trace this WorkFlow
compensate this WorkFlow
establish atomicity for this WorkFlow
apply timeout to this WorkFlow
```

This creates a key unresolved design choice:

```text
hoist/flatten
    simpler composition
    potentially weaker explicit boundaries

higher-order RunWorkFlow node
    preserves boundaries
    more complex effect model
```

The experiment did not have enough evidence to decide.

---

# 21. Feature versus WorkProcess ambiguity

At one point the model introduced a separate `WorkProcess<P, ALG, REQ, RES>` abstraction.

Later reasoning suggested that this may be unnecessary.

A Feature that composes child Features could remain a Feature:

```text
Feature == WorkFlow surface

Feature with composed ALG
    may coordinate child WorkFlows
```

This creates a terminology problem.

If the Work hierarchy is semantically important:

```text
WorkProcess != WorkFlow
```

then using the same Feature type for both can erase level information.

If Feature is intentionally only an executable surface and WorkProcess/WorkFlow are descriptive levels, the reuse may be acceptable.

The experiment did not settle whether:

```text
Feature == WorkFlow only
```

or:

```text
Feature
    is a generic executable Work surface
    that may represent WorkFlow or WorkProcess
```

is the better model.

This ambiguity should be resolved before rebuilding the abstraction.

---

# 22. Free is not a complete Work model by itself

A recurring temptation was to say:

```text
Free program == Work
```

The experiment provided evidence against making that claim too quickly.

A Free program gives:

- sequencing;
- dependency of later operations on earlier results;
- inert syntax before interpretation;
- separation between instruction language and interpreter.

It does not automatically give:

- semantic ownership;
- guarantee scopes;
- authorization;
- compensation;
- concurrency model;
- transaction semantics;
- cancellation;
- observability;
- WorkLine/WorkProcess metadata;
- domain admissibility;
- state evolution semantics;
- distributed coordination semantics.

Those require additional VSlices concepts.

So the useful statement is narrower:

> Free is a potentially useful compositional representation for some VSlices Work structures.

Not:

> Free defines VSlices Work.

---

# 23. Guarantees remained unresolved

Free makes a program inspectable only to the extent that the relevant semantics are explicit instructions.

It does not prove behavioral guarantees.

The following remained open throughout the experiment:

```text
Tracking
Atomic
Isolation
Durability
Ordering
Delivery
Staged persistence
Self persistence
Transaction boundaries
Cross-service compensation
Retry semantics
```

This is especially important for TicketSupport attachment operations.

For example:

```text
Folders.AddFile
    succeeds

Tickets.AddAttachmentReference
    fails
```

A composed Free program describes sequence.

It does not solve:

- orphan cleanup;
- compensation;
- retry;
- reconciliation;
- distributed atomicity.

Those remain Process/Guarantee concerns.

---

# 24. PointWriter and persistence semantics remained intentionally incomplete

The experiment used PointWriter as a small operation.

That does not settle all persistence semantics.

Questions still open include:

- Is Write always immediate/self-persisting?
- Can a Grounding stage Write until a later boundary?
- If staged, where is persistence authority represented?
- Does Write mean upsert?
- Does Work need explicit Create under some semantics?
- Does replacement preserve identity?
- How are concurrency conflicts represented?
- What is the guarantee of visibility after Write?
- Is read-after-write expected in one interpretation scope?
- How do transactions span multiple point operations?

The EF Grounding experiment used concrete behavior such as `SaveChanges` during mutation.

That was implementation evidence, not universal PointWriter semantics.

---

# 25. Removal semantics are also narrower than CRUD Delete

PointRemover was useful under simple CRUD pressure.

Real systems may distinguish:

```text
physical delete
logical delete
deactivate
archive
revoke
expire
tombstone
detach association
remove external resource
```

Therefore:

```text
PointRemover
```

should not automatically be treated as the universal lowering of every domain "Delete" operation.

The CRUD experiment only established that a physical in-memory removal operation was useful there.

---

# 26. Query/read vocabulary was not explored deeply

The experiment mainly covered point-addressed CRUD:

```text
read by id
write point
remove by id
```

It did not establish semantics for:

- read all;
- filtering;
- searching;
- projections;
- pagination;
- joins;
- existence probes;
- aggregates;
- streams;
- temporal queries;
- product-level query composition.

Restoring Repository merely to obtain those operations would repeat the original mistake.

If the Free direction returns, query vocabulary should still be discovered from real pressure.

---

# 27. Tooling pressure

The Free experiment repeatedly produced code that was correct but mechanically noisy.

Typical generated-looking pieces:

```text
abstract WorkPart<A>
ReadPart<A>(..., Func<Result, A> Next)
WritePart<A>(..., Func<Unit, A> Next)
Functor.Map
Free.lift constructors
AlgebraSum branches
InjectA / InjectB / ...
AlgebraSumIO delegation
```

These are poor candidates for handwritten application code.

If Free returns, a more VSlices-like path is probably:

```text
documentation
    -> VSIR declares WorkParts / capabilities
    -> Tooling generates instruction functor
    -> Tooling generates Functor.Map
    -> Tooling generates smart constructors
    -> Tooling generates algebra sums/injections when necessary
    -> developer writes semantic Work composition
```

This would leave handwritten code focused on decisions such as:

```text
which WorkParts exist
how they compose
what Space transformations/evolutions occur
what guarantees are required
```

rather than continuation plumbing.

This was one of the clearest possible resolutions to the boilerplate problem.

---

# 28. VSIR implications

The experiment suggests several possible future VSIR responsibilities, none finalized.

VSIR may eventually need to express:

## WorkPart vocabulary

Example conceptually:

```text
work-part read-todo
    input TodoId
    output Option<Todo>
```

## Feature/WorkFlow composition

```text
feature UpdateTodo
    read Todo
    evolve Todo
    write Todo
```

## Algebra ownership

```text
UpdateTodo.Algebra
    includes ReadTodo
    includes WriteTodo
```

## WorkProcess composition

```text
AttachFile
    uses Folders.AddFile
    uses Tickets.AddAttachmentReference
```

## Injection/hoist

This should probably be generated rather than authored explicitly unless the injection has semantic meaning beyond structural inclusion.

## Guarantees

Guarantees should remain separate from capability vocabulary.

None of this syntax was established by the experiment.

---

# 29. Relationship to category/Kleisli work

VSlices already has category/arrow machinery.

This created several useful cautions.

```text
A -> B
```

as an ordinary pure function is not the same categorical morphism as:

```text
A -> IO<B>
```

which belongs naturally in a Kleisli setting.

Likewise:

```text
Free<F, A>
```

is a unary monadic program representation, not directly the same thing as an existing binary arrow encoding:

```text
K<F, A, B>
```

The experiment should not collapse these merely because they all involve composition.

A future integration should decide carefully whether:

- Free replaces any current Work composition;
- Free lowers into existing Arrow/Kleisli abstractions;
- existing Arrow/Kleisli abstractions remain orthogonal;
- Feature is better modeled with another categorical representation.

This was not resolved.

---

# 30. Free versus Monad Transformers

LanguageExt's Monad Transformer work was also inspected.

The useful distinction was:

```text
Free
    composes instructions without selecting interpretation

MonadT
    composes effect structures used by an interpretation
```

These are different problems.

A future interpreter may target:

```text
IO
Eff
OptionT<IO>
EitherT<...>
another transformer stack
```

without changing the Free instruction language.

The experiment intentionally used `IO` as a concrete interpreter target rather than generalizing prematurely.

That choice was practical, not a claim that `IO` is the final semantic target.

---

# 31. What worked well

The strongest positive results were:

## 31.1 Inert Work representation

A Free program can be built without executing its external WorkParts.

This cleanly separates description from realization.

## 31.2 Multiple interpreters

The same program can be interpreted differently.

That supports:

- test Groundings;
- production Groundings;
- alternate infrastructure;
- potentially analysis/simulation interpreters.

## 31.3 Small capability vocabulary

PointReader/Writer/Remover avoid importing Repository assumptions.

## 31.4 Grounding separation

The algebra does not need to know EF, HTTP, dictionaries, brokers, or other realization mechanisms.

## 31.5 WorkFlow composition

Monadic bind naturally represents:

```text
do A
use result to decide B
use B result to decide C
```

which is a good fit for WorkFlow-like behavior.

## 31.6 Cross-WorkFlow algebra composition

Natural injections + hoist provide a mathematically clean way to embed independently authored WorkFlow programs into a larger vocabulary.

## 31.7 Interpreter delegation

A BFF/Product Grounding can delegate child algebra branches to service-owned interpreters rather than recreating them.

## 31.8 Ownership of injection

Using an external transformation witness avoids forcing a lower-level service algebra to know which higher-level BFF later composes it.

---

# 32. What did not work well

The strongest negative results were:

## 32.1 Parallel Programs layer

`TodoPrograms` duplicated Feature identity and stole the actual behavior.

This should not return in the same one-Program-per-Feature shape.

## 32.2 Boilerplate

Operation ADTs and `Functor.Map` are too mechanical to be a satisfying handwritten VSlices surface.

## 32.3 Hidden semantic operations

Pure C# continuations can contain significant Work semantics invisible to the Free AST.

This weakens claims that the Free tree fully represents Work.

## 32.4 Algebra proliferation

One precise algebra per Feature improves semantic precision but can create many types and interpreters.

## 32.5 Sum scaling

`AlgebraSum<A...G>` is workable machinery, not an elegant semantic model.

## 32.6 Flattened boundaries

Hoist can remove explicit WorkFlow boundaries that may later carry operational semantics.

## 32.7 Large architectural blast radius

Making Feature itself a Free program affects:

- Feature;
- ServiceFeature;
- ProductFeature;
- invocation adapters;
- Grounding;
- tests;
- examples;
- VSIR expectations;
- Tooling;
- documentation;
- host composition.

The surrounding model was not mature enough to justify all of that at once.

## 32.8 Guarantees remained separate and unsolved

Free composition does not solve tracking, atomicity, durability, retries, compensation, etc.

---

# 33. Possible future resolutions

If VSlices revisits the experiment, the following directions are worth testing.

## 33.1 Generate mechanical algebra code

Do not require application authors to handwrite:

- operation continuation records;
- `Functor.Map`;
- smart constructors;
- positional injection plumbing.

## 33.2 Define WorkPart precisely first

Before rebuilding Feature around Free, decide:

> Which operations must be explicit nodes of Work?

Candidates include:

- external effects only;
- every semantically named step;
- only capability requests;
- Space transformations/evolutions too;
- nested WorkFlows as explicit parts.

This is foundational.

## 33.3 Pressure-test with TicketSupport, not only Todo CRUD

The strongest next real case is attachment composition:

```text
Folders.AddFile
Tickets.AddAttachmentReference
```

because it exercises:

- two independently owned WorkFlows;
- BFF composition;
- cross-boundary failure;
- potential compensation;
- separate interpreters;
- ownership-preserving algebra injection.

## 33.4 Compare flattening versus explicit WorkFlow invocation

Implement both small variants:

```text
A. hoist into Process algebra
B. explicit RunWorkFlow higher-order operation
```

Then compare:

- code size;
- semantic visibility;
- guarantees;
- tracing;
- retry;
- authorization;
- compensation;
- Tooling complexity.

## 33.5 Preserve capabilities/guarantees separation

Even if the Free representation changes completely, do not collapse guarantee names into concrete infrastructure mechanisms.

## 33.6 Let Tooling own algebra composition mechanics

If a Process says:

```text
uses Feature A
uses Feature B
uses Feature C
```

Tooling can generate the sum/injections/interpreter adapter rather than requiring positional generic code.

## 33.7 Avoid premature generalization of interpreter target

Continue with the minimum effect target proven by the real case.

Generalize only when another target is actually required.

---

# 34. Open questions

The experiment should be considered incomplete until at least these questions are answered.

1. What exactly is a WorkPart?
2. Is every semantically meaningful Work step required to appear explicitly in the Free algebra?
3. Is Feature strictly equivalent to WorkFlow, or is Feature a more general executable surface?
4. Does WorkProcess deserve its own executable abstraction?
5. Should Process composition flatten WorkFlows through hoist?
6. When must a child WorkFlow remain an explicit higher-order node?
7. How are WorkFlow-level guarantees/scopes represented?
8. How are retry and compensation represented across composed service WorkFlows?
9. How should authorization interact with child WorkFlows?
10. How should cancellation/timeouts propagate?
11. How should concurrent/parallel WorkParts be represented?
12. How should VSIR express WorkParts and WorkFlow composition?
13. Which algebra code is generated?
14. Which algebra code should remain semantic and handwritten?
15. How are named Process algebras represented without positional generic explosion?
16. How are query operations represented?
17. Does PointWriter mean immediate realization, staged intent, or neither universally?
18. What evidence is required before splitting Create and Update capabilities?
19. How should optimistic concurrency/conflicts appear?
20. What remains useful from Free if Feature is ultimately represented differently?

---

# 35. Evidence and historical breadcrumbs

## Framework

Repository:

```text
vslices/framework
```

Relevant current/past artifacts include:

```text
docs/point-algebras.md
docs/feature-model.md
docs/notes/capabilities-and-guarantees.md

src/VSlices.Work/Algebra/
tests/VSlices.Work.Algebra.Tests/

examples/SampleWorkflow/
```

Historical experimental branch:

```text
experiment/simple-workflow-point-crud
```

The branch should be treated as historical executable evidence even if the mainline code is reverted.

Useful commits from the exploration include:

```text
ceb94646b9c50e2c064e13910fd7ae9591654e08
    Adopt free point algebras for Work capabilities

2620c2cf43e10bc57a7a5e75c08680b5b426e462
    Model simple workflow spaces semantically

4f3c2c191300db5edacf2527f6ed0835575ea160
    Import semantic error type in Todo update

434893337eb8f9be32297c239167c425ce77848a
    Record staged adoption boundary from Ticket Support pressure
```

The exact commit graph may continue evolving; these SHAs are breadcrumbs, not normative architecture.

## Real product evidence

Repository:

```text
atom-dev-serviu/access-management-product
```

Important TicketSupport evidence:

```text
products/ticket-support-product/TicketSupport.BFF/Features/Tickets/AttachFile.cs
products/ticket-support-product/TicketSupport.BFF/Features/Tickets/RemoveAttachment.cs
products/ticket-support-product/TicketSupport.BFF/Features/Tickets/DownloadAttachment.cs
products/ticket-support-product/docs/final-integration-state.md
services/ticket-service/docs/capability-ownership.md
services/ticket-service/docs/concept-classification.md
```

Important older Domain-style evidence used to correct the Todo Spaces:

```text
services/identity-service/Identities.Domain
services/account-service/Accounts.Domain
```

Those examples used older VSlices vocabulary but were useful evidence for:

- semantic identity construction;
- aggregate/domain value construction;
- identity-owned equality;
- explicit state/evolution.

---

# 36. External technical references inspected

The experiment was informed by Paul Louth / LanguageExt material, especially:

```text
Higher Kinds in C# with language-ext
Free Monad examples
Monad Transformer material
```

The modern LanguageExt repository was inspected directly and confirmed the existence of:

```text
Free<F, A>
Free.lift
Free.pure
Free.hoist
Natural<F, G>
```

A relevant LanguageExt sample represented instructions as an ADT with continuation functions and interpreted the resulting Free program separately.

Important lesson:

> We do not need to implement a Free Monad from scratch if this direction returns. The interesting VSlices work is defining the semantic vocabulary, ownership, composition, interpretation boundaries, and Tooling representation.

---

# 37. Rollback interpretation

Reverting the current Free-oriented implementation should not be interpreted as:

```text
Free was disproven
```

The more accurate interpretation is:

```text
the experiment produced useful evidence
    ->
the architecture around that evidence is not settled enough
    ->
preserve findings
    ->
reduce current implementation commitment
    ->
return when real Work pressure justifies another iteration
```

This follows the normal VSlices discovery cycle:

```text
real case
    -> observable limit
    -> identify ownership
    -> discover minimum mechanism
    -> implement
    -> observe next limit
```

The Free experiment successfully reached several next limits.

That is enough to make it valuable even if its implementation is removed.

---

# 38. Provisional conclusion

The strongest idea worth retaining is:

```text
Free Monad
    may be a useful mechanism for composing Work instructions
    while separating semantic Work from Grounding
```

The strongest structural hypothesis worth retaining is:

```text
WorkFlow
    is composed from WorkParts

WorkProcess
    coordinates WorkFlows

algebra composition + hoist
    may provide one implementation strategy
```

The strongest warning worth retaining is:

> A mathematically elegant Free representation is not automatically the correct VSlices semantic representation.

Before adopting it again, VSlices should establish:

- what WorkPart means;
- which boundaries must remain observable;
- what Feature means relative to WorkFlow and WorkProcess;
- which mechanics Tooling should generate;
- how guarantees interact with Work composition;
- whether the real TicketSupport/BFF cases remain simpler and more faithful under the proposed representation.

Until then, Free should remain a preserved research direction rather than an architectural requirement.
