# Current Work boundaries

- Define new Features with `static Eff<ALG, RES> Get(REQ request)` and nominal nested Request/Response contracts. See [Feature Model](feature-model.md) and [Feature Modeling Laws](work/feature-modeling.md).
- Do not introduce new `FeatureEff<RT, A>` usages or revive `Flow<RT, REQ, RES> Get()` as the Feature contract.
- `Flow` remains an independent core abstraction for other uses.
- Do not infer repository, transaction, durability or authorization guarantees from capability atoms.
- Preserve the difference between optional `PointReader.ReadOrDefault` and required `PointReader.Read`; do not manufacture a new catchable error classification without real consumer pressure.
