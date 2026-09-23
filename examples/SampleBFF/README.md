# SampleBFF — cross-service WorkProcess experiment

## Purpose

This sample pressures the Free WorkFlow / WorkProcess model across two independently owned service surfaces.

It deliberately recreates the attachment geometry observed in Ticket Support without requiring a migration of the Serviu consumer repository.

The scenario is:

```text
SampleBFF.AttachFileToTodo
    -> SampleFileRepo.AddFile
    -> SampleWorkflow.AddAttachmentReference
```

The important property is that the two child WorkFlows are not interpreted by the same Grounding.

## Ownership

```text
SampleFileRepo
    owns SampleFile identity, content, persistence vocabulary and realization

SampleWorkflow
    owns Todo semantics and the association to an opaque ResourceReference

SampleBFF
    owns the product-level composition
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

## Process algebra

`AttachFileToTodo` composes:

```text
AddFile.Algebra
    +
AddAttachmentReference.Algebra
```

through:

```text
AlgebraSum<AddFile.Algebra, AddAttachmentReference.Algebra>
```

Each child program is hoisted using the existing external natural-transformation witnesses:

```text
InjectLeft
InjectRight
```

No Framework change was required for cross-service composition.

## Grounding

The composed Process interpreter is constructed from two different realizations:

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

## Executable evidence

The cross-service test proves that:

1. a Todo can be created through its own WorkFlow and Grounding;
2. a file can be stored through an independent WorkFlow and Grounding;
3. the BFF can hoist and compose both child programs;
4. the resulting Todo persists the opaque resource association;
5. the file remains persisted in the file service;
6. both sides can be re-read independently after the Process completes.

GitHub Actions:

```text
Branch: experiment/cross-service-workprocess
Head:   ea9abe076fb4af42d47d1f759e61feb3c5b8e300
Run:    35857218289
Result: success
```

The previous SampleWorkflow CRUD smoke test remains green in the same run.

## First guarantee pressure

The sample also intentionally captures the current partial-failure behavior.

If:

```text
SampleFileRepo.AddFile succeeds
SampleWorkflow.AddAttachmentReference cannot find the Todo
```

then:

```text
the Process returns no association
the file remains stored
```

The test records this behavior instead of hiding it.

This is evidence that:

```text
WorkFlow composition
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
- arbitrary-N algebra composition;
- the final role of `Flow`;
- whether partial failure should become a first-class WorkPart, WorkProcess policy, guarantee, or another concept.

Those questions should be pressured independently rather than folded into `AlgebraSum`.
