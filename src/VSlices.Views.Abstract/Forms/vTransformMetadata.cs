using VSlices.Domain.Traits;

namespace VSlices.Views.Abstract.Forms;

/// <summary>
/// Describes presentation metadata for a value produced through a VSlices transform.
/// </summary>
/// <remarks>
/// The transformed domain type remains unaware of presentation concerns. Consumers can
/// provide an adapter implementing this interface when a view needs metadata associated
/// with a particular transformation boundary.
/// </remarks>
/// <typeparam name="T">The value produced by the transformation.</typeparam>
/// <typeparam name="TInput">The representation accepted by the transformation.</typeparam>
public interface vTransformMetadata<T, TInput>
    where T : Transform<T, TInput>
{
    /// <summary>
    /// Gets a human-readable name for the value at the view boundary.
    /// </summary>
    string? DisplayName { get; }
}

/// <summary>
/// Metadata used by text inputs that materialize a domain value from a string.
/// </summary>
/// <typeparam name="T">The value materialized by the text input.</typeparam>
public sealed record vTextInputMetadata<T>(
    string? DisplayName = null,
    string InputType = "text",
    string? AutoComplete = null,
    string? Placeholder = null)
    : vTransformMetadata<T, string>
    where T : Transform<T, string>;
