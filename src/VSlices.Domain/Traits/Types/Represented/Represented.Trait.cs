using System.ComponentModel;

namespace VSlices.Domain.Traits;

/// <summary>
///
/// </summary>
/// <typeparam name="REPR">
///
/// </typeparam>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface Represented<REPR>
{
    /// <summary>
    ///
    /// </summary>
    /// <returns>
    ///
    /// </returns>
    REPR To();
}
