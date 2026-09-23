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
    public async Task Point_capabilities_build_an_inert_free_program_and_grounding_interprets_it()
    {
        var id = new AccountId(Guid.NewGuid());
        var original = new Account(id, "before");
        var grounding = new InMemoryAppAlgebra(accounts: [original]);

        var program = AppPrograms.Rename(id, "after");

        Assert.Empty(grounding.Trace);
        Assert.Equal("before", grounding.Current(id).Match(static x => x.Name, static () => string.Empty));

        var result = await FreeAlgebra.interpret(program, grounding).RunAsync();

        Assert.True(result.IsSome);
        Assert.Equal("after", result.Match(static x => x.Name, static () => string.Empty));
        Assert.Equal("after", grounding.Current(id).Match(static x => x.Name, static () => string.Empty));
        Assert.Equal(["read-account", "write-account", "read-account"], grounding.Trace);
    }

    [Fact]
    public async Task The_same_free_program_can_be_interpreted_by_a_different_grounding()
    {
        var id = new AccountId(Guid.NewGuid());
        var program = AppPrograms.Rename(id, "after");

        var existing = new InMemoryAppAlgebra(accounts: [new Account(id, "before")]);
        var missing = new InMemoryAppAlgebra();

        var existingResult = await FreeAlgebra.interpret(program, existing).RunAsync();
        var missingResult = await FreeAlgebra.interpret(program, missing).RunAsync();

        Assert.True(existingResult.IsSome);
        Assert.True(missingResult.IsNone);
        Assert.Equal(["read-account", "write-account", "read-account"], existing.Trace);
        Assert.Equal(["read-account", "read-account"], missing.Trace);
    }

    [Fact]
    public async Task A_service_owned_algebra_composes_multiple_point_spaces_and_runs_inside_Flow()
    {
        var accountId = new AccountId(Guid.NewGuid());
        var roleId = new RoleId(Guid.NewGuid());

        var grounding = new InMemoryAppAlgebra(
            accounts: [new Account(accountId, "before")],
            roles: [new Role(roleId, "editor")]);

        var runtime = new TestRuntime(grounding);
        var request = new RenameAccount<TestRuntime>.Request(accountId, roleId, "after");

        var response = await RenameAccount<TestRuntime>
            .Get()
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.True(response.Account.IsSome);
        Assert.Equal(
            "after",
            response.Account.Match(static x => x.Name, static () => string.Empty));

        Assert.Equal(
            ["read-account", "read-role", "write-account", "read-account"],
            grounding.Trace);
    }
}

public static class AppPrograms
{
    public static Free<AppAlgebra, Option<Account>> Rename(
        AccountId id,
        string name) =>
        from current in PointReader.read<AppAlgebra, Account, AccountId>(id)
        from _ in current.Match(
            Some: account => PointWriter.write<AppAlgebra, Account>(account with { Name = name }),
            None: static () => Free.pure<AppAlgebra, Unit>(unit))
        from updated in PointReader.read<AppAlgebra, Account, AccountId>(id)
        select updated;

    public static Free<AppAlgebra, Option<Account>> RenameWhenRoleExists(
        AccountId accountId,
        RoleId roleId,
        string name) =>
        from account in PointReader.read<AppAlgebra, Account, AccountId>(accountId)
        from role in PointReader.read<AppAlgebra, Role, RoleId>(roleId)
        from _ in account.Match(
            Some: current => role.Match(
                Some: _ => PointWriter.write<AppAlgebra, Account>(current with { Name = name }),
                None: static () => Free.pure<AppAlgebra, Unit>(unit)),
            None: static () => Free.pure<AppAlgebra, Unit>(unit))
        from updated in PointReader.read<AppAlgebra, Account, AccountId>(accountId)
        select updated;
}

public sealed class RenameAccount<RT> :
    Feature<RenameAccount<RT>, RT, RenameAccount<RT>.Request, RenameAccount<RT>.Response>
    where RT : HasAlgebra<AppAlgebra, RT>
{
    public sealed record Request(
        AccountId AccountId,
        RoleId RoleId,
        string Name);

    public sealed record Response(Option<Account> Account);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<AppAlgebra, RT>
            .run(AppPrograms.RenameWhenRoleExists(
                request.AccountId,
                request.RoleId,
                request.Name))
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

public readonly record struct RoleId(Guid Value);

public sealed record Role(RoleId Id, string Name);

public abstract record AppOperation<A> : K<AppAlgebra, A>;

public sealed record ReadAccount<A>(
    AccountId Id,
    Func<Option<Account>, A> Next)
    : AppOperation<A>;

public sealed record WriteAccount<A>(
    Account Point,
    Func<Unit, A> Next)
    : AppOperation<A>;

public sealed record ReadRole<A>(
    RoleId Id,
    Func<Option<Role>, A> Next)
    : AppOperation<A>;

public sealed class AppAlgebra :
    Functor<AppAlgebra>,
    PointReader<AppAlgebra, Account, AccountId>,
    PointWriter<AppAlgebra, Account>,
    PointReader<AppAlgebra, Role, RoleId>
{
    static K<AppAlgebra, Option<Account>>
        PointReader<AppAlgebra, Account, AccountId>.Read(AccountId id) =>
        new ReadAccount<Option<Account>>(id, static point => point);

    static K<AppAlgebra, Unit>
        PointWriter<AppAlgebra, Account>.Write(Account point) =>
        new WriteAccount<Unit>(point, static value => value);

    static K<AppAlgebra, Option<Role>>
        PointReader<AppAlgebra, Role, RoleId>.Read(RoleId id) =>
        new ReadRole<Option<Role>>(id, static point => point);

    static K<AppAlgebra, B> Functor<AppAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<AppAlgebra, A> ma) =>
        ma switch
        {
            ReadAccount<A>(var id, var next) =>
                new ReadAccount<B>(id, point => f(next(point))),

            WriteAccount<A>(var point, var next) =>
                new WriteAccount<B>(point, value => f(next(value))),

            ReadRole<A>(var id, var next) =>
                new ReadRole<B>(id, point => f(next(point))),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(AppAlgebra)} operation.")
        };
}

public sealed class InMemoryAppAlgebra : AlgebraIO<AppAlgebra>
{
    private readonly Dictionary<AccountId, Account> accounts;
    private readonly Dictionary<RoleId, Role> roles;

    public InMemoryAppAlgebra(
        IEnumerable<Account>? accounts = null,
        IEnumerable<Role>? roles = null)
    {
        this.accounts = (accounts ?? [])
            .ToDictionary(static point => point.Id);

        this.roles = (roles ?? [])
            .ToDictionary(static point => point.Id);
    }

    public List<string> Trace { get; } = [];

    public Option<Account> Current(AccountId id) =>
        accounts.TryGetValue(id, out var point)
            ? Some(point)
            : None;

    public Option<Role> Current(RoleId id) =>
        roles.TryGetValue(id, out var point)
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

            ReadRole<A> read =>
                IO.lift(() =>
                {
                    Trace.Add("read-role");
                    return read.Next(Current(read.Id));
                }),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(AppAlgebra)} operation.")
        };
}
