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
namespace VSlices.Literals;

/// <summary>
/// Represents a strongly-typed constant value of true or false, backed by a <see cref="bool"/> value.
/// </summary>
public sealed class True : Const<bool>
{
    /// <summary>
    /// Represents the boolean value true.
    /// </summary>
    public static bool Value { get; } = true;
}

/// <summary>
/// Represents a strongly-typed constant value of false, backed by a <see cref="bool"/> value.
/// </summary>
public sealed class False : Const<bool>
{
    /// <summary>
    /// Represents the boolean value false.
    /// </summary>
    public static bool Value { get; } = false;
}