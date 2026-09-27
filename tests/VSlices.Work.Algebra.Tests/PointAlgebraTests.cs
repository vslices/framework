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
            ["read-account-or-default", "write-account", "read-account-or-default"],
            grounding.Trace);
    }

    [Fact]
    public async Task ReadOrDefault_preserves_absence_as_an_expected_branch()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAccountGrounding([]);

        var result = await grounding
            .ReadOrDefault(id)
            .Run()
            .As()
            .RunAsync();

        Assert.True(result.IsNone);
    }

    [Fact]
    public async Task Default_Read_is_available_on_concrete_readers_and_fails_like_Single()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAccountGrounding([]);

        var error = await Assert.ThrowsAsync<InvalidOperationException>(
            async () =>
            {
                _ = await grounding.Read(id).RunAsync();
            });

        Assert.Equal(
            "Sequence contains no matching element.",
            error.Message);
    }

    [Fact]
    public async Task Concrete_Read_can_specialize_required_read_failure_semantics()
    {
        var id = new AccountId(Guid.NewGuid());
        var reader = new SpecificAccountReader();

        var error = await Assert.ThrowsAsync<AccountNotFoundException>(
            async () =>
            {
                _ = await reader.Read(id).RunAsync();
            });

        Assert.Equal(id, error.Id);
    }

    [Fact]
    public async Task Specialized_Read_dispatches_through_the_PointReader_trait()
    {
        var id = new AccountId(Guid.NewGuid());
        PointReader<Account, AccountId> reader =
            new SpecificAccountReader();

        var error = await Assert.ThrowsAsync<AccountNotFoundException>(
            async () =>
            {
                _ = await reader.Read(id).RunAsync();
            });

        Assert.Equal(id, error.Id);
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
            algebra.Reader
                .ReadOrDefault(request.AccountId)
                .Run()
                .As()
                .Bind(current =>
                    current.Match(
                        Some: account =>
                            algebra.Writer
                                .Write(account with { Name = request.Name })
                                .Bind(_ => algebra.Reader.Read(request.AccountId))
                                .Map(updated =>
                                    new Response(Some(updated))),
                        None: static () =>
                            IO.pure(new Response(Option<Account>.None)))));
}

public readonly record struct AccountId(Guid Value);
public sealed record Account(AccountId Id, string Name);

public sealed class InMemoryAccountGrounding :
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

    public OptionT<IO, Account> ReadOrDefault(AccountId id) =>
        OptionT.lift<IO, Account>(
            IO.lift(() =>
            {
                Trace.Add("read-account-or-default");
                return accounts.TryGetValue(id, out var point)
                    ? Some(point)
                    : Option<Account>.None;
            }));

    public IO<Unit> Write(Account point) =>
        IO.lift(() =>
        {
            Trace.Add("write-account");
            accounts[point.Id] = point;
            return unit;
        });
}

public sealed class SpecificAccountReader :
    PointReader<Account, AccountId>
{
    public OptionT<IO, Account> ReadOrDefault(AccountId id) =>
        OptionT<IO, Account>.None;

    public IO<Account> Read(AccountId id) =>
        IO.lift<Account>(
            () => throw new AccountNotFoundException(id));
}

public sealed class AccountNotFoundException(AccountId id) :
    InvalidOperationException($"Account '{id}' does not exist.")
{
    public AccountId Id { get; } = id;
}
