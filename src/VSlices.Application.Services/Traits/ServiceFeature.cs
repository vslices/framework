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
    static abstract string UniqueName { get; }

    static abstract string Description { get; }

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
