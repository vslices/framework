using System;
using System.Linq;
using System.Runtime.CompilerServices;
using LanguageExt;
using VSlices.Spaces;
using VSlices.Spaces.Laws;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Tests.Fixtures.Definitions
{
    // SM001: the definition has no target-owned Transformable rule.
    public sealed class MissingTransformPoint
    {
        public readonly record struct Input(string Value);
        private MissingTransformPoint() { }
    }

    // SM002: the public constructor bypasses its otherwise valid Transform.
    public sealed class PublicConstructorPoint :
        Transformable<PublicConstructorPoint.Input, PublicConstructorPoint>
    {
        public readonly record struct Input(string Value);
        public PublicConstructorPoint(string value) => Value = value;
        public string Value { get; }
        public static Fin<PublicConstructorPoint> Transform(Input input) =>
            new PublicConstructorPoint(input.Value);
    }

    // SM003: the proposed State is owned by another type instead of the point.
    public sealed record ExternalState
    {
        private ExternalState(string value) => Value = value;
        public string Value { get; }
        public static ExternalState Create(string value) => new(value);
    }

    public sealed class ExternalStatePoint :
        Transformable<ExternalStatePoint.Input, ExternalStatePoint>,
        Evolvable<ExternalStatePoint, ExternalState>
    {
        public readonly record struct Input(string Value);
        private ExternalStatePoint(ExternalState state) => CurrentState = state;
        public ExternalState CurrentState { get; }
        public static Fin<ExternalStatePoint> Transform(Input input) =>
            new ExternalStatePoint(ExternalState.Create(input.Value));
        public static Fin<ExternalStatePoint> Evolve(ExternalState old, ExternalState @new) =>
            new ExternalStatePoint(@new);
    }

    // SM004: an internal State constructor broadens authority beyond the owner.
    public sealed class LeakyStateConstructorPoint :
        Transformable<LeakyStateConstructorPoint.Input, LeakyStateConstructorPoint>,
        Evolvable<LeakyStateConstructorPoint, LeakyStateConstructorPoint.State>
    {
        public readonly record struct Input(string Value);
        public sealed record State
        {
            internal State(string value) => Value = value;
            public string Value { get; init; }
        }

        private LeakyStateConstructorPoint(State state) => CurrentState = state;
        public State CurrentState { get; }
        public static Fin<LeakyStateConstructorPoint> Transform(Input input) =>
            new LeakyStateConstructorPoint(new State(input.Value));
        public static Fin<LeakyStateConstructorPoint> Evolve(State old, State @new) =>
            new LeakyStateConstructorPoint(@new);
    }

    // SM005: consumers can replace the accepted State directly.
    public sealed class SettableCurrentStatePoint :
        Transformable<SettableCurrentStatePoint.Input, SettableCurrentStatePoint>,
        Evolvable<SettableCurrentStatePoint, SettableCurrentStatePoint.State>
    {
        public readonly record struct Input(string Value);
        public sealed record State
        {
            private State(string value) => Value = value;
            public string Value { get; init; }
            public static State Create(string value) => new(value);
        }

        private SettableCurrentStatePoint(State state) => CurrentState = state;
        public State CurrentState { get; set; }
        public static Fin<SettableCurrentStatePoint> Transform(Input input) =>
            new SettableCurrentStatePoint(State.Create(input.Value));
        public static Fin<SettableCurrentStatePoint> Evolve(State old, State @new) =>
            new SettableCurrentStatePoint(@new);
    }

    // SM006: the Transformable input is external rather than nested in the owner.
    public readonly record struct ExternalInput(string Value);

    public sealed class ExternalInputPoint :
        Transformable<ExternalInput, ExternalInputPoint>
    {
        // Declared but deliberately NOT used by Transformable.
        public readonly record struct Input(string Value);
        private ExternalInputPoint(string value) => Value = value;
        public string Value { get; }
        public static Fin<ExternalInputPoint> Transform(ExternalInput input) =>
            new ExternalInputPoint(input.Value);
    }

    // SM007: a nested Input has nullable reference and nullable value members.
    public sealed class NullableInputPoint :
        Transformable<NullableInputPoint.Input, NullableInputPoint>
    {
        public readonly record struct Input(string? Name, int? Rank);
        private NullableInputPoint() { }
        public static Fin<NullableInputPoint> Transform(Input input) => new NullableInputPoint();
    }

    // SM007: State nullability is forbidden even when construction stays private.
    public sealed class NullableStatePoint :
        Transformable<NullableStatePoint.Input, NullableStatePoint>,
        Evolvable<NullableStatePoint, NullableStatePoint.State>
    {
        public readonly record struct Input(string Name);
        public sealed record State
        {
            private State(string? name) => Name = name;
            public string? Name { get; init; }
        }

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern State NewState(string? name);

        private NullableStatePoint(State state) => CurrentState = state;
        public State CurrentState { get; }
        public static Fin<NullableStatePoint> Transform(Input input) =>
            new NullableStatePoint(NewState(input.Name));
        public static Fin<NullableStatePoint> Evolve(State old, State @new) =>
            new NullableStatePoint(@new);
    }

    // Positive control: a complete definition obeys all structural rules.
    public sealed class ValidPoint :
        Transformable<ValidPoint.Input, ValidPoint>,
        Evolvable<ValidPoint, ValidPoint.State>,
        DiscreteSpace<ValidPoint>
    {
        public readonly record struct Input(string Value);
        public sealed record State
        {
            private State(string value) => Value = value;
            public string Value { get; init; }
        }

        [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
        private static extern State NewState(string value);

        private ValidPoint(State state) => CurrentState = state;
        public State CurrentState { get; }
        public static Fin<ValidPoint> Transform(Input input) =>
            new ValidPoint(NewState(input.Value));
        public static Fin<ValidPoint> Evolve(State old, State @new) =>
            new ValidPoint(@new);

        public bool Equals(ValidPoint? other) =>
            other is not null && CurrentState.Value == other.CurrentState.Value;
        public override bool Equals(object? obj) => obj is ValidPoint other && Equals(other);
        public override int GetHashCode() => CurrentState.Value.GetHashCode();
        public static bool operator ==(ValidPoint? left, ValidPoint? right) =>
            ReferenceEquals(left, right) || (left is not null && left.Equals(right));
        public static bool operator !=(ValidPoint? left, ValidPoint? right) => !(left == right);
    }
}

namespace VSlices.Spaces.Tests
{
    public class SpaceModelingLawsTests
    {
        [Theory]
        [InlineData(typeof(Fixtures.Definitions.MissingTransformPoint), "SM001", "sm001-semantic-construction")]
        [InlineData(typeof(Fixtures.Definitions.PublicConstructorPoint), "SM002", "sm002-constructor-encapsulation")]
        [InlineData(typeof(Fixtures.Definitions.ExternalStatePoint), "SM003", "sm003-evolution-state-ownership")]
        [InlineData(typeof(Fixtures.Definitions.LeakyStateConstructorPoint), "SM004", "sm004-state-constructor-encapsulation")]
        [InlineData(typeof(Fixtures.Definitions.SettableCurrentStatePoint), "SM005", "sm005-accepted-state-exposure")]
        [InlineData(typeof(Fixtures.Definitions.ExternalInputPoint), "SM006", "sm006-input-ownership")]
        public void Invalid_definition_explains_its_specific_law(
            Type definition, string ruleId, string documentAnchor)
        {
            var violations = SpaceModelingLaws.Inspect(definition);
            var violation = Assert.Single(violations, v => v.RuleId == ruleId);

            Assert.Equal(definition.FullName, violation.Definition);
            Assert.False(string.IsNullOrWhiteSpace(violation.Error));
            Assert.Equal(
                SpaceModelingLaws.Guide + "#" + documentAnchor,
                violation.GuidanceUrl);
            Assert.Contains("[" + ruleId + "]", violation.ToString());
            Assert.Contains(violation.Error, violation.ToString());
            Assert.Contains(violation.GuidanceUrl, violation.ToString());
        }

        [Fact]
        public void Consumer_scan_discovers_all_structural_laws_and_reports_their_owners()
        {
            var violations = SpaceModelingLaws.InspectDefinitions(
                typeof(Fixtures.Definitions.ValidPoint).Assembly,
                "VSlices.Spaces.Tests.Fixtures.Definitions");

            var expected = new[]
            {
                "SM001", "SM002", "SM003", "SM004", "SM005", "SM006", "SM007", "SM008"
            };

            foreach (var rule in expected)
                Assert.Contains(violations, v => v.RuleId == rule);

            Assert.DoesNotContain(
                violations, v => v.Definition == typeof(Fixtures.Definitions.ValidPoint).FullName);
        }

        [Fact]
        public void Complete_evolvable_definition_satisfies_all_structural_laws()
        {
            Assert.Empty(SpaceModelingLaws.Inspect(
                typeof(Fixtures.Definitions.ValidPoint)));
        }

        [Fact]
        public void SM007_identifies_nullable_members_in_both_owned_contracts()
        {
            var input = SpaceModelingLaws.Inspect(typeof(Fixtures.Definitions.NullableInputPoint));
            Assert.Contains(input, v => v.RuleId == "SM007" && v.Error.Contains("Input.Name"));
            Assert.Contains(input, v => v.RuleId == "SM007" && v.Error.Contains("Input.Rank"));

            var state = SpaceModelingLaws.Inspect(typeof(Fixtures.Definitions.NullableStatePoint));
            Assert.Contains(state, v => v.RuleId == "SM007" && v.Error.Contains("State.Name"));
            foreach (var violation in input.Concat(state).Where(v => v.RuleId == "SM007"))
            {
                Assert.Equal(SpaceModelingLaws.Guide + "#sm007-explicit-optionality", violation.GuidanceUrl);
                Assert.Contains(violation.GuidanceUrl, violation.ToString());
            }
        }

        [Fact]
        public void SM008_requires_nominal_DiscreteSpace_contract()
        {
            var missing = SpaceModelingLaws.Inspect(typeof(Fixtures.Definitions.PublicConstructorPoint));
            Assert.Contains(missing, v => v.RuleId == "SM008");
            Assert.Empty(DiscreteSpaceStructuralLaws.Inspect(typeof(Fixtures.Definitions.ValidPoint)));
        }

        [Fact]
        public void External_input_is_rejected_even_when_a_nested_input_exists()
        {
            // An Input declaration alone is insufficient unless Transformable uses it.
            var violations = SpaceModelingLaws.Inspect(typeof(Fixtures.Definitions.ExternalInputPoint));
            Assert.Contains(violations, v => v.RuleId == "SM006");
        }
    }
}
