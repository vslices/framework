using VSlices.Spaces;

namespace VSlices.Spaces.Laws;

/// <summary>
/// Checks observed algebraic equality properties for caller-supplied witnesses.
/// It cannot establish universal correctness over a domain.
/// </summary>
public static class DiscreteSpaceSemanticLaws
{
    public static IReadOnlyList<SpaceLawViolation> Verify<A>(A left, A right, A third)
        where A : DiscreteSpace<A>
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        ArgumentNullException.ThrowIfNull(third);

        var values = new[] { left, right, third };
        var issues = new List<SpaceLawViolation>();
        var comparer = EqualityComparer<A>.Default;

        void Fail(string id, string error) =>
            issues.Add(new SpaceLawViolation(id, typeof(A).FullName ?? typeof(A).Name,
                error, SpaceModelingLaws.Guide + "#discrete-space-semantic-laws"));

        for (var i = 0; i < values.Length; i++)
        {
            var a = values[i];
            if (!a.Equals(a) || !comparer.Equals(a, a) || !(a == a) || a != a)
                Fail("DS001", "Equality is not reflexive for a supplied witness.");

            for (var j = 0; j < values.Length; j++)
            {
                var b = values[j];
                var ab = a.Equals(b);
                var ba = b.Equals(a);
                if (ab != ba)
                    Fail("DS002", "Equality is not symmetric for supplied witnesses.");

                if (ab != (a == b) || (a != b) == ab ||
                    ab != comparer.Equals(a, b))
                    Fail("DS004", "Equals, ==, != and EqualityComparer disagree.");

                if (ab && a.GetHashCode() != b.GetHashCode())
                    Fail("DS005", "Equal witnesses produce different hash codes.");

                for (var k = 0; k < values.Length; k++)
                {
                    var c = values[k];
                    if (ab && b.Equals(c) && !a.Equals(c))
                        Fail("DS003", "Equality is not transitive for supplied witnesses.");
                }
            }
        }

        return issues;
    }
}
