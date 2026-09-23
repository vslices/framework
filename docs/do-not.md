# Do Not

Do not introduce new Feature execution paths based on:

```text
FeatureEff<RT, A>
Flow<RT, REQ, RES>
Feature<F, RT, REQ, RES>
HasAlgebra<ALG, RT>
```

Those are historical execution surfaces.

The current executable Feature contract is:

```text
Feature<F, ALG, REQ, RES>
    -> Free<ALG, RES>
```

Grounding is supplied independently through `AlgebraIO<ALG>` when the inert WorkFlow is interpreted.

Do not restore an ambient runtime carrier merely to recover dependency injection or capability lookup. Add only the Work vocabulary that the Feature actually requires.
