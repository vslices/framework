using LanguageExt;
using LanguageExt.Traits;
using VSlices;
using VSlices.Monads;
using VSlices.Space.Quantities;
using VSlices.Space.Temporal;
using VSlices.Work;
using Xunit;
using static LanguageExt.Prelude;

namespace VSlices.Work.Tests;

public sealed class TemporalAlgebraTests
{
    [Fact]
    public async Task Temporal_program_is_inert_and_separates_clock_observation_from_delay()
    {
        var origin = new Moment(
            new DateTimeOffset(2026, 9, 23, 12, 0, 0, TimeSpan.Zero));

        var temporal = new RecordingTemporalIO(origin);
        var interpreter = new TemporalProbeIO(temporal, temporal);

        var program = TemporalProbeWork.Program(
            new Duration<double>(2),
            new Duration<double>(5));

        Assert.Empty(temporal.Trace);

        var response = await FreeAlgebra
            .interpret(program, interpreter)
            .RunAsync();

        Assert.Equal(origin, response);
        Assert.Equal(
            ["now", "delay-for:2", "delay-until:5"],
            temporal.Trace);
    }

    [Fact]
    public async Task Temporal_Feature_exposes_the_program_through_Flow()
    {
        var origin = new Moment(
            new DateTimeOffset(2026, 9, 23, 12, 0, 0, TimeSpan.Zero));

        var temporal = new RecordingTemporalIO(origin);
        var interpreter = new TemporalProbeIO(temporal, temporal);
        var runtime = new TemporalRuntime(interpreter);

        var response = await TemporalProbe<TemporalRuntime>
            .Get()
            .RunFlow(
                runtime,
                new TemporalProbe<TemporalRuntime>.Request(
                    new Duration<double>(2),
                    new Duration<double>(5)))
            .RunAsync();

        Assert.Equal(origin, response.Observed);
        Assert.Equal(
            ["now", "delay-for:2", "delay-until:5"],
            temporal.Trace);
    }
}

public sealed class TemporalProbe<RT> :
    Feature<
        TemporalProbe<RT>,
        RT,
        TemporalProbe<RT>.Request,
        TemporalProbe<RT>.Response>
    where RT : HasAlgebra<TemporalProbeAlgebra, RT>
{
    public sealed record Request(
        Duration<double> DelayFor,
        Duration<double> DelayUntilOffset);

    public sealed record Response(Moment Observed);

    public static Flow<RT, Request, Response> Get() =>
        Flow<RT, Request>.Asks(static request => request) >>
        (request => AlgebraEnv<TemporalProbeAlgebra, RT>
            .run(TemporalProbeWork.Program(
                request.DelayFor,
                request.DelayUntilOffset))
            .Map(observed => new Response(observed)));
}

public static class TemporalProbeWork
{
    public static Free<TemporalProbeAlgebra, Moment> Program(
        Duration<double> delayFor,
        Duration<double> delayUntilOffset) =>
        from now in Clock.now<TemporalProbeAlgebra>()
        from _ in Delay.forDuration<TemporalProbeAlgebra>(delayFor)
        from __ in Delay.until<TemporalProbeAlgebra>(
            now + delayUntilOffset)
        select now;
}

public abstract record TemporalProbeWorkPart<A> : K<TemporalProbeAlgebra, A>;

public sealed record TemporalProbeNowPart<A>(
    Func<Moment, A> Next)
    : TemporalProbeWorkPart<A>;

public sealed record TemporalProbeDelayForPart<A>(
    Duration<double> Duration,
    Func<Unit, A> Next)
    : TemporalProbeWorkPart<A>;

public sealed record TemporalProbeDelayUntilPart<A>(
    Moment Moment,
    Func<Unit, A> Next)
    : TemporalProbeWorkPart<A>;

public sealed class TemporalProbeAlgebra :
    Functor<TemporalProbeAlgebra>,
    Clock<TemporalProbeAlgebra>,
    Delay<TemporalProbeAlgebra>
{
    static K<TemporalProbeAlgebra, Moment>
        Clock<TemporalProbeAlgebra>.Now() =>
        new TemporalProbeNowPart<Moment>(static moment => moment);

    static K<TemporalProbeAlgebra, Unit>
        Delay<TemporalProbeAlgebra>.For(Duration<double> duration) =>
        new TemporalProbeDelayForPart<Unit>(
            duration,
            static value => value);

    static K<TemporalProbeAlgebra, Unit>
        Delay<TemporalProbeAlgebra>.Until(Moment moment) =>
        new TemporalProbeDelayUntilPart<Unit>(
            moment,
            static value => value);

    static K<TemporalProbeAlgebra, B> Functor<TemporalProbeAlgebra>.Map<A, B>(
        Func<A, B> f,
        K<TemporalProbeAlgebra, A> ma) =>
        ma switch
        {
            TemporalProbeNowPart<A>(var next) =>
                new TemporalProbeNowPart<B>(
                    moment => f(next(moment))),
            TemporalProbeDelayForPart<A>(var duration, var next) =>
                new TemporalProbeDelayForPart<B>(
                    duration,
                    value => f(next(value))),
            TemporalProbeDelayUntilPart<A>(var moment, var next) =>
                new TemporalProbeDelayUntilPart<B>(
                    moment,
                    value => f(next(value))),
            _ => throw new NotSupportedException()
        };
}

public sealed class TemporalProbeIO(
    ClockIO clock,
    DelayIO delay)
    : AlgebraIO<TemporalProbeAlgebra>
{
    public IO<A> Interpret<A>(
        K<TemporalProbeAlgebra, A> operation) =>
        operation switch
        {
            TemporalProbeNowPart<A> now =>
                clock.Now.Map(now.Next),
            TemporalProbeDelayForPart<A> wait =>
                delay.For(wait.Duration).Map(wait.Next),
            TemporalProbeDelayUntilPart<A> wait =>
                delay.Until(wait.Moment).Map(wait.Next),
            _ => throw new NotSupportedException()
        };
}

public sealed record TemporalRuntime(AlgebraIO<TemporalProbeAlgebra> Algebra)
    : HasAlgebra<TemporalProbeAlgebra, TemporalRuntime>
{
    static K<Eff<TemporalRuntime>, AlgebraIO<TemporalProbeAlgebra>>
        Has<Eff<TemporalRuntime>, AlgebraIO<TemporalProbeAlgebra>>.Ask { get; } =
        liftEff<TemporalRuntime, AlgebraIO<TemporalProbeAlgebra>>(rt => rt.Algebra);
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
