using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ForkExt;
using ForkExt.Common;
using ForkExt.Traits;
using ForkExt.Effects;
using ForkExt.Traits.Domain;
using static ForkExt.Prelude;
using VSlices;
using VSlices.Monads;
using static VSlices.VSlicesPrelude;
namespace VSlices.Errors;

/// <summary>
/// Represents a validation issue encountered during processing.
/// </summary>
/// <param name="Detail">A detailed description of the validation issue.</param>
/// <param name="PropertyPath">The path to the property associated with the validation issue.</param>
public sealed record ValidationIssue(string Detail, string PropertyPath);
