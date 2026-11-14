using VSlices.Spaces.Definitions;

namespace VSlices.Work.Algebra;

/// <summary>Defines observation of the current semantic moment.</summary>
public interface Clock
{
    IO<Moment> Now { get; }
}
