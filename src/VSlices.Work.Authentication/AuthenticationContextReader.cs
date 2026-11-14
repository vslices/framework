using LanguageExt;
using VSlices.Spaces.Definitions;

namespace VSlices.Work.Algebra;

/// <summary>
/// Executable Work capability independent of cookie, JWT, HTTP or remote
/// identity mechanisms. Concrete identity types are supplied by consumers.
/// </summary>
public interface AuthenticationContextReader
{
    /// <summary>
    /// Returns an observation even when authentication is absent or rejected.
    /// Unobservable technical failures fail the IO instead of inventing absence.
    /// </summary>
    IO<AuthenticationContext> Read();
}
