using LanguageExt;
using VSlices.Core;
using VSlices.Core.Interfaces;
using VSlices.Domain.Environments.Persistence;

namespace VSlices.Tests;

public sealed record CreateQuestionCommand(string Title, string Content);

file sealed class CreateQuestion<RT> : Feature<RT, CreateQuestionCommand>
    where RT : HasPersistence<RT>
{
    public static string Name => throw new NotImplementedException();

    public static FeatureEff<RT, Unit> Handle(CreateQuestionCommand input) => 
        PersistenceEnv<RT>.start();
}

public class SliceTest
{
    [Fact]
    public void Test1()
    {

    }
}
