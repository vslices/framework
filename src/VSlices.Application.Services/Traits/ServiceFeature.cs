using VSlices.Services;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface ServiceFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ServiceFeature<F, RT, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract ServiceClaim ExecutableBy { get; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
public interface ServiceFeature<F, RT, REQ> : ServiceFeature<F, RT, REQ, Unit>
    where F : ServiceFeature<F, RT, REQ>;
