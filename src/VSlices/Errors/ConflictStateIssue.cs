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
/// Represents an issue that describes a conflict state in the application.
/// </summary>
/// <param name="Detail">A detailed description of the conflict state issue.</param>
public sealed record ConflictStateIssue(string Detail);