## Coordinated VSlices prerelease

The seven packable libraries under `src/` form the coordinated `0.3.0-preview.1` prerelease bundle. `VSlices.Spaces.Analyzers` remains source-only and is excluded from NuGet distribution pending controlled validation.

The seven packages are:

- `VSlices`
- `VSlices.Spaces`
- `VSlices.Work`
- `VSlices.Grounding`
- `VSlices.Grounding.EntityFrameworkCore`
- `VSlices.Grounding.EntityFrameworkCore.Functional`
- `VSlices.Views.Abstract`

`VSlices.Grounding.EntityFrameworkCore.Functional` replaces the former `VSlices.EntityFrameworkCore.Functional` package ID and is built from source inside this repository. Existing releases under the old ID are not modified or republished.

The `Verify VSlices NuGet preview bundle` workflow packs and checks all seven; it **never publishes** packages. The `Publish VSlices NuGet` workflow is triggered only by a published GitHub Release, validates the version from the tag, requires the tagged commit to be on `master`, runs tests, packages, verifies manifests and publishes via OIDC in the `nuget-release` environment.

For `v0.3.0-preview.1`, publish a GitHub prerelease using the tag on the reviewed `master` commit. Creating a tag alone is insufficient to trigger the publisher.

The package verifier confirms each expected package ID, common version, README/icon, excluded experimental analyzers, and package-specific author metadata. The imported functional EF Core package preserves its own original authors in NuGet; its provenance is documented in `docs/functional-ef-core.md`.

`LanguageExt.Core` remains a dependency of the core VSlices project. No NuGet credentials should be stored in this repository.
