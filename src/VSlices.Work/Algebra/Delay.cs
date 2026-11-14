using VSlices.Spaces.Temporal;
using VSlices.Spaces.Quantities;

namespace VSlices.Work.Algebra;

/// <summary>Defines temporal delay without scheduling or invocation semantics.</summary>
public interface Delay
{
    IO<Unit> For(Duration<double> duration);
    IO<Unit> Until(Moment moment);
}
