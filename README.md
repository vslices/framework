<p align="center">
  <img src="icons/icon hq.png" alt="VSlices icon" width="192px">
</p>

# VSlices Framework

[![Release: preview](https://img.shields.io/badge/release-0.4.0--preview.1-orange)](https://github.com/vslices/framework/releases/tag/v0.4.0-preview.1)
[![Target: .NET 10](https://img.shields.io/badge/.NET-10-512bd4)](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
[![NuGet bundle CI](https://github.com/vslices/framework/actions/workflows/nuget-preview-bundle.yml/badge.svg)](https://github.com/vslices/framework/actions/workflows/nuget-preview-bundle.yml)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue)](LICENSE)

**Feel the semantics.**

VSlices is an experimental, modular framework for C# and .NET 10. Its current design distinguishes semantic meaning (**Spaces**), executable behavior (**Work**) and the concrete realization of capabilities (**Grounding**). Presentation can consume these contracts without becoming their semantic owner.

This framework is validated through concrete implementations and tests. It does not prescribe DDD, functional programming or vertical-slice architecture as a universal taxonomy.

## Current modules

- **VSlices:** common functional mechanisms and foundations.
- **VSlices.Spaces:** semantic construction, evolution, quantities and temporal models.
- **VSlices.Work:** nominal Features with `Eff<ALG, Response> Get(Request)` and executable capability algebras.
- **VSlices.Grounding:** concrete implementations of capabilities, including time.
- **VSlices.Grounding.EntityFrameworkCore:** point-oriented EF Core implementation.
- **VSlices.Grounding.EntityFrameworkCore.Functional:** functional EF Core extensions with [documented provenance](docs/functional-ef-core.md).
- **VSlices.Views.Abstract:** presentation components and adapters.

Learn more in [Architecture](docs/architecture.md), [Feature Model](docs/feature-model.md), [Space Modeling Laws](docs/spaces/modeling.md), and [TransformAdapter Modeling](docs/transform-adapter.md).

## Releases and support

The seven NuGet packages are versioned together. The currently published coordinated preview is **0.3.0-preview.1**; see the [release history](https://github.com/vslices/framework/releases) and [package policy](docs/nuget-preview.md).

The current **0.x preview line is experimental**: APIs may change without backward compatibility. Historical v5/v6 support statements do not describe guarantees for this line.

The `VSlices.Spaces.Analyzers` experiment remains in source, **not** in the distributed NuGet packages. Consumers can explicitly run structural Laws in their tests; no compiler-analyzer coverage is promised for the published preview.

## Contributing

Consult [contribution guidance](how%20to%20contribute.md) and [AGENTS.md](AGENTS.md). Start from current source and tests rather than historical design assumptions. [Report issues](https://github.com/vslices/framework/issues).

## Support and acknowledgements

Voluntary development support: [PayPal](https://paypal.me/enyu20?country.x=CL&locale.x=es_XC).

Thanks to Álvaro Osorio for his influence in early development and to Antonio Riquelme for documentation contributions.
