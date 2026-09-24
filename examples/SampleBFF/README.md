# SampleBFF — cross-service Feature composition experiment

## Purpose

This sample pressures the Free WorkFlow model across two independently owned service surfaces.

It deliberately recreates the attachment geometry observed in Ticket Support without requiring an immediate migration of the Serviu consumer repository.

The scenario is:

```text
SampleBFF.AttachFileToTodo
    -> SampleFileRepo.AddFile
    -> SampleWorkflow.AddAttachmentReference
```

`AttachFileToTodo` is not a special WorkProcess type. It is an ordinary Feature whose `ALG` is the sum of the two child Feature algebras.

## Ownership

```text
SampleFileRepo
    owns SampleFile identity, content, persistence vocabulary and realization

SampleWorkflow
    owns Todo semantics and the association to an opaque ResourceReference

SampleBFF
    owns the composed Feature
    owns SampleFileId -> ResourceReference interpretation for this integration
    owns ordering between the child WorkFlows

VSlices.Work
    owns the generic algebra composition and hoisting mechanisms
```

The Todo surface does not depend on `SampleFileRepo`.

It stores only:

```text
ResourceReference
```

The BFF is the only surface that knows that, in this product context:

```text
SampleFileId
    <=> ResourceReference
```

## Composed Feature algebra

`AttachFileToTodo` is:

```text
Feature<
    AlgebraSum<
        AddFile.Algebra,
        TodoAlgebra>,
    Request,
    Response>
```

The implementation aliases the composed vocabulary locally as `Algebra` and embeds each child WorkFlow with:

```csharp
Algebra.FromA(AddFile.Describe(...))
Algebra.FromB(AddAttachmentReference.Describe(...))
```

This keeps the call site independent of the internal natural-transformation details.

The underlying witnesses remain:

```text
InjectA<A, B>
InjectB<A, B>
```

and the mathematical operation remains Free hoisting.

## Grounding

The composed interpreter is constructed from two different realizations:

```csharp
new AlgebraSumIO<
    AddFile.Algebra,
    AddAttachmentReference.Algebra>(
        fileWork,
        todoWork);
```

where:

```text
fileWork : AlgebraIO<AddFile.Algebra>
todoWork : AlgebraIO<AddAttachmentReference.Algebra>
```

This is stronger evidence than the earlier `CreateAndGetTodo` miniature, where both child algebras were interpreted by the same `InMemoryTodoWork`.

## AlgebraSum arities

The generic mechanism is available through seven positional children:

```text
AlgebraSum<A, B>
AlgebraSum<A, B, C>
AlgebraSum<A, B, C, D>
AlgebraSum<A, B, C, D, E>
AlgebraSum<A, B, C, D, E, F>
AlgebraSum<A, B, C, D, E, F, G>
```

Each arity supplies matching `FromA` ... `FromG`, `InjectA` ... `InjectG`, and `AlgebraSumIO<...>` support.

The A..G positions are composition mechanics; they do not encode semantic ordering or authority.

## Executable evidence

The cross-service test proves that:

1. a Todo can be created through its own WorkFlow and Grounding;
2. a file can be stored through an independent WorkFlow and Grounding;
3. the BFF Feature can hoist and compose both child programs;
4. the resulting Todo persists the opaque resource association;
5. the file remains persisted in the file service;
6. both sides can be re-read independently after the composed Feature completes.

The earlier successful evidence was:

```text
Branch: experiment/cross-service-workprocess
Head:   ea9abe076fb4af42d47d1f759e61feb3c5b8e300
Run:    35857218289
Result: success
```

Later commits simplify the model by removing the redundant `WorkProcess` interface and extend `AlgebraSum` through arity seven. After promotion, `master` CI is the authoritative evidence for the maintained state.

## First guarantee pressure

The sample intentionally captures the current partial-failure behavior.

If:

```text
SampleFileRepo.AddFile succeeds
SampleWorkflow.AddAttachmentReference cannot find the Todo
```

then:

```text
the Feature returns no association
the file remains stored
```

The test records this behavior instead of hiding it.

This is evidence that:

```text
Feature composition
!= atomic composition
```

and:

```text
AlgebraSum + hoist
do not imply rollback, compensation or reconciliation
```

That is desirable: the composition mechanism remains honest about what it guarantees.

## Still open

This sample does not settle:

- compensation;
- retry;
- reconciliation;
- atomicity;
- durability;
- explicit guarantee vocabulary;
- the final role of `Flow`;
- whether partial failure should become a first-class WorkPart, Feature policy, guarantee, or another concept;
- whether composition beyond A..G deserves a different representation.

Those questions should be pressured independently rather than folded into `AlgebraSum`.
