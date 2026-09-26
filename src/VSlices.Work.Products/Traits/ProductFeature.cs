using VSlices.Products;

namespace VSlices.Work;

/// <summary>
/// Represents a product-owned executable feature.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="ALG">The executable algebra required to run the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
public interface ProductFeature<F, ALG, REQ, RES> : Feature<F, ALG, REQ, RES>
    where F : ProductFeature<F, ALG, REQ, RES>
{
    static abstract ProductRole ExecutableBy { get; }
}
