using System.Runtime.CompilerServices;
using LanguageExt;
using VSlices.Work;
using VSlices.Work.Laws;
using Xunit;

namespace VSlices.Work.Tests;

public sealed class FeatureModelingLawsTests
{
    [Fact]
    public void Valid_feature_satisfies_all_structural_laws()
    {
        Assert.Empty(FeatureModelingLaws.Inspect(typeof(ValidFeature)));
    }

    [Theory]
    [InlineData(typeof(ExternalRequestFeature), "FM002")]
    [InlineData(typeof(ExternalRequestFeature), "FM004")]
    [InlineData(typeof(PublicResponseConstructor), "FM006")]
    [InlineData(typeof(PublicResponseConstructor), "FM008")]
    [InlineData(typeof(ExternallyMutableResponse), "FM010")]
    [InlineData(typeof(NullableRequest), "FM009")]
    [InlineData(typeof(NullableResponse), "FM009")]
    [InlineData(typeof(NonPublicRequestConstruction), "FM005")]
    public void Reports_exact_structural_failure(Type feature, string id)
    {
        Assert.Contains(FeatureModelingLaws.Inspect(feature), issue => issue.RuleId == id);
    }

    [Fact]
    public void Definitions_inspection_only_includes_Features()
    {
        var violations = FeatureModelingLaws.InspectDefinitions(
            typeof(FeatureModelingLawsTests).Assembly, typeof(ValidFeature).Namespace!);
        Assert.Contains(violations, item => item.RuleId == "FM006");
        Assert.DoesNotContain(violations, item => item.Definition == typeof(ValidFeature).FullName);
    }

    public sealed class ValidFeature : Feature<ValidFeature, string, ValidFeature.Request, ValidFeature.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed record ExternalRequest(string Value);
    public sealed class ExternalRequestFeature :
        Feature<ExternalRequestFeature, string, ExternalRequest, ExternalRequestFeature.Response>
    {
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(ExternalRequest request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed class PublicResponseConstructor :
        Feature<PublicResponseConstructor, string, PublicResponseConstructor.Request, PublicResponseConstructor.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response(string Value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(new Response(request.Value)));
    }

    public sealed class ExternallyMutableResponse :
        Feature<ExternallyMutableResponse, string, ExternallyMutableResponse.Request, ExternallyMutableResponse.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; set; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed class NullableRequest :
        Feature<NullableRequest, string, NullableRequest.Request, NullableRequest.Response>
    {
        public sealed record Request(string? Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value ?? "")));
    }

    public sealed class NullableResponse :
        Feature<NullableResponse, string, NullableResponse.Request, NullableResponse.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string? value) => Value = value;
            public string? Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string? value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed class NonPublicRequestConstruction :
        Feature<NonPublicRequestConstruction, string, NonPublicRequestConstruction.Request, NonPublicRequestConstruction.Response>
    {
        public sealed record Request
        {
            private Request(string value) => Value = value;
            public string Value { get; }
            public static Request Create(string value) => new(value);
        }
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }
}
