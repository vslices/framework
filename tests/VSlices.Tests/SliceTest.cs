using VSlices.Application.Interfaces;
using VSlices.Domain;
using VSlices.Domain.Envs;
using VSlices.Domain.Envs.Clock;
using VSlices.Domain.Envs.DataAccess;
using VSlices.Infrastructure.Environments;
using VSlices.Monads;

namespace VSlices.Tests;

public sealed record CreateQuestionCommand(string Title, string Content);

public sealed record TestRT(
    PersistenceIO Persistence,
    DataAccessIO DataAccess,
    ClockIO Clock) 
    : DomainRuntime<TestRT>
{
    static K<Eff<TestRT>, PersistenceIO> Has<Eff<TestRT>, PersistenceIO>.Ask =>
        liftEff((TestRT rt) => rt.Persistence);

    static K<Eff<TestRT>, DataAccessIO> Has<Eff<TestRT>, DataAccessIO>.Ask =>
        liftEff((TestRT rt) => rt.DataAccess);

    static K<Eff<TestRT>, ClockIO> Has<Eff<TestRT>, ClockIO>.Ask =>
        liftEff((TestRT rt) => rt.Clock);
}

file sealed class CreateQuestion : Feature<CreateQuestion, TestRT, CreateQuestionCommand>
{
    public static string Name => throw new NotImplementedException();

    public static Flow<TestRT, CreateQuestionCommand, Unit> Get()
    {
        throw new NotImplementedException();
    }
}

public class SliceTest
{
    [Fact]
    public void Test1()
    {
        var runtime = new TestRT(
            Persistence: new Persistence(),
            DataAccess: new DataAccess(),
            Clock: new SystemClockIO(TimeProvider.System));
    }
}
