# LanguageExt — semantic usage report

**Repository:** `vslices/framework`  
**Method:** MSBuildWorkspace + Roslyn `SemanticModel`, with projects loaded from current checkout, upstream `language-ext/` excluded.  
**Status:** Verified successful GitHub Actions run `LanguageExt semantic audit`, after restoring the standalone NativeAOT probe.  
**Production changes:** None.

## Verified results

| Metric | Value |
|---|---:|
| First-party .NET projects scanned | 32 |
| Projects loaded | 32 |
| Source documents analyzed across projects | 295 |
| Compiler-attributed LanguageExt occurrences | **2,231** |
| Origin assemblies | **LanguageExt.Core** (all 2,231) |
| Compilation diagnostics classified as errors | 0 |
| Project load failures | 0 |

The two remaining nonfatal workspace warnings refer to duplicate `AnalyzerReleases.*.md` AdditionalFiles in the analyzer project. No upstream LanguageExt project is counted as a VSlices consumer.

This is a **per-project reference-location census**, not a count of distinct library methods and not a count of distinct source files. One C# statement can produce several bound references (e.g., `Fin<Error, T>` types and a `Match` method). Documents may appear in multiple project compilations.

## Highest-volume VSlices projects (reference locations)

| Project | Locations |
|---|---:|
| `VSlices` | 1,051 |
| `SampleWorkflow.Work` | 226 |
| `VSlices.Work.Algebra.Tests` | 217 |
| `SampleWorkflow.Api` | 68 |
| `VSlices.Spaces` | 62 |
| `SampleBFF.Tests` | 58 |
| `SampleWorkflow.Grounding` | 50 |
| `SampleFileRepo` | 46 |
| `VSlices.Spaces.Tests` | 43 |

The first-party library `VSlices` is the dominant migration pressure. Examples/tests are important compatibility evidence but not independent production dependency ownership.

## High-frequency resolved definitions

| Definition / method | Locations |
|---|---:|
| `LanguageExt.Traits.K<F,A>` | 191 |
| `LanguageExt.Traits.K<F,A,B>` | 154 |
| `RunAsync` | 137 |
| `Map<B>` | 122 |
| `Match<B>` | 118 |
| `Bind<B>` | 95 |
| `LanguageExt.Fin<A>` | 78 |
| `pure<A>` | 75 |
| `LanguageExt.Traits.Const<TYPE>` | 64 |
| `LanguageExt.IO` | 62 |
| `LanguageExt.Common.Error` | 52 |
| `LanguageExt.Eff<A>` | 52 |
| `LanguageExt.Option<A>` | 39 |

Definitions above are compiler-bound to `LanguageExt.Core`. Counts are occurrences, **not** calls to a single implementation variant guaranteed to remain unchanged under migration.

## Primary evidence

- [`symbols.csv`](./semantic/symbols.csv) — `project,file,line,column,kind,symbol,definition,assembly,expression`.
- [`summary.json`](./semantic/summary.json) — compilation coverage, project totals, assembly counts, top definitions, diagnostics.
- [`semantic analyzer`](../../../../tools/languageext-audit/semantic/Program.cs) — reproducible Roslyn implementation.
- [`semantic CI`](../../../../.github/workflows/languageext-semantic-audit.yml) — build/restore context.
- [`all-locations.csv`](./all-locations.csv) — earlier **textual candidate** inventory, useful for comparing symbol-bound hits to unbound text.
- [`MIGRATION-PLAN.md`](./MIGRATION-PLAN.md) — proposed capability boundaries and attribution controls.

## Design implications

1. **Core typeclass/HKT pressure first:** the two `K` shapes and their operations dominate the library's own usages. Do not blindly transplant all of LanguageExt's higher-kinded typeclass interfaces.
2. **Effects and flow semantics:** `IO`, `Eff`, `RunAsync`, `Bind`, `Map` are deeply coupled to `Flow` and Work. Preserve execution ordering, cancellation, failure propagation, resource cleanup, lazy vs eager evaluation.
3. **Error/result semantics:** `Fin`, `FinT`, `Error`, `Either`, `Option` require explicit preservation tests and source-provenance choices before replacement.
4. **Source ownership:** represent only mechanisms the Framework uses. `VSlices.csproj` can initially remain a single assembly with internal folders; split projects only if independently owned boundaries emerge.
5. **License:** any copied or modified third-party implementation must retain applicable MIT notices and original copyright information, with upstream revision and path. Independent implementations of concepts should be labeled as such, rather than falsely attributed as copied code.

## Coverage limits and next checks

Roslyn attribution enumerates the explicit and inferred bound symbols it visits, including resolved extension methods, operators and user-defined conversions; it **does not prove that every runtime dependency has been statically exposed**. Reflection, dynamically constructed calls, binary-only assemblies, preprocessor-excluded code, and generated source outside the compilation documents need separate treatment. The CSV is pinned to the branch checkout that generated it; file/line mappings change as source evolves.

**Suggested next checkpoint before removing packages:** derive the public API surface that exposes LanguageExt types, then create behavioral/law-oriented tests for the minimal candidate replacement substrate. Do not remove `LanguageExt.Core` based on lexical matches alone.
