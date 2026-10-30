# LanguageExt removal — migration boundary and attribution plan

**Source snapshot:** `vslices/framework@842523aeac8d67c5ad1f4471d6f02fe142ef2afe` (`master` after PR #177).
**Evidence:** [generated lexical inventory](./README.md), [line-level CSV](./all-locations.csv), [packages](./packages.csv), [global imports](./msbuild-imports.csv), [project references](./project-references.csv).
**Status:** discovery only. No production dependencies or APIs changed.

## Facts established from the repository

- `src/VSlices/VSlices.csproj:10` declares `LanguageExt.Core` **5.0.0-beta-77**.
- `src/VSlices/VSlices.csproj:14-21` imports `LanguageExt`, `Common`, `Traits`, `Effects`, `Pretty`, `Traits.Domain`, and static `Prelude`.
- `src/VSlices.Spaces/VSlices.Spaces.csproj:18-22`, `src/VSlices.Work/VSlices.Work.csproj:8-12` and `src/VSlices.Views.Abstract/VSlices.Views.Abstract.csproj:22-23` introduce further global imports.
- An unqualified symbol in source can still be LanguageExt because of these MSBuild global imports or a transitive project dependency.
- The audit is a **lexical candidate inventory**, not a full compiler-bound symbol map. A `Map` call may be LanguageExt, VSlices, or unrelated; `State`, `Unit`, `ArrowApply`, and `Kleisli` also have local definitions.

## First decomposition of required capabilities (not the proposed implementation)

| Concern | Representative evidence | Decision to take before implementation |
| --- | --- | --- |
| HKT and traits | `K`, `Monad`, `Functor`, `Applicative`, `Fallible`, `MonadIO` in `src/VSlices/Monads/` | Which laws and type representations are indispensable? |
| Result/error semantics | `Fin`, `FinT`, `Error`, `Either` | Error algebra, aggregation, exception bridging and transformer semantics |
| Effect execution | `IO`, `Eff`, `RunAsync`, `RunIO`, `Bind` | Cancellation, resource safety, async sequencing, runtimes and evaluation |
| Optionality | `Option`, `OptionT`, `Some`, `None` | Null/absence and short-circuit contracts |
| Composition | `Map`, `Bind`, `Match`, `SelectMany`, static Prelude | Distinguish language LINQ, VSlices extensions and LanguageExt extensions |
| Small primitives | `Unit`, `Seq`, `Pure`, `Readable`, `SemigroupK` | Determine true consumers; avoid copying broad unused functionality |
| Integration | `VSlices.Spaces`, `VSlices.Work`, `VSlices.Views.Abstract`, examples, probes, tests | Decide whether public APIs may break, and what tests evidence preservation |

## Recommended order of implementation

1. **Compiler binding pass:** Run a Roslyn compilation of each .NET project with current packages. Emit `file,line,column,symbol,containing-assembly,namespace,type/method,project` for each bound reference to `LanguageExt.Core`. Record unresolved code and dynamic calls separately. Do not replace candidate results with speculative attribution.
2. **Public surface map:** distinguish LanguageExt types exposed in public APIs from implementation-only dependencies. This determines consumer migration cost and compatibility boundaries.
3. **Behavioral contracts:** write targeted tests for success/failure accumulation, laziness, effect execution order, cancellation, resource disposal, exception behavior, LINQ/query syntax and typeclass laws.
4. **Minimal first-party core:** migrate only the capabilities required by those contracts. Keep `VSlices` module boundaries grounded in observed responsibilities instead of mirroring LanguageExt's namespace tree automatically.
5. **Source provenance & compliance:** for copied/modified code, preserve upstream copyright notices, MIT license text, source URL, upstream revision and per-file origin. For independently reimplemented concepts, identify inspiration accurately without implying a code fork where none occurred.
6. **Removal test:** remove `LanguageExt.Core` from `VSlices.csproj`, remove global imports, build all projects, run unit tests, smoke tests and NativeAOT checks, and verify the dependency graph no longer resolves LanguageExt transitively.

## Proposed organization (candidate, not a commitment)

- **VSlices functional substrate:** HKT representation, laws and minimal traits required by `Flow` and `FinT`.
- **VSlices results:** `Fin`, `Error`, optionality, conversions, and transformers actually used.
- **VSlices effects:** `IO`/`Eff` behavior, resource management and runtime integration.
- **VSlices composition surface:** `Prelude` and syntactic bridges only for proven consumers.
- **VSlices adapters:** gradual migration/compatibility bridges; do not inadvertently give adapters semantic authority over Spaces or Work.

These can initially remain folders in `VSlices.csproj`. New assemblies require evidence of independent versioning or dependency boundaries.

## License and attribution

LanguageExt's upstream GitHub project advertises the **MIT license**. The current repository's `LICENSE` applies to VSlices itself and **does not automatically substitute for third-party copyright notices**. Before distributing source-derived code, verify the exact upstream license text and notices at the chosen revision and ship the required notices with any copies/substantial portions. Track upstream provenance individually: `origin URL + revision + original file + transformation + retained notices`.

**A concept, API shape or generic algorithm and copied source code are not the same thing.** Keep this distinction visible in documentation and licensing. This is an engineering compliance checklist, not a legal determination.

## Inventories

- `all-locations.csv` is the requested path/line/column index. Its candidate symbols are **not** yet all compiler-proven LanguageExt references.
- `files-scanned.csv` is the coverage list; identify excluded binary/generated files deliberately.
- `packages.csv` and `project-references.csv` help trace the declared package and project graph.
- `README.md` summarizes occurrences and points to individual files.

## Reproduction

```bash
python tools/languageext-audit/inventory.py
```

The GitHub Action `.github/workflows/languageext-usage-audit.yml` runs the same script and commits its artifacts on the audit branch. The report is pinned to the initial source snapshot, even though the generation workflow itself is on a later audit-branch commit.
