using VSlices.Space.Quantities;
using VSlices.Space.Temporal;

namespace VSlices.Work;

/// <summary>
/// Realizes observation of the current semantic moment.
/// </summary>
public interface ClockIO
{
    IO<Moment> Now { get; }
}

/// <summary>
/// Realizes temporal delay without defining scheduling or invocation semantics.
/// </summary>
public interface DelayIO
{
    IO<Unit> For(Duration<double> duration);
    IO<Unit> Until(Moment moment);
}
