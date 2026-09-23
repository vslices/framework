using LanguageExt;
using LanguageExt.Traits;
using SampleWorkflow.Spaces;

namespace SampleWorkflow.Work;

/// <summary>
/// Work capability for obtaining a new semantic Todo identity.
/// </summary>
public interface TodoIdGenerationIO
{
    IO<TodoId> Next { get; }
}

public interface HasTodoIdGeneration<RT> :
    Has<Eff<RT>, TodoIdGenerationIO>;

public static class TodoIdGenerationEnv<RT>
    where RT : HasTodoIdGeneration<RT>
{
    private static Eff<RT, TodoIdGenerationIO> generation =>
        Has<Eff<RT>, RT, TodoIdGenerationIO>.ask.As();

    public static Eff<RT, TodoId> next =>
        generation.Bind(value => value.Next);
}
