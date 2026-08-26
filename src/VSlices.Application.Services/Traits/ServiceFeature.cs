using VSlices.Services;

namespace VSlices.Application;

public interface ServiceFeature<F, RT, REQ, RES> : Feature<F, RT, REQ, RES>
    where F : ServiceFeature<F, RT, REQ, RES>
{
    static abstract ServiceClaim ExecutableBy { get; }
}

public interface ServiceFeature<F, RT, REQ> : ServiceFeature<F, RT, REQ, Unit>
    where F : ServiceFeature<F, RT, REQ>;
