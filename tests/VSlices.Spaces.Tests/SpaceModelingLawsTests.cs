using VSlices.Spaces;
using VSlices.Spaces.Laws;
using VSlices.Spaces.Traits;

namespace VSlices.Spaces.Tests.Fixtures.Definitions
{
    // Deliberately invalid fixture: no semantic Transform, public constructor.
    public sealed class UncheckedPoint
    {
        public UncheckedPoint() { }
    }

    // Deliberately invalid fixture: State constructor is internal, not private.
    public sealed class LeakyPoint :
        Transformable<string, LeakyPoint>,
        Evolvable<LeakyPoint, LeakyPoint.State>
    {
        public sealed record State
        {
            internal State(string name) => Name = name;
            public string Name { get; init; }
        }

        private LeakyPoint(State state) => CurrentState = state;
        public State CurrentState { get; }
        public static LanguageExt.Fin<LeakyPoint> Transform(string name) =>
            new LeakyPoint(new State(name));
        public static LanguageExt.Fin<LeakyPoint> Evolve(State old, State @new) =>
            new LeakyPoint(@new);
    }

    public sealed class ValidPoint : Transformable<string, ValidPoint>
    {
        private ValidPoint(string name) => Name = name;
        public string Name { get; }
        public static LanguageExt.Fin<ValidPoint> Transform(string name) =>
            new ValidPoint(name);
    }
}

namespace VSlices.Spaces.Tests
{
    public class SpaceModelingLawsTests
    {
        [Fact]
        public void Consumer_can_scan_definitions_and_receive_specific_guidance()
        {
            var violations = SpaceModelingLaws.InspectDefinitions(
                typeof(Fixtures.Definitions.ValidPoint).Assembly,
                "VSlices.Spaces.Tests.Fixtures.Definitions");

            Assert.Contains(violations, v => v.RuleId == "SM001" && v.Definition.EndsWith("UncheckedPoint"));
            Assert.Contains(violations, v => v.RuleId == "SM002" && v.Definition.EndsWith("UncheckedPoint"));
            Assert.Contains(violations, v => v.RuleId == "SM004" && v.Definition.EndsWith("LeakyPoint"));
            Assert.All(violations, v =>
            {
                Assert.Contains("docs/spaces/modeling.md#sm00", v.GuidanceUrl);
                Assert.Contains(v.GuidanceUrl, v.ToString());
            });
        }

        [Fact]
        public void Valid_space_satisfies_structural_laws()
        {
            Assert.Empty(SpaceModelingLaws.Inspect(
                typeof(Fixtures.Definitions.ValidPoint)));
        }
    }
}
