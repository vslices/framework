#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace VSlices.Traits;

public static class CategoryLaws
{
    public static void LeftIdentity<F, A, B>(
        K<F, A, B> f,
        Func<K<F, A, B>, K<F, A, B>, bool> equivalent)
        where F : Category<F>
    {
        var actual =
            F.Compose(
                F.Identity<A>(),
                f);

        if (!equivalent(actual, f))
            throw new Exception("Category law failed: left identity");
    }

    public static void RightIdentity<F, A, B>(
        K<F, A, B> f,
        Func<K<F, A, B>, K<F, A, B>, bool> equivalent)
        where F : Category<F>
    {
        var actual =
            F.Compose(
                f,
                F.Identity<B>());

        if (!equivalent(actual, f))
            throw new Exception("Category law failed: right identity");
    }

    public static void Associativity<F, A, B, C, D>(
        K<F, A, B> f,
        K<F, B, C> g,
        K<F, C, D> h,
        Func<K<F, A, D>, K<F, A, D>, bool> equivalent)
        where F : Category<F>
    {
        var left =
            F.Compose(
                F.Compose(f, g),
                h);

        var right =
            F.Compose(
                f,
                F.Compose(g, h));

        if (!equivalent(left, right))
            throw new Exception("Category law failed: associativity");
    }
}
