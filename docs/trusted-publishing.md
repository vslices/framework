# VSlices Trusted Publishing (GitHub Actions → NuGet.org)

VSlices publishes the **ten** .NET 10 libraries as version `0.1.0-preview.1`. The experimental analyzer is not distributed. Publishing uses **NuGet/login@v1** and an ephemeral GitHub Actions OIDC credential instead of a stored `NUGET_API_KEY`.

## Required policy on NuGet.org (manual account action)

Sign in to the **NuGet.org account or organization that will own the ten IDs**. From the account menu select **Trusted Publishing** and add a GitHub Actions policy with exactly:

| Field | Value |
| --- | --- |
| Repository owner | `vslices` |
| Repository | `framework` |
| Workflow file | `publish-nuget.yml` (filename only) |
| Environment | `nuget-release` |

Choose the correct **policy owner** and scopes for all ten VSlices packages (or scope to `VSlices*` when that policy scope is supported and appropriate). An existing package ID owned by a different NuGet account **cannot** be claimed merely by OIDC. The root package ID remains **VSlices**, not `VSlices.Core`; verify ownership/availability with NuGet.org before running the publish workflow.

In GitHub **Settings → Environments**, configure `nuget-release`. A reviewer approval gate is recommended if your GitHub plan/repository supports it. The policy must use exactly the same environment name; no API key secret is required.

## Publish

1. Review and merge the packaging configuration to `master`.
2. Confirm NuGet.org policy ownership and all ten IDs.
3. Open **Actions → Publish VSlices NuGet (Trusted Publishing) → Run workflow**, choose `master`.
4. Enter your **NuGet.org username** (not email) and the confirmation `PUBLISH_VSLICES_0_1_0_PREVIEW_1`.
5. The `pack` job produces and verifies the bundle; `publish` requires the `nuget-release` environment, exchanges GitHub OIDC for a short-lived NuGet key and uploads the ten packages.

The workflow does **not** run on push or pull requests. NuGet packages are immutable by `ID + version`; partial publishes can require recovery. Do **not** re-use the same version to change package contents. After a partial failure, examine NuGet.org state and rerun only when the missing package IDs can be published safely.

To prepare the bundle without publication, run **Verify VSlices NuGet preview bundle**. Do not store legacy long-lived NuGet keys in GitHub or the repository; rotate/revoke a previous compromised key via the NuGet.org API Keys page.
