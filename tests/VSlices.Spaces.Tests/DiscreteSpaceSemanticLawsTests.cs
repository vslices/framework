using VSlices.Spaces;
using VSlices.Spaces.Laws;

namespace VSlices.Spaces.Tests;

public class DiscreteSpaceSemanticLawsTests
{
    private sealed record GoodValue(int Value) : DiscreteSpace<GoodValue>;

    private sealed class InconsistentHash : DiscreteSpace<InconsistentHash>
    {
        private readonly int value;
        private readonly int hash;
        public InconsistentHash(int value, int hash) { this.value = value; this.hash = hash; }
        public bool Equals(InconsistentHash? other) => other is not null && value == other.value;
        public override bool Equals(object? obj) => obj is InconsistentHash other && Equals(other);
        public override int GetHashCode() => hash;
        public static bool operator ==(InconsistentHash? a, InconsistentHash? b) =>
            ReferenceEquals(a, b) || (a is not null && a.Equals(b));
        public static bool operator !=(InconsistentHash? a, InconsistentHash? b) => !(a == b);
    }

    private sealed class NonTransitive : DiscreteSpace<NonTransitive>
    {
        private readonly int value;
        public NonTransitive(int value) => this.value = value;
        public bool Equals(NonTransitive? other) =>
            other is not null && Math.Abs(value - other.value) <= 1;
        public override bool Equals(object? obj) => obj is NonTransitive other && Equals(other);
        public override int GetHashCode() => 0;
        public static bool operator ==(NonTransitive? a, NonTransitive? b) =>
            ReferenceEquals(a, b) || (a is not null && a.Equals(b));
        public static bool operator !=(NonTransitive? a, NonTransitive? b) => !(a == b);
    }

    [Fact]
    public void Consistent_witnesses_pass()
    {
        Assert.Empty(DiscreteSpaceSemanticLaws.Verify(
            new GoodValue(1), new GoodValue(1), new GoodValue(2)));
    }

    [Fact]
    public void Equal_witnesses_with_different_hashes_are_reported()
    {
        var issues = DiscreteSpaceSemanticLaws.Verify(
            new InconsistentHash(1, 101), new InconsistentHash(1, 202),
            new InconsistentHash(2, 303));
        Assert.Contains(issues, i => i.RuleId == "DS005");
    }

    [Fact]
    public void Nontransitive_witnesses_are_reported()
    {
        var issues = DiscreteSpaceSemanticLaws.Verify(
            new NonTransitive(1), new NonTransitive(2), new NonTransitive(3));
        Assert.Contains(issues, i => i.RuleId == "DS003");
    }
}
