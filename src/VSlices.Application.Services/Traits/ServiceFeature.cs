using VSlices.Services;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface ServiceFeature<F, REQ, RES> : Feature<F, REQ, RES>
    where F : ServiceFeature<F, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract AppClaim ExecutableBy { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="REQ"></typeparam>
public interface ServiceFeature<F, REQ> : ServiceFeature<F, REQ, Unit>
    where F : ServiceFeature<F, REQ>;
