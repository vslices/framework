# Semantic LanguageExt reference audit

This tool uses Roslyn's **semantic models** and the **actual MSBuild projects** to attribute a reference to the assembly that defines the referenced symbol. It is complementary to, not a replacement for, `../inventory.py` and the declared-package/global-import graph.

## Run

```sh
dotnet restore VSlices.slnx
dotnet run --project tools/languageext-audit/semantic/LanguageExt.SemanticAudit.csproj -- --root . --out docs/audits/languageext-2026-10/semantic
```

The GitHub audit workflow runs and publishes the reports.

**Attribution rule:** only resolved symbols with a containing assembly whose name begins `LanguageExt` are classified as LanguageExt references. Static/extension calls, generic type usages, operator overloads, implicit conversions, LINQ query bindings, and LanguageExt `using static` imports are considered. A third-party type passed to a VSlices-defined method does not falsely attribute that *method* to LanguageExt.

Reports include **project**, **path**, **line/column**, **symbol**, **originating assembly**, **original definition**, and **kind**. Results are deduplicated by use location and symbol within a project.

**Limits:** a compiler model does not attribute code excluded by conditional compilation, unresolved references, reflection, runtime dynamic dispatch, source text inside comments, or opaque binaries. Generated source and Razor must be inspected separately if not part of the Roslyn project documents. Project-loading failures and compilation errors are **reported, not silently treated as a clean scan**. The source tree is analyzed at the checkout SHA given in the metadata, not at an imagined fixed revision.

This tool **does not** copy LanguageExt code, implement replacements, or change the production dependency. License compliance is evaluated separately by source provenance.
