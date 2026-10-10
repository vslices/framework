using LanguageExt;
using LanguageExt.Traits;
using VSlices.Services;

namespace VSlices.Work;

public interface ServiceFeature<F, ALG, REQ, RES> :
    Feature<ALG, REQ, RES>
    where ALG : Functor<ALG>
    where F : ServiceFeature<F, ALG, REQ, RES>
{
    static abstract string UniqueName { get; }
    static abstract string Description { get; }

    static virtual ServiceClaim Claim =>
        ServiceClaim.New<F>(F.UniqueName, F.Description);
}

public interface ServiceFeature<F, ALG, REQ> :
    ServiceFeature<F, ALG, REQ, Unit>
    where ALG : Functor<ALG>
    where F : ServiceFeature<F, ALG, REQ>;
