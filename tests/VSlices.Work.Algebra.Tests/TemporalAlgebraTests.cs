using LanguageExt;
using VSlices.Monads;
using VSlices.Space.Quantities;
using VSlices.Space.Temporal;
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
            .Get()
            .RunFlow(
                algebra,
                new TemporalProbe.Request(
                    new Duration<double>(2),
                    new Duration<double>(5)))
            .RunAsync();

        Assert.Equal(origin, response.Observed);
        Assert.Equal(
            ["now", "delay-for:2", "delay-until:5"],
            grounding.Trace);
    }
}

public sealed record TemporalAlgebra(
    ClockIO Clock,
    DelayIO Delay);

public sealed class TemporalProbe :
    Feature<TemporalProbe, TemporalAlgebra, TemporalProbe.Request, TemporalProbe.Response>
{
    public sealed record Request(
        Duration<double> DelayFor,
        Duration<double> DelayUntilOffset);

    public sealed record Response(Moment Observed);

    public static Flow<TemporalAlgebra, Request, Response> Get() =>
        new((algebra, request) =>
            algebra.Clock.Now
                .Bind(now =>
                    algebra.Delay.For(request.DelayFor)
                        .Bind(_ =>
                            algebra.Delay.Until(
                                now + request.DelayUntilOffset))
                        .Map(_ => new Response(now))));
}

public sealed class RecordingTemporalIO(Moment now) :
    ClockIO,
    DelayIO
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
