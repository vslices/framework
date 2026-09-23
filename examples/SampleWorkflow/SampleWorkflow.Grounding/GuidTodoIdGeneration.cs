using LanguageExt;
using SampleWorkflow.Spaces;
using SampleWorkflow.Work;

namespace SampleWorkflow.Grounding;

/// <summary>
/// Grounding that realizes Todo identity generation with Guid.NewGuid.
/// </summary>
public sealed class GuidTodoIdGeneration : TodoIdGenerationIO
{
    public IO<TodoId> Next =>
        IO.lift(Guid.NewGuid)
            .Bind(id => TodoId.Transformation
                .RunFin(id)
                .Match(
                    Succ: IO.pure,
                    Fail: IO.fail<TodoId>));
}
