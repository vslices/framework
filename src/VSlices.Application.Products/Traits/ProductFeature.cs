using VSlices.Products;

namespace VSlices.Application;

/// <summary>
/// Represents a product-owned executable feature.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="RT">The runtime required to execute the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
public interface ProductFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ProductFeature<F, RT, REQ, RES>
{
    static abstract ProductRole ExecutableBy { get; }
}
