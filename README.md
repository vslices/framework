<p align="center">
  <img src="icons/icon hq.png" alt="icon" width="192px">
</p>
<p align="center">A high-level, develop in C#, modular framework  that aims to a faster but clean and pragmatic software developtment</p>
<p align="center">
  <a href="https://www.nuget.org/packages/VSlices.Core.Abstracts">
    <img src="https://buildstats.info/nuget/vslices.core.abstracts" alt="VSlices.Core.Abstract release package info" />
  </a>
  <a href="https://github.com/HernanFAR/vslices-framework/actions/workflows/test.yml">
    <img src="https://github.com/HernanFAR/vslice-framework/actions/workflows/test.yml/badge.svg" alt="master-test.yml" />
  </a>
  <a href="https://github.com/HernanFAR/vslices-framework/blob/master/LICENSE">
    <img src="https://img.shields.io/badge/license-MIT-purple" alt="VSlices license" />
  </a>
  <a href="https://paypal.me/enyu20">
    <img src="https://img.shields.io/badge/donate-paypal-red" alt="VSlices paypal donation link" />
  </a>
</p>

# Scope

This repository contains the current **.NET realization of VSlices Framework**.

It is not the complete semantic definition of VSlices Framework.

The broader Framework responsibility is to represent or condense software intent so that it can serve as a basis for later realizations, derivations, transformations, validation, testing, reuse, or production. A runtime library, C# API, source generator, analyzer, VSIR representation, lowering pipeline, or other executable mechanism is one possible realization of that responsibility.

Therefore:

```text
VSlices Framework
!=
this .NET implementation

Framework
!=
runtime

Framework
!=
mandatory IR

Tooling executes or assists mechanisms
!=
semantic authority over Framework
```

This repository should describe the guarantees and behavior of the .NET realization while suite- and product-level definitions remain authoritative for the broader responsibility.

# About
This .NET realization is a high-level modular framework for C# inspired by vertical slice architecture and functional programming, with support for domain-driven design practices. Its implementation explores how software semantics and guarantees can be expressed through .NET types, APIs, effects, runtime mechanisms, analyzers, generators, tests, and related ecosystem capabilities.

**Unofficial motto:** *feel the semantics*

The documentation of the software is [here in readthedocs](https://vslice-framework.readthedocs.io/en/latest/), there you'll learn the functionalities that the framework brings. It also has and explication of the implementation of the proposed architecture and the good practices

If you find a problem with the documentation, have a improvement or clarification proposal, you can generate a ticket here: [link](https://github.com/HernanFAR/vslice-framework/issues)

## Versioning
We use [Semantic Versioning v2.0.0](https://semver.org/spec/v2.0.0.html) for production-ready releases:
- We increment Major (the X in X.Y.Z) versions when we make a change that breaks backward compatibility. The current major version is 5.
- We increment Minor (the Y in X.Y.Z) versions when we add backward compatible features.
- We increment Patch (the Z in X.Y.Z) versions when we make backward compatible bug fixes or improvements in the documentation.
- We increment Pre-releases (the W in X.Y.Z-pre.W) versions, when we make a pull request to the related release.

Also, the framework's packages will be updated **at the same version, at the same time**, that implies that if VSlices.Core.Abstracts gets updated to the v6, **all the nugets will be updated to the v6**.

## Support policy
Since v6, we will only support the latest 2 versions, except in v6, in which only we'll support v6. 

That means when we release v6, only v6 will be supported and will get security updates or bug fixes, and v5 backwards will no longer get security updates.

## How to contribute
If you want to make contributions to the framework, you must follow the framework convetions. This is detailed [here](how%20to%20contribute.md)

## Support VSlices development
First of all, thanks so much :) you can support the development of the framework (and my adiction to cafeine) here, with [paypal](https://paypal.me/enyu20?country.x=CL&locale.x=es_XC)

## Thanks
- To Alvaro Osorio, for beign a good influence in my early years in development.
- To Antonio Riquelme, for help me in the documentation of the framework.

