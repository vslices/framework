using VSlices.Services;

namespace VSlices.Application;

/// <summary>
/// Represents a service-owned executable feature.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="RT">The runtime required to execute the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
public interface ServiceFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ServiceFeature<F, RT, REQ, RES>
{
    /// <summary>
    /// 
    /// </summary>
    static abstract string UniqueName { get; }

    /// <summary>
    /// 
    /// </summary>
    static abstract string Description { get; }

    /// <summary>
    /// 
    /// </summary>
    static virtual ServiceClaim Claim =>
        ServiceClaim.New<F>(
            F.UniqueName,
            F.Description);
}

/// <summary>
/// Represents a completion-only service feature.
/// </summary>
public interface ServiceFeature<F, RT, REQ> :
    ServiceFeature<F, RT, REQ, Unit>
    where F : ServiceFeature<F, RT, REQ>;
