using LanguageExt;
using LanguageExt.Common;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class OptionTFallibleTests
{
    [Fact]
    public void Some_is_preserved_without_evaluating_the_missing_error()
    {
        var calls = 0;
        Fin<int> result = OptionT.lift<Fin, int>(Prelude.Some(42))
            .IfNone(() =>
            {
                calls++;
                return Error.New("not_found");
            }).As();

        Assert.Equal(42, result.Match(Succ: x => x, Fail: _ => -1));
        Assert.Equal(0, calls);
    }

    [Fact]
    public void None_is_converted_to_the_supplied_failure()
    {
        var calls = 0;
        Fin<int> result = OptionT.lift<Fin, int>(Option<int>.None)
            .IfNone(() =>
            {
                calls++;
                return Error.New("not_found");
            }).As();

        Assert.True(result.IsFail);
        Assert.Contains("not_found", result.ToString());
        Assert.Equal(1, calls);
    }
}
