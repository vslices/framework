using VSlices.Products;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface ProductFeature<F, REQ, RES> : Feature<F, REQ, RES>
    where F : ProductFeature<F, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract AppRole ExecutableBy { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="REQ"></typeparam>
public interface ProductFeature<F, REQ> : ProductFeature<F, REQ, Unit>
    where F : ProductFeature<F, REQ>;
