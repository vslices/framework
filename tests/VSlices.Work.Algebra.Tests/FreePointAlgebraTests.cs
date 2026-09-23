using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.Tests;

public sealed class FreePointAlgebraTests
{
    [Fact]
    public async Task Feature_is_an_inert_Free_program_until_interpreted()
    {
        var id = new AccountId(Guid.NewGuid());
        var original = new Account(id, "before");
        var grounding = new InMemoryAppAlgebra(accounts: [original]);

        var program = RenameAccount.Describe(
            new RenameAccount.Request(id, "after"));

        Assert.Empty(grounding.Trace);

        var response = await FreeAlgebra
            .interpret(program, grounding)
            .RunAsync();

        Assert.True(response.Account.IsSome);
        Assert.Equal("after", response.Account.Match(static x => x.Name, static () => string.Empty));
        Assert.Equal(["read-account", "write-account", "read-account"], grounding.Trace);
    }

    [Fact]
    public async Task The_same_Feature_can_be_interpreted_by_a_different_Grounding()
    {
        var id = new AccountId(Guid.NewGuid());
        var program = RenameAccount.Describe(
            new RenameAccount.Request(id, "after"));

        var existing = new InMemoryAppAlgebra(
            accounts: [new Account(id, "before")]);

        var missing = new InMemoryAppAlgebra();

        var existingResult = await FreeAlgebra.interpret(program, existing).RunAsync();
        var missingResult = await FreeAlgebra.interpret(program, missing).RunAsync();

        Assert.True(existingResult.Account.IsSome);
        Assert.True(missingResult.Account.IsNone);
        Assert.Equal(["read-account", "write-account", "read-account"], existing.Trace);
        Assert.Equal(["read-account", "read-account"], missing.Trace);
    }
}

public sealed class RenameAccount :
    Feature<AppAlgebra, RenameAccount.Request, RenameAccount.Response>
{
    public sealed record Request(
        AccountId AccountId,
        string Name);

    public sealed record Response(Option<Account> Account);

    public static Free<AppAlgebra, Response> Describe(Request request) =>
        from current in PointReader.read<AppAlgebra, Account, AccountId>(
            request.AccountId)
        from _ in current.Match(
            Some: account =>
                PointWriter.write<AppAlgebra, Account>(
                    account with { Name = request.Name }),
            None: static () =>
                Free.pure<AppAlgebra, Unit>(unit))
        from updated in PointReader.read<AppAlgebra, Account, AccountId>(
            request.AccountId)
        select new Response(updated);
}

public readonly record struct AccountId(Guid Value);

public sealed record Account(AccountId Id, string Name);

public abstract record AppOperation<A> : K<AppAlgebra, A>;

public sealed record ReadAccount<A>(
    AccountId Id,
    Func<Option<Account>, A> Next)
    : AppOperation<A>;

public sealed record WriteAccount<A>(
    Account Point,
    Func<Unit, A> Next)
    : AppOperation<A>;

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

    public InMemoryAppAlgebra(
        IEnumerable<Account>? accounts = null) =>
        this.accounts = (accounts ?? [])
            .ToDictionary(static point => point.Id);

    public List<string> Trace { get; } = [];

    public Option<Account> Current(AccountId id) =>
        accounts.TryGetValue(id, out var point)
            ? Some(point)
            : None;

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
