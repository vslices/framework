using VSlices.Spaces.Temporal;
using VSlices.Spaces.Quantities;
using VSlices.Work.Algebra;
using System.Runtime.CompilerServices;
using LanguageExt;
using VSlices.Monads;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class TemporalAlgebraTests
{
    [Fact]
    public async Task Temporal_Feature_uses_executable_temporal_atoms_directly()
    {
        var origin = new Moment(
            new DateTimeOffset(2026, 9, 23, 12, 0, 0, TimeSpan.Zero));

        var grounding = new RecordingTemporalIO(origin);
        var algebra = new TemporalAlgebra(grounding, grounding);

        var response = await TemporalProbe
            .Get(new TemporalProbe.Request(
                    new Duration<double>(2),
                    new Duration<double>(5))).RunIO(algebra)
            .RunAsync(TestContext.Current.CancellationToken);

        Assert.Equal(origin, response.Observed);
        Assert.Equal(
            ["now", "delay-for:2", "delay-until:5"],
            grounding.Trace);
    }
}

public sealed record TemporalAlgebra(
    Clock Clock,
    Delay Delay);

public sealed class TemporalProbe :
    Feature<TemporalProbe, TemporalAlgebra, TemporalProbe.Request, TemporalProbe.Response>
{
    public sealed record Request(
        Duration<double> DelayFor,
        Duration<double> DelayUntilOffset);

    public sealed record Response
    {
        private Response(Moment observed) => Observed = observed;
        public Moment Observed { get; }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Response NewResponse(Moment observed);

    public static Eff<TemporalAlgebra, Response> Get(Request request) =>
        Eff<TemporalAlgebra, Response>.LiftIO(algebra =>
            algebra.Clock.Now
                .Bind(now =>
                    algebra.Delay.For(request.DelayFor)
                        .Bind(_ =>
                            algebra.Delay.Until(
                                now + request.DelayUntilOffset))
                        .Map(_ => NewResponse(now))));
}

public sealed class RecordingTemporalIO(Moment now) :
    Clock,
    Delay
{
    public List<string> Trace { get; } = [];

    public IO<Moment> Now =>
        IO.lift(() =>
        {
            Trace.Add("now");
            return now;
        });

    public IO<Unit> For(Duration<double> duration) =>
        IO.lift(() =>
        {
            Trace.Add($"delay-for:{duration.Value}");
            return default(Unit);
        });

    public IO<Unit> Until(Moment moment) =>
        IO.lift(() =>
        {
            Trace.Add($"delay-until:{(moment - now).Value}");
            return default(Unit);
        });
}
