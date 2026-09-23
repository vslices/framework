using LanguageExt;
using LanguageExt.Traits;
using VSlices.Space.Quantities;
using VSlices.Space.Temporal;
using VSlices.Work;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class TemporalAlgebraTests
{
    [Fact]
    public async Task Temporal_Feature_is_inert_and_separates_clock_observation_from_delay()
    {
        var origin = new Moment(
            new DateTimeOffset(2026, 9, 23, 12, 0, 0, TimeSpan.Zero));

        var temporal = new RecordingTemporalIO(origin);
        var interpreter = new TemporalProbeIO(temporal, temporal);

        var program = TemporalProbe.Get(
            new TemporalProbe.Request(
                new Duration<double>(2),
                new Duration<double>(5)));

        Assert.Empty(temporal.Trace);

        var response = await FreeAlgebra
            .interpret(program, interpreter)
            .RunAsync();

        Assert.Equal(origin, response.Observed);
        Assert.Equal(
            ["now", "delay-for:2", "delay-until:5"],
            temporal.Trace);
    }
}

public sealed class TemporalProbe :
    Feature<
        TemporalProbe,
        TemporalProbe.Algebra,
        TemporalProbe.Request,
        TemporalProbe.Response>
{
    public sealed record Request(
        Duration<double> DelayFor,
        Duration<double> DelayUntilOffset);

    public sealed record Response(Moment Observed);

    public abstract record WorkPart<A> : K<Algebra, A>;

    public sealed record NowPart<A>(
        Func<Moment, A> Next)
        : WorkPart<A>;

    public sealed record DelayForPart<A>(
        Duration<double> Duration,
        Func<Unit, A> Next)
        : WorkPart<A>;

    public sealed record DelayUntilPart<A>(
        Moment Moment,
        Func<Unit, A> Next)
        : WorkPart<A>;

    public sealed class Algebra :
        Functor<Algebra>,
        Clock<Algebra>,
        Delay<Algebra>
    {
        static K<Algebra, Moment>
            Clock<Algebra>.Now() =>
            new NowPart<Moment>(static moment => moment);

        static K<Algebra, Unit>
            Delay<Algebra>.For(Duration<double> duration) =>
            new DelayForPart<Unit>(
                duration,
                static value => value);

        static K<Algebra, Unit>
            Delay<Algebra>.Until(Moment moment) =>
            new DelayUntilPart<Unit>(
                moment,
                static value => value);

        static K<Algebra, B> Functor<Algebra>.Map<A, B>(
            Func<A, B> f,
            K<Algebra, A> ma) =>
            ma switch
            {
                NowPart<A>(var next) =>
                    new NowPart<B>(
                        moment => f(next(moment))),
                DelayForPart<A>(var duration, var next) =>
                    new DelayForPart<B>(
                        duration,
                        value => f(next(value))),
                DelayUntilPart<A>(var moment, var next) =>
                    new DelayUntilPart<B>(
                        moment,
                        value => f(next(value))),
                _ => throw new NotSupportedException()
            };
    }

    public static Free<Algebra, Response> Get(Request request) =>
        from now in Clock.now<Algebra>()
        from _ in Delay.forDuration<Algebra>(request.DelayFor)
        from __ in Delay.until<Algebra>(
            now + request.DelayUntilOffset)
        select new Response(now);
}

public sealed class TemporalProbeIO(
    ClockIO clock,
    DelayIO delay)
    : AlgebraIO<TemporalProbe.Algebra>
{
    public IO<A> Interpret<A>(
        K<TemporalProbe.Algebra, A> operation) =>
        operation switch
        {
            TemporalProbe.NowPart<A> now =>
                clock.Now.Map(now.Next),
            TemporalProbe.DelayForPart<A> wait =>
                delay.For(wait.Duration).Map(wait.Next),
            TemporalProbe.DelayUntilPart<A> wait =>
                delay.Until(wait.Moment).Map(wait.Next),
            _ => throw new NotSupportedException()
        };
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
