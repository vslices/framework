using LanguageExt;
using VSlices.Monads;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.Tests;

public sealed class PointAlgebraTests
{
    [Fact]
    public async Task Feature_executes_directly_against_composed_point_atoms()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAccountGrounding(
            [new Account(id, "before")]);

        var response = await RenameAccount
            .Get()
            .RunFlow(
                grounding.Algebra,
                new RenameAccount.Request(id, "after"))
            .RunAsync();

        Assert.True(response.Account.IsSome);
        Assert.Equal(
            "after",
            response.Account.Match(
                static x => x.Name,
                static () => string.Empty));
        Assert.Equal(
            ["read-account", "write-account", "read-account"],
            grounding.Trace);
    }
}

public sealed record AccountAlgebra(
    PointReader<Account, AccountId> Reader,
    PointWriter<Account> Writer);

public sealed class RenameAccount :
    Feature<RenameAccount, AccountAlgebra, RenameAccount.Request, RenameAccount.Response>
{
    public sealed record Request(AccountId AccountId, string Name);
    public sealed record Response(Option<Account> Account);

    public static Flow<AccountAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Reader.Read(request.AccountId)
                .Bind(current =>
                    current.Match(
                        Some: account =>
                            algebra.Writer
                                .Write(account with { Name = request.Name })
                                .Bind(_ => algebra.Reader.Read(request.AccountId))
                                .Map(updated => new Response(updated)),
                        None: static () =>
                            IO.pure(new Response(Option<Account>.None)))));
}

public readonly record struct AccountId(Guid Value);
public sealed record Account(AccountId Id, string Name);

public sealed class InMemoryAccountGrounding :
    AlgebraIO<AccountAlgebra>,
    PointReader<Account, AccountId>,
    PointWriter<Account>
{
    private readonly Dictionary<AccountId, Account> accounts;

    public InMemoryAccountGrounding(IEnumerable<Account> accounts)
    {
        this.accounts = accounts.ToDictionary(static point => point.Id);
        Algebra = new(this, this);
    }

    public AccountAlgebra Algebra { get; }
    public List<string> Trace { get; } = [];

    public IO<Option<Account>> Read(AccountId id) =>
        IO.lift(() =>
        {
            Trace.Add("read-account");
            return accounts.TryGetValue(id, out var point)
                ? Some(point)
                : Option<Account>.None;
        });

    public IO<Unit> Write(Account point) =>
        IO.lift(() =>
        {
            Trace.Add("write-account");
            accounts[point.Id] = point;
            return unit;
        });
}
