using LanguageExt;
using LanguageExt.Common;
using VSlices;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.Tests;

public sealed class FreeTTests
{
    [Fact]
    public async Task FreeT_composes_Free_instructions_with_Fin_semantics()
    {
        var id = new AccountId(Guid.NewGuid());
        var original = new Account(id, "before");
        var grounding = new InMemoryAppAlgebra(accounts: [original]);

        var program =
            from current in FreeT.liftFree<AppAlgebra, Fin, Option<Account>>(
                PointReader.read<AppAlgebra, Account, AccountId>(id))
            from account in current.ToFin(
                Error.New("Expected account to exist."))
            let updated = account with { Name = "after" }
            from _ in FreeT.liftFree<AppAlgebra, Fin, Unit>(
                PointWriter.write<AppAlgebra, Account>(updated))
            select updated;

        var result = await FreeTAlgebra
            .interpret(program, grounding)
            .RunAsync();

        var account = result.ThrowIfFail();

        Assert.Equal("after", account.Name);
        Assert.Equal(["read-account", "write-account"], grounding.Trace);
    }

    [Fact]
    public async Task Fin_failure_short_circuits_later_Free_instructions()
    {
        var id = new AccountId(Guid.NewGuid());
        var grounding = new InMemoryAppAlgebra();

        var program =
            from current in FreeT.liftFree<AppAlgebra, Fin, Option<Account>>(
                PointReader.read<AppAlgebra, Account, AccountId>(id))
            from account in current.ToFin(
                Error.New("Account was not found."))
            from _ in FreeT.liftFree<AppAlgebra, Fin, Unit>(
                PointWriter.write<AppAlgebra, Account>(
                    account with { Name = "never-written" }))
            select account;

        var result = await FreeTAlgebra
            .interpret(program, grounding)
            .RunAsync();

        Assert.True(result.IsFail);
        Assert.Equal(["read-account"], grounding.Trace);
    }
}
