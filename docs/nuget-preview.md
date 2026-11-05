## NuGet preview bundle (candidate)

The ten packable libraries under `src/` are prepared as a coordinated `0.1.0-preview.1` bundle. `VSlices.Spaces.Analyzers` is deliberately excluded from distribution pending controlled validation; it remains available in source builds. `LanguageExt.Core` is pinned at `5.0.0-beta-78`.

Run `dotnet pack src/VSlices/VSlices.csproj -c Release` to pack a single library, or run the `Verify VSlices NuGet preview bundle` GitHub Actions workflow to pack and inspect all ten. That workflow **never pushes packages**.

The root package ID is provisionally `VSlices`. Prior to publication, confirm that the ID is available or controlled by the intended NuGet.org owner. If not, select `VSlices.Core` and adjust the root project `PackageId` and all dependent nuspec/verification expectations **together**. An existing `VSlices.Core` family is known, so do not assume this fallback is available either. No NuGet API key should be stored in the repository or shared in chat.

Refer to `src/README.nuget.md` for the actual package readme. Source remains authoritative for semantic/API behavior.
