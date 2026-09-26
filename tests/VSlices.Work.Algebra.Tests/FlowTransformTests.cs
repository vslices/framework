using LanguageExt;
using VSlices;
using VSlices.Monads;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class FlowTransformTests
{
    [Fact]
    public async Task Runtime_mapping_obeys_identity()
    {
        var flow = BaseFlow();
        var mapped = flow.MapRuntime(
            (BaseRuntime runtime) => runtime);

        var runtime = new BaseRuntime(7);
        var request = new BaseRequest("x");

        var expected = await flow
            .RunFlow(runtime, request)
            .RunAsync();

        var actual = await mapped
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Runtime_mapping_obeys_composition()
    {
        var flow = BaseFlow();

        var stepwise = flow
            .MapRuntime(
                (OuterRuntime runtime) => runtime.Inner)
            .MapRuntime(
                (TopRuntime runtime) => runtime.Inner);

        var composed = flow.MapRuntime(
            (TopRuntime runtime) => runtime.Inner.Inner);

        var runtime = new TopRuntime(
            new OuterRuntime(
                new BaseRuntime(11)));

        var request = new BaseRequest("runtime");

        var expected = await composed
            .RunFlow(runtime, request)
            .RunAsync();

        var actual = await stepwise
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Request_mapping_obeys_identity()
    {
        var flow = BaseFlow();
        var mapped = flow.MapRequest(
            (BaseRequest request) => request);

        var runtime = new BaseRuntime(13);
        var request = new BaseRequest("request");

        var expected = await flow
            .RunFlow(runtime, request)
            .RunAsync();

        var actual = await mapped
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task Request_mapping_obeys_composition()
    {
        var flow = BaseFlow();

        var stepwise = flow
            .MapRequest(
                (OuterRequest request) => request.Inner)
            .MapRequest(
                (TopRequest request) => request.Inner);

        var composed = flow.MapRequest(
            (TopRequest request) => request.Inner.Inner);

        var runtime = new BaseRuntime(17);
        var request = new TopRequest(
            new OuterRequest(
                new BaseRequest("composed")));

        var expected = await composed
            .RunFlow(runtime, request)
            .RunAsync();

        var actual = await stepwise
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ContraMap_equals_independent_runtime_and_request_mapping()
    {
        var flow = BaseFlow();

        var separate = flow
            .MapRuntime(
                (OuterRuntime runtime) => runtime.Inner)
            .MapRequest(
                (OuterRequest request) => request.Inner);

        var together = flow.ContraMap(
            (OuterRuntime runtime) => runtime.Inner,
            (OuterRequest request) => request.Inner);

        var runtime = new OuterRuntime(
            new BaseRuntime(23));

        var request = new OuterRequest(
            new BaseRequest("both"));

        var expected = await separate
            .RunFlow(runtime, request)
            .RunAsync();

        var actual = await together
            .RunFlow(runtime, request)
            .RunAsync();

        Assert.Equal(expected, actual);
    }

    private static Flow<BaseRuntime, BaseRequest, string> BaseFlow() =>
        new((runtime, request) =>
            IO.pure($"{runtime.Value}:{request.Value}"));

    public sealed record BaseRuntime(int Value);
    public sealed record OuterRuntime(BaseRuntime Inner);
    public sealed record TopRuntime(OuterRuntime Inner);

    public sealed record BaseRequest(string Value);
    public sealed record OuterRequest(BaseRequest Inner);
    public sealed record TopRequest(OuterRequest Inner);
}
