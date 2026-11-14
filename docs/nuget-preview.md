## Coordinated VSlices prerelease

The eleven packable libraries under `src/` form the coordinated `0.3.0-preview.1` prerelease bundle. `VSlices.Spaces.Analyzers` remains source-only and is excluded from NuGet distribution pending controlled validation.

The eleven packages are:

- `VSlices`
- `VSlices.Spaces`
- `VSlices.Work`
- `VSlices.Work.DataAccess`
- `VSlices.Grounding`
- `VSlices.Grounding.DataAccess.EFCore`
- `VSlices.Extensions.EFCore`
- `VSlices.Views.Abstract`
- `VSlices.Spaces.Authentication`
- `VSlices.Work.Authentication`
- `VSlices.Grounding.Authentication.AspNetCore`

`VSlices.Extensions.EFCore` replaces the former `VSlices.EntityFrameworkCore.Functional` package ID and is built from source inside this repository. Existing releases under the old ID are not modified or republished.

The `Verify VSlices NuGet preview bundle` workflow packs and checks all eleven; it **never publishes** packages. The preview verifier derives its expected version from the VSlices MSBuild project and its shared `src/Directory.Build.props`. The release publisher derives its version independently from the GitHub Release tag. The `Publish VSlices NuGet` workflow is triggered only by a published GitHub Release, validates the version from the tag, requires the tagged commit to be on `master`, runs tests, packages, verifies manifests and publishes via OIDC in the `nuget-release` environment.

For `v0.3.0-preview.1`, publish a GitHub prerelease using the tag on the reviewed `master` commit. Creating a tag alone is insufficient to trigger the publisher.

The package verifier confirms each expected package ID, common version, README/icon, excluded experimental analyzers, and package-specific author metadata. The imported functional EF Core package preserves its own original authors in NuGet; its provenance is documented in `docs/functional-ef-core.md`.

`LanguageExt.Core` remains a dependency of the core VSlices project. No NuGet credentials should be stored in this repository.

## Authentication additions (2026-11-14)

Authentication spaces, a Work reader algebra, and an ASP.NET Core recognition grounding originate in `atom-dev-serviu/auth-dependency` (experiment and consumer tests). The assembly, package, and public namespace are `VSlices.Grounding.Authentication.AspNetCore`. This namespace change requires consumer `using` directives to migrate from `VSlices.Grounding.Authentication.HttpCookie`, but does not redesign reader selection or behavior. A future review will split authentication method (cookie/JWT/API key) from recognition channel (`HttpContext`/Blazor `AuthenticationStateProvider`), with hybrid recognition restricted to hosts requiring it. See the corresponding tracking issue. Serviu-specific identity interpretation, authorization and hosting stay in Serviu. This integration is not evidence of production IIS verification. NuGet publication requires a release on reviewed `master` and the normal workflow.

## Data access package organization (2026-11-14)

`VSlices.Work.DataAccess` distributes point reading, writing, and removal contracts while keeping their namespace `VSlices.Work.Algebra`. `VSlices.Grounding.DataAccess.EFCore` realizes those contracts using Entity Framework Core. `VSlices.Extensions.EFCore` contains effectful EF Core extensions independently of Grounding. The old EF Core package IDs are not published again from this source layout; existing releases remain immutable.
