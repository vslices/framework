using VSlices.Services;

namespace VSlices.Work;

/// <summary>
/// Represents a service-owned executable feature.
/// </summary>
/// <typeparam name="F">The concrete feature type.</typeparam>
/// <typeparam name="ALG">The executable algebra required to run the feature.</typeparam>
/// <typeparam name="REQ">The feature-owned request type.</typeparam>
/// <typeparam name="RES">The feature-owned response type.</typeparam>
public interface ServiceFeature<F, ALG, REQ, RES> : Feature<F, ALG, REQ, RES>
    where F : ServiceFeature<F, ALG, REQ, RES>
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
public interface ServiceFeature<F, ALG, REQ> :
    ServiceFeature<F, ALG, REQ, Unit>
    where F : ServiceFeature<F, ALG, REQ>;
