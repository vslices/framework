# VSlices — experimental .NET 10 preview

VSlices is a modular C# framework exploring semantic Spaces, executable Work, concrete Grounding and presentation. Current **0.x prereleases** are experimental; APIs may change between previews.

Source and issue tracker: https://github.com/vslices/framework

The seven public packages share coordinated versions. The `v0.3.0-preview.1` release has been published; see https://github.com/vslices/framework/releases. Consumers can reference only the modules they need, with transitive package dependencies resolved by NuGet.

The `VSlices` root package is part of this seven-package family. `VSlices.Grounding.EntityFrameworkCore.Functional` supplies additional effectful EF Core operations; its history and attribution are documented in the repository.

The experimental `VSlices.Spaces.Analyzers` project is **not** distributed as a NuGet compiler analyzer. Consumer-invoked modeling Laws are available; see `docs/spaces/modeling.md`, `docs/work/feature-modeling.md`, and `docs/transform-adapter.md`.

NativeAOT compatibility for the reflection-backed TransformAdapter is not established.

Licensed under MIT.
