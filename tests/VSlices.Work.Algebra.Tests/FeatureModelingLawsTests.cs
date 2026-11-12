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
    [InlineData(typeof(ExternalRequestFeature), "FM002", "fm002-request-ownership")]
    [InlineData(typeof(ExternalRequestFeature), "FM004", "fm004-nominal-binding")]
    [InlineData(typeof(PublicResponseConstructor), "FM006", "fm006-response-construction")]
    [InlineData(typeof(PublicResponseConstructor), "FM008", "fm008-response-factory")]
    [InlineData(typeof(ExternallyMutableResponse), "FM010", "fm010-response-immutability")]
    [InlineData(typeof(NullableRequest), "FM009", "fm009-explicit-optionality")]
    [InlineData(typeof(NullableResponse), "FM009", "fm009-explicit-optionality")]
    [InlineData(typeof(NonPublicRequestConstruction), "FM005", "fm005-request-construction")]
    [InlineData(typeof(MissingNestedResponse), "FM003", "fm003-response-ownership")]
    [InlineData(typeof(MissingNestedResponse), "FM004", "fm004-nominal-binding")]
    [InlineData(typeof(WrongResponseFactory), "FM008", "fm008-response-factory")]
    [InlineData(typeof(InternalResponseFactory), "FM008", "fm008-response-factory")]
    [InlineData(typeof(WrongSelfFeature), "FM001", "fm001-feature-identity")]
    [InlineData(typeof(PublicInitResponse), "FM010", "fm010-response-immutability")]
    public void Reports_exact_structural_failure(Type feature, string id, string documentAnchor)
    {
        var violation = Assert.Single(
            FeatureModelingLaws.Inspect(feature), issue => issue.RuleId == id);

        Assert.Equal(feature.FullName, violation.Definition);
        Assert.False(string.IsNullOrWhiteSpace(violation.Error));
        Assert.Equal(FeatureModelingLaws.Guide + "#" + documentAnchor, violation.GuidanceUrl);
        Assert.Contains("[" + id + "]", violation.ToString());
        Assert.Contains(violation.Error, violation.ToString());
        Assert.Contains(violation.GuidanceUrl, violation.ToString());
    }

    [Fact]
    public void Consumer_scan_is_scoped_and_reports_owners()
    {
        var violations = FeatureModelingLaws.InspectDefinitions(
            typeof(FeatureModelingLawsTests).Assembly, typeof(ValidTopLevelFeature).Namespace!);

        Assert.Contains(violations, v => v.RuleId == "FM006"
            && v.Definition == typeof(InvalidTopLevelFeature).FullName);
        Assert.DoesNotContain(violations, v => v.Definition == typeof(ValidTopLevelFeature).FullName);
        Assert.DoesNotContain(violations, v => v.Definition == typeof(FeatureModelingLawsTests).FullName);
    }

    [Fact]
    public void Definitions_inspection_only_includes_Features()
    {
        var violations = FeatureModelingLaws.InspectDefinitions(
            typeof(FeatureModelingLawsTests).Assembly, typeof(ValidFeature).Namespace!);
        Assert.Contains(violations, item =>
            item.RuleId == "FM006" && item.Definition == typeof(InvalidTopLevelFeature).FullName);
        Assert.DoesNotContain(violations, item =>
            item.Definition == typeof(ValidTopLevelFeature).FullName);
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

    public sealed record ExternalResponse(string Value);

    public sealed class MissingNestedResponse :
        Feature<MissingNestedResponse, string, MissingNestedResponse.Request, ExternalResponse>
    {
        public sealed record Request(string Value);
        public static Eff<string, ExternalResponse> Get(Request request) =>
            Eff<string, ExternalResponse>.LiftIO(_ => IO.pure(new ExternalResponse(request.Value)));
    }

    public sealed class WrongResponseFactory :
        Feature<WrongResponseFactory, string, WrongResponseFactory.Request, WrongResponseFactory.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(int value);

        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(default(Response)!));
    }

    public sealed class PublicInitResponse :
        Feature<PublicInitResponse, string, PublicInitResponse.Request, PublicInitResponse.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; init; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed class InternalResponseFactory :
        Feature<InternalResponseFactory, string, InternalResponseFactory.Request, InternalResponseFactory.Response>
    {
        public sealed record Request(string Value);
        public sealed record Response
        {
            private Response(string value) => Value = value;
            public string Value { get; }
        }
        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        internal static extern Response NewResponse(string value);
        public static Eff<string, Response> Get(Request request) =>
            Eff<string, Response>.LiftIO(_ => IO.pure(NewResponse(request.Value)));
    }

    public sealed class WrongSelfFeature :
        Feature<ValidFeature, string, ValidFeature.Request, ValidFeature.Response>
    {
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

public sealed class ValidTopLevelFeature :
    Feature<ValidTopLevelFeature, string, ValidTopLevelFeature.Request, ValidTopLevelFeature.Response>
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

public sealed class InvalidTopLevelFeature :
    Feature<InvalidTopLevelFeature, string, InvalidTopLevelFeature.Request, InvalidTopLevelFeature.Response>
{
    public sealed record Request(string Value);
    public sealed record Response(string Value);

    public static Eff<string, Response> Get(Request request) =>
        Eff<string, Response>.LiftIO(_ => IO.pure(new Response(request.Value)));
}
