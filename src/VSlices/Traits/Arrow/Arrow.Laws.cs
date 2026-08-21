namespace VSlices.Traits;

public static class ArrowLaws
{
    public static void LiftIdentity<F, A>(
        Func<K<F, A, A>, K<F, A, A>, bool> equivalent)
        where F : Arrow<F>
    {
        var liftedIdentity =
            F.Lift<A, A>(x => x);

        var categoryIdentity =
            F.Identity<A>();

        if (!equivalent(liftedIdentity, categoryIdentity))
            throw new Exception("Arrow law failed: Lift(identity) == Identity");
    }

    public static void LiftComposition<F, A, B, C>(
        Func<A, B> f,
        Func<B, C> g,
        Func<K<F, A, C>, K<F, A, C>, bool> equivalent)
        where F : Arrow<F>
    {
        var composedLift =
            F.Compose(
                F.Lift(f),
                F.Lift(g));

        var liftedComposition =
            F.Lift<A, C>(
                x => g(f(x)));

        if (!equivalent(composedLift, liftedComposition))
            throw new Exception("Arrow law failed: Lift composition");
    }

    public static void FirstIdentity<F, A, X>(
        Func<K<F, (A, X), (A, X)>, K<F, (A, X), (A, X)>, bool> equivalent)
        where F : Arrow<F>
    {
        var firstIdentity =
            F.First<A, A, X>(
                F.Identity<A>());

        var tupleIdentity =
            F.Identity<(A, X)>();

        if (!equivalent(firstIdentity, tupleIdentity))
            throw new Exception("Arrow law failed: First(identity) == Identity");
    }

    public static void FirstComposition<F, A, B, C, X>(
        K<F, A, B> f,
        K<F, B, C> g,
        Func<K<F, (A, X), (C, X)>, K<F, (A, X), (C, X)>, bool> equivalent)
        where F : Arrow<F>
    {
        var firstOfComposition =
            F.First<A, C, X>(
                F.Compose(f, g));

        var compositionOfFirsts =
            F.Compose(
                F.First<A, B, X>(f),
                F.First<B, C, X>(g));

        if (!equivalent(firstOfComposition, compositionOfFirsts))
            throw new Exception("Arrow law failed: First composition");
    }

    public static void SecondDefinition<F, A, B, X>(
        K<F, A, B> f,
        Func<K<F, (X, A), (X, B)>, K<F, (X, A), (X, B)>, bool> equivalent)
        where F : Arrow<F>
    {
        var actual =
            F.Second<A, B, X>(f);

        var expected =
            F.Compose(
                F.Lift<(X, A), (A, X)>(
                    x => (x.Item2, x.Item1)),
                F.Compose(
                    F.First<A, B, X>(f),
                    F.Lift<(B, X), (X, B)>(
                        x => (x.Item2, x.Item1))));

        if (!equivalent(actual, expected))
            throw new Exception("Arrow derived law failed: Second");
    }

    public static void SplitDefinition<F, A, B, C, D>(
        K<F, A, B> f,
        K<F, C, D> g,
        Func<K<F, (A, C), (B, D)>, K<F, (A, C), (B, D)>, bool> equivalent)
        where F : Arrow<F>
    {
        var actual =
            F.Split(f, g);

        var expected =
            F.Compose(
                F.First<A, B, C>(f),
                F.Second<C, D, B>(g));

        if (!equivalent(actual, expected))
            throw new Exception("Arrow derived law failed: Split");
    }

    public static void FanoutDefinition<F, A, B, C>(
        K<F, A, B> f,
        K<F, A, C> g,
        Func<K<F, A, (B, C)>, K<F, A, (B, C)>, bool> equivalent)
        where F : Arrow<F>
    {
        var actual =
            F.Fanout(f, g);

        var expected =
            F.Compose(
                F.Lift<A, (A, A)>(
                    x => (x, x)),
                F.Split(f, g));

        if (!equivalent(actual, expected))
            throw new Exception("Arrow derived law failed: Fanout");
    }

    public static void ConvergeDefinition<F, A, B, C, D>(
        K<F, A, B> f,
        K<F, A, C> g,
        K<F, (B, C), D> join,
        Func<K<F, A, D>, K<F, A, D>, bool> equivalent)
        where F : Arrow<F>
    {
        var actual =
            F.Converge(f, g, join);

        var expected =
            F.Compose(
                F.Fanout(f, g),
                join);

        if (!equivalent(actual, expected))
            throw new Exception("Arrow derived law failed: Converge");
    }
}
