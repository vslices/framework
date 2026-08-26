using VSlices.Products;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface ProductFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ProductFeature<F, RT, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract ProductRole ExecutableBy { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
public interface ProductFeature<F, RT, REQ> : ProductFeature<F, RT, REQ, Unit>
    where F : ProductFeature<F, RT, REQ>;
