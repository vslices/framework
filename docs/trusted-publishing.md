# Trusted Publishing for VSlices releases

Restored the historical GitHub Release workflow from vslices/framework@cf3aba0
(.github/workflows/master-deploy.yml). Its sequence remains restore, build,
test, pack, push; NuGet/login@v1 uses short-lived GitHub OIDC rather than
the previous long-lived NUGET_AUTH_TOKEN secret.

## Configure NuGet.org trusted publisher

- GitHub owner: vslices
- GitHub repository: framework
- Workflow filename: master-deploy.yml
- GitHub environment: nuget-release

Create the policy from the NuGet.org owner account. Confirm the owner controls
all ten package IDs (in particular VSlices). OIDC cannot claim third-party IDs.
Set repository Actions variable NUGET_USERNAME to that NuGet.org username.
Configure a GitHub environment named nuget-release, ideally with reviewers.

## Release behavior

Publish a GitHub Release tagged 0.1.0-preview.1 (or v0.1.0-preview.1)
on a commit reachable from master. Build and verification run first;
the publish job authenticates with OIDC and uploads all ten packages.
Only those two tags are accepted by this initial-preview workflow.
Packages are immutable per ID/version. --skip-duplicate does not replace
existing packages; verify IDs and ownership before first release.
nuget-preview-bundle.yml remains available for checks without publication.
The experimental analyzer is deliberately not distributed.
