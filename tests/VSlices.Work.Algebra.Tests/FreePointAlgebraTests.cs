using LanguageExt;
using LanguageExt.Traits;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.FreeAlgebra.Tests;

public sealed class FreePointAlgebraTests
{
    [Fact]
    public async Task Point_capabilities_build_an_inert_free_program_and_grounding_interprets_it()
    {
        var id = new AccountId(Guid.NewGuid());
        var original = new Account(id, "before");
        var grounding = new InMemoryAccountAlgebra(original);

        var program = Rename(id, "after");

        Assert.Empty(grounding.Trace);
        Assert.Equal("before", grounding.Current(id).Match(static x => x.Name, static () => string.Empty));

        var result = await Algebra.interpret(program, grounding).RunAsync();

        Assert.True(result.IsSome);
        Assert.Equal("after", result.Match(static x => x.Name, static () => string.Empty));
        Assert.Equal("after", grounding.Current(id).Match(static x => x.Name, static () => string.Empty));
        Assert.Equal(["read", "write", "read"], grounding.Trace);
    }

    [Fact]
    public async Task The_same_free_program_can_be_interpreted_by_a_different_grounding()
    {
        var id = new AccountId(Guid.NewGuid());
        var program = Rename(id, "after");

        var existing = new InMemoryAccountAlgebra(new Account(id, "before"));
        var missing = new InMemoryAccountAlgebra();

        var existingResult = await Algebra.interpret(program, existing).RunAsync();
        var missingResult = await Algebra.interpret(program, missing).RunAsync();

        Assert.True(existingResult.IsSome);
        Assert.True(missingResult.IsNone);
        Assert.Equal(["read", "write", "read"], existing.Trace);
        Assert.Equal(["read", "read"], missing.Trace);
    }

    private static Free<AccountAlgebra, Option<Account>> Rename(
        AccountId id,
        string name) =>
        from current in PointReader.read<AccountAlgebra, Account, AccountId>(id)
        from _ in current.Match(
            Some: account => PointWriter.write<AccountAlgebra, Account>(account with { Name = name }),
            None: static () => Free.pure<AccountAlgebra, Unit>(unit))
        from updated in PointReader.read<AccountAlgebra, Account, AccountId>(id)
        select updated;
}

public readonly record struct AccountId(Guid Value);

public sealed record Account(AccountId Id, string Name);

public abstract record AccountOperation<A> : K<AccountAlgebra, A>;

public sealed record ReadAccount<A>(
    AccountId Id,
    Func<Option<Account>, A> Next)
    : AccountOperation<A>;

public sealed record WriteAccount<A>(
    Account Point,
    Func<Unit, A> Next)
    : AccountOperation<A>;

public sealed class AccountAlgebra :
    Functor<AccountAlgebra>,
    PointReader<AccountAlgebra, Account, AccountId>,
    PointWriter<AccountAlgebra, Account>
{
    static K<AccountAlgebra, Option<Account>>
        PointReader<AccountAlgebra, Account, AccountId>.Read(AccountId id) =>
        new ReadAccount<Option<Account>>(id, static point => point);

    static K<AccountAlgebra, Unit>
        PointWriter<AccountAlgebra, Account>.Write(Account point) =>
        new WriteAccount<Unit>(point, static value => value);

    static K<AccountAlgebra, B> Functor<AccountAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<AccountAlgebra, A> ma) =>
        ma switch
        {
            ReadAccount<A>(var id, var next) =>
                new ReadAccount<B>(id, point => f(next(point))),

            WriteAccount<A>(var point, var next) =>
                new WriteAccount<B>(point, value => f(next(value))),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(AccountAlgebra)} operation.")
        };
}

public sealed class InMemoryAccountAlgebra : AlgebraIO<AccountAlgebra>
{
    private readonly Dictionary<AccountId, Account> points;

    public InMemoryAccountAlgebra(params Account[] initial) =>
        points = initial.ToDictionary(static point => point.Id);

    public List<string> Trace { get; } = [];

    public Option<Account> Current(AccountId id) =>
        points.TryGetValue(id, out var point)
            ? Some(point)
            : None;

    public IO<A> Interpret<A>(K<AccountAlgebra, A> operation) =>
        operation switch
        {
            ReadAccount<A> read =>
                IO.lift(() =>
                {
                    Trace.Add("read");
                    return read.Next(Current(read.Id));
                }),

            WriteAccount<A> write =>
                IO.lift(() =>
                {
                    Trace.Add("write");
                    points[write.Point.Id] = write.Point;
                    return write.Next(unit);
                }),

            _ => throw new NotSupportedException(
                $"Unknown {nameof(AccountAlgebra)} operation.")
        };
}
