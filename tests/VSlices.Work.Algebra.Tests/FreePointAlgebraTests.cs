using LanguageExt;
using LanguageExt.Traits;
using VSlices.Monads;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.Tests;

public sealed class FreePointAlgebraTests
{
    [Fact]
    public async Task Point_program_is_inert_until_interpreted()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAppAlgebra(
            accounts: [new Account(id, "before")]);

        var program = RenameAccountWork.Program(id, "after");

        Assert.Empty(grounding.Trace);

        var account = await FreeAlgebra
            .interpret(program, grounding)
            .RunAsync();

        Assert.True(account.IsSome);
        Assert.Equal("after", account.Match(static x => x.Name, static () => string.Empty));
        Assert.Equal(["read-account", "write-account", "read-account"], grounding.Trace);
    }

    [Fact]
    public async Task Feature_exposes_the_same_work_through_Flow()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAppAlgebra(
            accounts: [new Account(id, "before")]);
        var runtime = new TestRuntime(grounding);

        var response = await RenameAccount<TestRuntime>
            .Get()
            .RunFlow(
                runtime,
                new RenameAccount<TestRuntime>.Request(id, "after"))
            .RunAsync();

        Assert.True(response.Account.IsSome);
        Assert.Equal("after", response.Account.Match(static x => x.Name, static () => string.Empty));
        Assert.Equal(["read-account", "write-account", "read-account"], grounding.Trace);
    }
}

public static class RenameAccountWork
{
    public static Free<AppAlgebra, Option<Account>> Program(
        AccountId id,
        string name) =>
        from current in PointReader.read<AppAlgebra, Account, AccountId>(id)
        from _ in current.Match(
            Some: account =>
                PointWriter.write<AppAlgebra, Account>(
                    account with { Name = name }),
            None: static () =>
                Free.pure<AppAlgebra, Unit>(unit))
        from updated in PointReader.read<AppAlgebra, Account, AccountId>(id)
        select updated;
}

public sealed class RenameAccount<RT> :
    Feature<RenameAccount<RT>, RT, RenameAccount<RT>.Request, RenameAccount<RT>.Response>
    where RT : HasAlgebra<AppAlgebra, RT>
{
    public sealed record Request(AccountId AccountId, string Name);
    public sealed record Response(Option<Account> Account);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<AppAlgebra, RT>
            .run(RenameAccountWork.Program(request.AccountId, request.Name))
            .Map(account => new Response(account)));
}

public sealed record TestRuntime(AlgebraIO<AppAlgebra> AppAlgebra)
    : HasAlgebra<AppAlgebra, TestRuntime>
{
    static K<Eff<TestRuntime>, AlgebraIO<AppAlgebra>>
        Has<Eff<TestRuntime>, AlgebraIO<AppAlgebra>>.Ask { get; } =
        liftEff<TestRuntime, AlgebraIO<AppAlgebra>>(runtime => runtime.AppAlgebra);
}

public readonly record struct AccountId(Guid Value);
public sealed record Account(AccountId Id, string Name);

public abstract record AppOperation<A> : K<AppAlgebra, A>;
public sealed record ReadAccount<A>(AccountId Id, Func<Option<Account>, A> Next) : AppOperation<A>;
public sealed record WriteAccount<A>(Account Point, Func<Unit, A> Next) : AppOperation<A>;

public sealed class AppAlgebra :
    Functor<AppAlgebra>,
    PointReader<AppAlgebra, Account, AccountId>,
    PointWriter<AppAlgebra, Account>
{
    static K<AppAlgebra, Option<Account>>
        PointReader<AppAlgebra, Account, AccountId>.Read(AccountId id) =>
        new ReadAccount<Option<Account>>(id, static point => point);

    static K<AppAlgebra, Unit>
        PointWriter<AppAlgebra, Account>.Write(Account point) =>
        new WriteAccount<Unit>(point, static value => value);

    static K<AppAlgebra, B> Functor<AppAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<AppAlgebra, A> ma) =>
        ma switch
        {
            ReadAccount<A>(var id, var next) =>
                new ReadAccount<B>(id, point => f(next(point))),
            WriteAccount<A>(var point, var next) =>
                new WriteAccount<B>(point, value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}

public sealed class InMemoryAppAlgebra : AlgebraIO<AppAlgebra>
{
    private readonly Dictionary<AccountId, Account> accounts;

    public InMemoryAppAlgebra(IEnumerable<Account>? accounts = null) =>
        this.accounts = (accounts ?? []).ToDictionary(static point => point.Id);

    public List<string> Trace { get; } = [];

    public Option<Account> Current(AccountId id) =>
        accounts.TryGetValue(id, out var point) ? Some(point) : None;

    public IO<A> Interpret<A>(K<AppAlgebra, A> operation) =>
        operation switch
        {
            ReadAccount<A> read =>
                IO.lift(() =>
                {
                    Trace.Add("read-account");
                    return read.Next(Current(read.Id));
                }),
            WriteAccount<A> write =>
                IO.lift(() =>
                {
                    Trace.Add("write-account");
                    accounts[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),
            _ => throw new NotSupportedException()
        };
}
