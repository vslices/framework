using VSlices.Services;

namespace VSlices.Application;

/// <summary>
/// 
/// </summary>
/// <typeparam name="F"></typeparam>
/// <typeparam name="RT"></typeparam>
/// <typeparam name="REQ"></typeparam>
/// <typeparam name="RES"></typeparam>
public interface ServiceFeature<F, RT, REQ, RES>
    where F : ServiceFeature<F, RT, REQ, RES>
{
    static abstract vClaim ExecutableBy { get; }
}

/// <summary>
/// Represents a feature interface that defines a contract for implementing a specific functionality
/// with a runtime environment and request type.
/// </summary>
/// <remarks>
/// This interface is a specialized version of <see cref="ServiceFeature{F, RT, REQ, RES}"/> where the response type is <see cref="Unit"/>.
/// It is designed to be implemented by concrete feature classes, serving as the base for functionalities
/// that do not produce a specific response output.
/// </remarks>
/// <typeparam name="F">
/// The type of the feature implementation. Must inherit from <see cref="Feature{F, RT, REQ}"/>.
/// </typeparam>
/// <typeparam name="RT">The type of the runtime environment required for the feature.</typeparam>
/// <typeparam name="REQ">The type of the request input for the feature.</typeparam>
public interface ServiceFeature<F, RT, REQ> : ServiceFeature<F, RT, REQ, Unit>
    where F : ServiceFeature<F, RT, REQ>;
