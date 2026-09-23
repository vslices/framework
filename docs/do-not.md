# Do not

Do not introduce new usages of retired execution or domain-pattern abstractions merely because they existed in previous VSlices iterations.

In particular:

- do not use `Flow<RT, REQ, RES>`; Feature owns `Free<ALG, RES>` directly;
- do not reintroduce `FeatureEff<RT, A>`;
- do not reintroduce `Repository` or `DatabaseIO` as Work semantics;
- do not reintroduce `HasClock<RT>` or `ClockEnv<RT>`;
- do not model semantic values as `Entity` or `AggregateRoot` merely because they have identity or coordinate state.

Prefer the smallest currently justified concepts:

```text
Space
Feature / Free WorkFlow
Feature-owned Algebra
Grounding
explicit capabilities
explicit guarantees when required
```

Historical abstractions remain evidence about solved problems, not mandatory names or ownership boundaries.
